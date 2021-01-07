using System.Collections.Generic;
using System.Linq;

namespace GolayCode
{
    public class BitmapConverter
    {
        private readonly ByteConverter _byteConverter;
        private const int HeaderSize = 54;

        public BitmapConverter(ByteConverter byteConverter)
        {
            _byteConverter = byteConverter;
        }


        /// <summary>
        /// Bitmapas paruošiamas konversijai (nuimamas headeris), tada kitoje klasėje apdorojami baitai
        /// </summary>
        /// <param name="bitmap">Bitmap'as bitais</param>
        /// <returns></returns>
        public (List<Vector> Vectors, int AddedBits, IEnumerable<byte> Header) ConvertBitmapToEncodableVectors(IEnumerable<byte> bitmap)
        {
            var bytes = bitmap.ToList();
            var bytesToConvert = bytes.Skip(HeaderSize);

            var (vectors, addedBits) = _byteConverter.ConvertBytesToEncodableVectors(bytesToConvert);

            return (vectors, addedBits, bytes.Take(HeaderSize));
        }

        /// <summary>
        /// Vektoriai verčiami į bitmapą - prie konvertuotų baitų pridedamas headeris.
        /// </summary>
        /// <param name="vectors">Vektoriai</param>
        /// <param name="addedBits">Pridėtų bitų skaičius</param>
        /// <param name="header">BMP headeris</param>
        /// <returns></returns>
        public IEnumerable<byte> ConvertVectorsToBitmap(List<Vector> vectors, int addedBits, IEnumerable<byte> header)
        {
            var imageBytes = _byteConverter.ConvertVectorsToBytes(vectors, addedBits);

            return header.Concat(imageBytes);
        }
    }
}