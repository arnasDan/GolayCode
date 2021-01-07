using System;
using System.Linq;

namespace GolayCode
{
    public class GolayDecoder
    {
        /// <summary>
        /// Kontrolinė matrica (H)
        /// </summary>
        private static readonly Matrix _parityCheckMatrix = CreateParityCheckMatrix();
        
        /// <summary>
        /// Atkoduojamas vektorius, užkoduotas Golėjaus kodu:
        /// Apskaičiuojamas nelyginio lyginumo bitas ir pridedamas prie įeities vektoriaus.
        /// Apskaičiuojamas sindromas, su juo bandoma gauti klaidos vektorių.
        /// Jei nepavyksta, sindromas padauginamas iš B matricos ir bandoma dar kartą.
        /// </summary>
        /// <param name="vector">Atkoduojamas vektorius. Ilgis privalo būti 23.</param>
        /// <returns>Pirmi 12 bitų iš vektoriaus, gauto pritaikius dekodavimo algoritmą</returns>
        public Vector DecodeVector(Vector vector)
        {
            if (vector.Count != 23)
                throw new ArgumentOutOfRangeException(nameof(vector), "Vector length must be 23");

            var inputVector = new Vector(vector);

            var oddParityBit = inputVector.ParityBit.Invert();
            inputVector.Add(oddParityBit);

            var syndrome = CalculateSyndrome(inputVector);

            if (!TryCalculateErrorVector(syndrome, true, out var errorVector))
            {
                syndrome *= GolayCodeConstants.BMatrix;

                TryCalculateErrorVector(syndrome, false, out errorVector);
            }

            var correctedInputVector = inputVector + errorVector;

            return new Vector(correctedInputVector.Take(GolayCodeConstants.VectorSize));
        }

        /// <summary>
        /// Bandoma rasti klaidos vektorių B.
        /// Jei sindromo svoris mažesnis nei arba lygus 3, grąžinamas klaidos vektorius [syndrome, 0]
        /// Toliau algoritmas vykdomas su kiekviena B matricos eilute i:
        /// Prie sindromo pridedamas vektorius i. Jei gauto vektoriaus svoris mažesnis nei arba lygus 2, grąžinamas klaidos vektorius [syndrome, eI] arba [eI, syndrome], priklausomai nuo parametro "placeSyndromeFirst" reikšmės.
        /// </summary>
        /// <param name="syndrome">Sindromas, paskaičiuotas pagal atkoduojamą vektorių</param>
        /// <param name="placeSyndromeFirst">Nurodo, ar klaidos vektoriuje sindromas turėtų būti pradžioje ar pabaigoje</param>
        /// <param name="errorVector">Klaidos vektorius, jei pavyko jį rasti</param>
        /// <returns>Ar pavyko rasti klaidos vektorių</returns>
        private static bool TryCalculateErrorVector(Vector syndrome, bool placeSyndromeFirst, out Vector errorVector)
        {
            errorVector = null;

            if (syndrome.Weight <= 3)
            {  
                errorVector = GetErrorVector(syndrome, placeSyndromeFirst);
                return true;
            }

            foreach (var (row, i) in GolayCodeConstants.BMatrix.Select((value, index) => (value, index)))
            {
                var summedVector = new Vector(syndrome) + row;

                if (summedVector.Weight > 2)
                    continue;

                errorVector = GetErrorVector(summedVector, placeSyndromeFirst, i);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Grąžinamas klaidos vektorius.
        /// Priklausomai nuo parametrų, gali būti grąžinami vektoriai:
        /// [syndrome, 0]; [syndrome, eI]; [eI, syndrome].
        /// </summary>
        /// <param name="syndrome">Sindromas, paskaičiuotas pagal atkoduojamą vektorių</param>
        /// <param name="placeSyndromeFirst">Nurodo, ar klaidos vektoriuje sindromas turėtų būti pradžioje ar pabaigoje</param>
        /// <param name="bMatrixRowIndex">Jei aktualu - B matricos eilutės indeksas, kur rastas tinkamas vektoriaus svoris</param>
        /// <returns>Klaidos vektorius</returns>
        private static Vector GetErrorVector(Vector syndrome, bool placeSyndromeFirst, int? bMatrixRowIndex = null)
        {
            var secondPart = new Vector(new Bit[GolayCodeConstants.VectorSize]);

            if (bMatrixRowIndex.HasValue)
                secondPart[bMatrixRowIndex.Value] = 1;

            if (placeSyndromeFirst)
            {
                var result = new Vector(syndrome);
                result.AddRange(secondPart);

                return result;
            }

            secondPart.AddRange(syndrome);
            return secondPart;
        }

        private static Vector CalculateSyndrome(Vector vector) => vector * _parityCheckMatrix;

        /// <summary>
        /// Sukuriama kontrolinė matrica (H):
        /// [I,
        ///  B]
        /// </summary>
        /// <returns>Kontrolinė matrica (H)</returns>
        private static Matrix CreateParityCheckMatrix()
        {
            var result = new Matrix
            {
                GolayCodeConstants.IdentityMatrix,
                GolayCodeConstants.BMatrix
            };

            return result;
        }
    }
}