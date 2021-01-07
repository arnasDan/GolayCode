using System;
using System.Linq;

namespace GolayCode
{
    /// <summary>
    /// Kanalas, kuriuo siunčiami vektoriai
    /// </summary>
    public class Channel
    {
        private readonly Random _random = new Random();
        private decimal _errorProbability;

        /// <summary>
        /// Kanalo klaidos tikimybė
        /// </summary>
        public decimal ErrorProbability
        {
            get => _errorProbability;
            set
            {
                if (value < 0 || value > 1)
                    throw new ArgumentOutOfRangeException(nameof(ErrorProbability), "Must be between 0 and 1");

                _errorProbability = value;
            }
        }

        /// <summary>
        /// Vektorius išsiunčamas kanalu.
        /// Vektoriaus bitai iškraipomi su ErrorProbability tikimybe.
        /// </summary>
        /// <param name="vector">Vektorius, gautas kitame kanalo gale </param>
        /// <returns></returns>
        public Vector Send(Vector vector)
        {
            var errorProbability = ErrorProbability * 1000000;
            
            var resultingBits = vector.Select(bit =>
            {
                var random = _random.Next(0, 1000000);
                return random < errorProbability
                    ? bit.Invert()
                    : bit;
            });

            return new Vector(resultingBits);
        }
    }
}