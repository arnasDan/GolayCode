using System.Collections.Generic;
using System.Linq;

namespace GolayCode
{
    /// <summary>
    /// Dvejetainis vektorius
    /// </summary>
    public class Vector : List<Bit>
    {
        /// <summary>
        /// Inicializuojamas tuščias vektorius
        /// </summary>
        public Vector()
        {

        }

        /// <summary>
        /// Inicializuojamas vektorius su bitais
        /// </summary>
        /// <param name="values">Vektoriaus bitai</param>
        public Vector(IEnumerable<Bit> values) : base(values)
        {

        }

        /// <summary>
        /// Vektoriaus svoris - bitų suma
        /// </summary>
        public int Weight => this.Sum(b => b.Value);


        /// <summary>
        /// Vektoriaus lyginumo bitas
        /// </summary>
        public Bit ParityBit => new Bit(Weight);

        /// <summary>
        /// Vektorius dauginamas iš matricos
        /// </summary>
        /// <param name="vector">Vektorius, kuris dauginamas</param>
        /// <param name="matrix">Matrica, iš kurios dauginama</param>
        /// <returns>Vektorius, gautas atlikus daugybą</returns>
        public static Vector operator *(Vector vector, Matrix matrix)
        {
            var resultingVector = new Vector();

            for (var i = 0; i < matrix.Columns; i++)
            {
                Bit vectorBit = 0;
                for (var j = 0; j < matrix.Rows; j++)
                    vectorBit += matrix[j][i] * vector[j];

                resultingVector.Add(vectorBit);
            }
            return resultingVector;
        }

        /// <summary>
        /// Apskaičiuojama dviejų vektorių suma, sudedant kiekvieną jų elementą su atitinkamu.
        /// </summary>
        /// <param name="left">Vektorius, prie kurio pridedama</param>
        /// <param name="right">Vektorius, kuris pridedamas</param>
        /// <returns>Suma</returns>
        public static Vector operator +(Vector left, Vector right)
        {
            var bits = left.Zip(right).Select(pair => (Bit) (pair.First.Value + pair.Second.Value));

            return new Vector(bits);
        }

        /// <summary>
        /// Apskaičiuojamas dviejų vektorių XOR, lyginant
        /// </summary>
        /// <param name="left">Kairysis vektorius</param>
        /// <param name="right">Dešinysis vektorius</param>
        /// <returns>XOR rezultatas</returns>
        public static Vector operator ^(Vector left, Vector right)
        {
            var bits = left.Zip(right).Select(pair => (Bit) (pair.First.Value ^ pair.Second.Value));

            return new Vector(bits);
        }

        /// <summary>
        /// Vektorius konvertuojamas į tekstą
        /// </summary>
        /// <returns>Vektoriaus tekstinė reprezentacija</returns>
        public override string ToString()
        {
            return string.Join(" ", this);
        }
    }
}