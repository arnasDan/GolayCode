using System.Collections.Generic;
using System.Linq;

namespace GolayCode
{
    /// <summary>
    /// Dvejetainė matrica
    /// </summary>
    public class Matrix : List<Vector>
    {
        /// <summary>
        /// Matricos eilučių skaičius
        /// </summary>
        public int Rows => Count;

        /// <summary>
        /// Matricos stulpelių skaičius
        /// </summary>
        public int Columns => this.FirstOrDefault()?.Count ?? 0;

        /// <summary>
        /// Į matricą Įdedamos eilutės (vektoriai)
        /// </summary>
        /// <param name="rows">Vektoriai, pridedami į matricą</param>
        public void Add(IEnumerable<Vector> rows)
        {
            foreach (var row in rows)
            {
                Add(row.ToArray());
            }
        }

        /// <summary>
        /// Į matricą įdedama nauja eilutė su nurodytais bitais
        /// </summary>
        /// <param name="integers">Bitai, kurie turi būti pridėti į matricą</param>
        public void Add(params Bit[] integers)
        {

            Add(new Vector(integers));
        }

        /// <summary>
        /// Matrica konvertuojama į tekstą
        /// </summary>
        /// <returns>Matricos tekstinė reprezentacija</returns>
        public override string ToString()
        {
            return string.Join("\n", this);
        }
    }
}