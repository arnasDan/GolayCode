using System;
using System.Collections.Generic;

namespace GolayCode
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Masyvas padalijamas į dalis
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Masyvas</param>
        /// <param name="batchSize">Dalies dydis</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> SplitIntoBatches<T>(this T[] array, int batchSize)
        {
            for (var i = 0; i < array.Length; i += batchSize)
            {
                var upperBound = Math.Min(array.Length, i + batchSize);
                yield return array[i..upperBound];
            }
        }
    }
}