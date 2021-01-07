using System;
using System.Linq;

namespace GolayCode
{
    public class GolayEncoder
    {
        /// <summary>
        /// Generuojanti matrica (G)
        /// </summary>
        private static readonly Matrix _generatorMatrix = CreateGeneratorMatrix();

        /// <summary>
        /// Golėjaus kodu užkoduojamas vektorius.
        /// Užkodavimas vykdomas vektorių padauginant iš generuojančios matricos.
        /// </summary>
        /// <param name="vector">Užkoduotinas vektorius. Ilgis privalo būti 12.</param>
        /// <returns></returns>
        public Vector EncodeVector(Vector vector)
        {
            if (vector.Count != 12)
                throw new ArgumentOutOfRangeException(nameof(vector), "Vector length must be 12");

            return vector * _generatorMatrix;
        }

        /// <summary>
        /// Sukuriama modifikuota matrica B užkodavimui, iš matricos B išmetant paskutinį stulpelį
        /// </summary>
        /// <returns>Modifikuota matrica B</returns>
        private static Matrix CreateAdjustedBMatrix()
        {
            var adjustedMatrix = new Matrix
            {
                GolayCodeConstants.BMatrix
            };

            foreach (var row in adjustedMatrix)
                row.RemoveAt(row.Count - 1);

            return adjustedMatrix;
        }

        /// <summary>
        /// Sukuriama generuojanti matrica G:
        /// [I, B]
        /// </summary>
        /// <returns>Generuojanti matrica (G)</returns>
        private static Matrix CreateGeneratorMatrix()
        {
            var bMatrix = CreateAdjustedBMatrix();

            var generatorMatrixRows = GolayCodeConstants.IdentityMatrix
                .Zip(bMatrix)
                .Select(pair => new Vector(pair.First.Concat(pair.Second)));

            return new Matrix
            {
                generatorMatrixRows
            };
        }
    }
}