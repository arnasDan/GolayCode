namespace GolayCode
{
    /// <summary>
    /// Konstantos, reikalingos kodavimui ir atkodavimui
    /// </summary>
    public static class GolayCodeConstants
    {
        /// <summary>
        /// Vektoriaus dydis
        /// </summary>
        public const int VectorSize = 12;

        /// <summary>
        /// Matrica B iš Golėjaus kodo apibrėžimo
        /// </summary>
        public static readonly Matrix BMatrix = new Matrix
        {
            { 1, 1, 0, 1, 1, 1, 0, 0, 0, 1, 0, 1 },
            { 1, 0, 1, 1, 1, 0, 0, 0, 1, 0, 1, 1 },
            { 0, 1, 1, 1, 0, 0, 0, 1, 0, 1, 1, 1 },
            { 1, 1, 1, 0, 0, 0, 1, 0, 1, 1, 0, 1 },
            { 1, 1, 0, 0, 0, 1, 0, 1, 1, 0, 1, 1 },
            { 1, 0, 0, 0, 1, 0, 1, 1, 0, 1, 1, 1 },
            { 0, 0, 0, 1, 0, 1, 1, 0, 1, 1, 1, 1 },
            { 0, 0, 1, 0, 1, 1, 0, 1, 1, 1, 0, 1 },
            { 0, 1, 0, 1, 1, 0, 1, 1, 1, 0, 0, 1 },
            { 1, 0, 1, 1, 0, 1, 1, 1, 0, 0, 0, 1 },
            { 0, 1, 1, 0, 1, 1, 1, 0, 0, 0, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 }
        };

        /// <summary>
        /// 12x12 vienetinė matrica
        /// </summary>
        public static readonly Matrix IdentityMatrix = CreateIdentityMatrix();

        /// <summary>
        /// Sukuriama vienetinė matrica
        /// </summary>
        /// <returns>Vienetinė matrica</returns>
        private static Matrix CreateIdentityMatrix()
        {
            var identityMatrix = new Matrix();
            for (var i = 0; i < VectorSize; i++)
            {
                var row = new Vector(new Bit[VectorSize])
                {
                    [i] = 1
                };

                identityMatrix.Add(row);
            }

            return identityMatrix;
        }
    }
}