using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GolayCode
{
    public class StringConverter
    {
        private readonly ByteConverter _byteConverter;

        public StringConverter(ByteConverter byteConverter)
        {
            _byteConverter = byteConverter;
        }

        /// <summary>
        /// Tekstas paverčiamas į baitus, tada kitoje klasėje apdorojami baitai
        /// </summary>
        /// <param name="str">Tekstas</param>
        /// <seealso cref="ByteConverter"/>
        /// <returns>Vektoriai, pridėtų bitų skaičius</returns>
        public (List<Vector> Vectors, int AddedBits) ConvertStringToEncodableVectors(string str)
        {
            var bytes = Encoding.ASCII.GetBytes(str);

            return _byteConverter.ConvertBytesToEncodableVectors(bytes);
        }

        /// <summary>
        /// Vektoriai verčiami į tekstą.
        /// Vektoriai kitoje klasėje verčiami į baitus ir čia interpretuojami kaip tekstas.
        /// </summary>
        /// <seealso cref="ByteConverter"/>
        /// <param name="vectors">Konvertuotini vektoriai</param>
        /// <param name="addedBits">Pridėtų baitų skaičius</param>
        /// <returns></returns>
        public string ConvertVectorsToString(List<Vector> vectors, int addedBits)
        {
            var bytes = _byteConverter.ConvertVectorsToBytes(vectors, addedBits);

            return Encoding.ASCII.GetString(bytes.ToArray());
        }
    }
}