using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GolayCode
{
    public class ByteConverter
    {
        /// <summary>
        /// Baitai paverčiami į bitus ir padalijami į reikiamo dydžio dvejetainius vektorius. Jei reikia, gale pridedami nuliai.
        /// </summary>
        /// <param name="bytes">Baitai</param>
        /// <returns>Vektoriai, pridėtų bitų skaičius</returns>
        public (List<Vector> Vectors, int AddedBits) ConvertBytesToEncodableVectors(IEnumerable<byte> bytes)
        {
            var charBytes = bytes.Select(n => Convert.ToString(n, 2).PadLeft(8, '0'));

            var bits = string.Join("", charBytes).ToArray();

            var vectors = new List<Vector>();
            var addedBits = 0;
            foreach (var batch in bits.SplitIntoBatches(GolayCodeConstants.VectorSize))
            {
                var batchBits = batch.ToList();

                if (batchBits.Count < GolayCodeConstants.VectorSize)
                {
                    addedBits = GolayCodeConstants.VectorSize - batchBits.Count;
                    batchBits.AddRange(Enumerable.Repeat('0', addedBits));
                }

                var bitString = new string(batchBits.ToArray());

                vectors.Add(new Vector(bitString.ParseAsVector()));
            }

            return (vectors, addedBits);
        }

        /// <summary>
        /// Vektoriai verčiami į baitus.
        /// Vektoriai sujungiami ir padalijami į baito dydžio dalis.
        /// </summary>
        /// <param name="vectors"></param>
        /// <param name="addedBits"></param>
        /// <returns></returns>
        public IEnumerable<byte> ConvertVectorsToBytes(List<Vector> vectors, int addedBits)
        {
            var bits = string.Join("", vectors
                .SelectMany(v => v.Select(b => b.ToString()))
                .ToList());

            return bits
                .Take(bits.Length - addedBits)
                .ToArray()
                .SplitIntoBatches(8)
                .Select(chars => Convert.ToByte(new string(chars.ToArray()), 2));
        }
    }
}