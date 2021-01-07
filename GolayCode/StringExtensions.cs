using System;
using System.Collections.Generic;
using System.Linq;

namespace GolayCode
{
    public static class StringExtensions
    {
        /// <summary>
        /// Tekstas yra konvertuojamas į dvejetainį vektorių.
        /// Tekste gali būti tik tarpai bei 0 ir 1 simboliai.
        /// </summary>
        /// <param name="str">Konvertuojamas tekstas</param>
        /// <returns>Dvejetainis vektorius</returns>
        public static Vector ParseAsVector(this string str)
        {
            str = str.Replace(" ", "");

            if (str.Any(character => character != '0' && character != '1'))
                throw new ArgumentException("String may only contain zeroes and ones", nameof(str));

            var bits = str.Select(character => (Bit) char.GetNumericValue(character));

            return new Vector(bits);
        }
    }
}