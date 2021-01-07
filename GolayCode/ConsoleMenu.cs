using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GolayCode
{
    /// <summary>
    /// Konsolinis meniu / pagrindinė klasė vartotojo darbui su programa
    /// </summary>
    public class ConsoleMenu
    {
        private readonly GolayDecoder _decoder = new GolayDecoder();
        private readonly GolayEncoder _encoder = new GolayEncoder();
        private readonly Channel _channel = new Channel();
        private readonly ByteConverter _byteConverter = new ByteConverter();
        private readonly StringConverter _stringConverter;
        private readonly BitmapConverter _bitmapConverter;
        private readonly FileService _fileService = new FileService();

        /// <summary>
        /// Galimi veiksmai
        /// </summary>
        private readonly Dictionary<char, Action> _menuActions;

        public ConsoleMenu()
        {
            _bitmapConverter = new BitmapConverter(_byteConverter);
            _stringConverter = new StringConverter(_byteConverter);

            _menuActions = new Dictionary<char, Action>
            {
                { '1', HandleVector },
                { '2', HandleString },
                { '3', HandleBitmap },
                { '4', ChangeErrorProbability }
            };
        }

        /// <summary>
        /// Pagrindinis loop'as
        /// </summary>
        public void Start()
        {
            while (true)
            {
                var input = PromptUserWithMenu();
                Console.WriteLine();

                if (input == '0')
                    return;

                if (_menuActions.TryGetValue(input, out var action))
                {
                    try
                    {
                        action();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"An error occured: {e.Message}");
                    }
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Invalid choice");
                }
            }
        }

        /// <summary>
        /// Vykdomas 1 scenarijus:
        /// 1. Iš vartotojo priimamas vektorius;
        /// 2. Jei tinkamas, vektorius užkoduojamas ir parodomas vartotojui;
        /// 3. Užkoduotas vektorius siunčiamas kanalu, rezultaltas ir klaidos parodomi vartototojui, jį galima redaguoti;
        /// 3. Dekuoduojamas gautas vektorius iš 3) ir parodomas vartotojui.
        /// 
        /// </summary>
        private void HandleVector()
        {
            Console.Write("Enter vector: ");

            var vector = Console.ReadLine().ParseAsVector();

            vector = _encoder.EncodeVector(vector);
            Console.WriteLine($"Encoded vector: \n{vector}");

            Console.WriteLine($"Sending vector through channel, error probability {_channel.ErrorProbability}");

            var receivedVector = _channel.Send(vector);

            Console.WriteLine($"Resulting vector");

            var vectorString = ReadLineWithExistingValue(receivedVector.ToString());

            receivedVector = vectorString.ParseAsVector();

            var vectorDifference = vector ^ receivedVector;

            Console.WriteLine($"Error positions (1 where error occured):\n{vectorDifference}");
            Console.WriteLine($"Error count: {vectorDifference.Weight}");

            Console.WriteLine($"Decoded vector: \n{_decoder.DecodeVector(receivedVector)}");
        }

        /// <summary>
        /// Vykdomas 2 scenarijus:
        /// 1. Iš vartotojo priimamas tekstas;
        /// 2. Tekstas padalijamas į reikiamo ilgio vektorius;
        /// 3. Vektoriai siunčiami kanalu be kodo, atkuriamas tekstas ir parodomas;
        /// 3. Užkoduoti vektoriai siunčiami kanalu, atkuriamas tekstas ir parodomas;
        /// </summary>
        private void HandleString()
        {
            Console.Write("Enter string: ");
            var str = Console.ReadLine();

            var (vectors, addedBits) = _stringConverter.ConvertStringToEncodableVectors(str);

            Console.WriteLine($"Sending plain vectors through channel, error probability {_channel.ErrorProbability}");
            HandleStringVectors(vectors, addedBits, vector => _channel.Send(vector));

            Console.WriteLine($"Sending encoded vectors through channel, error probability {_channel.ErrorProbability}");
            HandleStringVectors(vectors, addedBits, vector =>
            {
                var encodedVector = _encoder.EncodeVector(vector);
                var receivedVector = _channel.Send(encodedVector);
                return _decoder.DecodeVector(receivedVector);
            });
        }

        /// <summary>
        /// Vykdomas 3 scenarijus:
        /// </summary>
        private void HandleBitmap()
        {
            Console.Write("Enter bitmap path: ");

            var bitmapPath = Console.ReadLine();

            _fileService.Open(bitmapPath);

            var bytes = _fileService.ReadAsBytes(bitmapPath);

            var (vectors, addedBits, header) = _bitmapConverter.ConvertBitmapToEncodableVectors(bytes);

            Console.WriteLine($"Sending plain vectors through channel, error probability {_channel.ErrorProbability}");
            HandleBitmapVectors(vectors, addedBits, header, bitmapPath + "_unencoded.bmp", vector => _channel.Send(vector));

            Console.WriteLine($"Sending encoded vectors through channel, error probability {_channel.ErrorProbability}");
            HandleBitmapVectors(vectors, addedBits, header, bitmapPath + "_encoded.bmp", vector =>
            {
                var encodedVector = _encoder.EncodeVector(vector);
                var receivedVector = _channel.Send(encodedVector);
                return _decoder.DecodeVector(receivedVector);
            });
        }

        /// <summary>
        /// Leidžia vartotojui pakeisti klaidos tikimybę
        /// </summary>
        private void ChangeErrorProbability()
        {
            Console.WriteLine("Enter error probability: ");

            var probabilityString = Console.ReadLine().Replace(",", ".");

            _channel.ErrorProbability = decimal.Parse(probabilityString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Transformuoja vektorius su duota funkcija ir paverčia juos į tekstą
        /// </summary>
        /// <param name="vectors">Vektoriai</param>
        /// <param name="addedBits">Kiek 0 bitų buvo pridėta prie paskutinio vektoriaus</param>
        /// <param name="transformationFunc">Transformacijos funkcija</param>
        private void HandleStringVectors(IEnumerable<Vector> vectors, int addedBits, Func<Vector, Vector> transformationFunc)
        {
            var transformedVectors = vectors.Select(transformationFunc);

            var str = _stringConverter.ConvertVectorsToString(transformedVectors.ToList(), addedBits);
            Console.WriteLine($"Resulting string: \n{str}");
        }

        /// <summary>
        /// Transformuoja vektorius su duota funkcija ir paverčia juos į paveiksliuką, kurį atidaro
        /// </summary>
        /// <param name="vectors">Vektoriai</param>
        /// <param name="addedBits">Kiek 0 bitų buvo pridėta prie paskutinio vektoriaus</param>
        /// <param name="header">Failo headeris</param>
        /// <param name="transformationFunc">Transformacijos funkcija</param>
        private void HandleBitmapVectors(IEnumerable<Vector> vectors, int addedBits, IEnumerable<byte> header, string path, Func<Vector, Vector> transformationFunc)
        {
            var transformedVectors = vectors.Select(transformationFunc);

            var bitmap = _bitmapConverter.ConvertVectorsToBitmap(transformedVectors.ToList(), addedBits, header);
            
            _fileService.WriteBytes(path, bitmap);
            _fileService.Open(path);
        }

        /// <summary>
        /// Vartotojui parodamas meniu ir užklausiama pasirinkomo
        /// </summary>
        /// <returns>Vartotojo pasirinkimas</returns>
        private static char PromptUserWithMenu()
        {
            Console.WriteLine("[0] Exit");
            Console.WriteLine("[1] Input vector");
            Console.WriteLine("[2] Input string");
            Console.WriteLine("[3] Input bitmap");
            Console.WriteLine("[4] Change error probability");
            Console.Write("Choose an option: ");

            return Console.ReadKey().KeyChar;
        }

        /// <summary>
        /// Rodomas tekstas, kurį vartotojas gali redaguoti
        /// </summary>
        /// <param name="existingValue">Redaguotinas tekstas</param>
        /// <returns></returns>
        private static string ReadLineWithExistingValue(string existingValue)
        {
            Console.Write(existingValue);

            var chars = new List<char>(new char[255]);
            if (!string.IsNullOrEmpty(existingValue)) {
                foreach (var (character, i) in existingValue.Select((character, i) => (character, i)))
                {
                    chars[i] = character;
                }
            }

            while (true)
            {
                var keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.LeftArrow:
                        Console.CursorLeft = Math.Max(0, Console.CursorLeft - 1);
                        break;
                    case ConsoleKey.RightArrow:
                        Console.CursorLeft = Math.Min(chars.Count, Console.CursorLeft + 1);
                        break;
                    case ConsoleKey.Backspace when Console.CursorLeft > 0:
                        chars[Console.CursorLeft - 1] = ' ';
                        Console.CursorLeft -= 1;
                        Console.Write(' ');
                        Console.CursorLeft -= 1;
                        break;
                    case ConsoleKey.Enter:
                        Console.Write(Environment.NewLine);
                        return new string(chars.ToArray()).Trim().Replace("\0", "");
                    default:
                        Console.Write(keyInfo.KeyChar);
                        chars[Console.CursorLeft] = keyInfo.KeyChar;
                        break;
                }
            }
            
        }
    }
}