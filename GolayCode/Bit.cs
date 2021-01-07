
namespace GolayCode
{
    /// <summary>
    /// Bitas
    /// </summary>
    public struct Bit
    {
        /// <summary>
        /// Bito skaitinė reikšmė - visada 0 arba 1.
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// Iš sveiko skaičiaus sukuriamas bitas naudojantis operacija mod 2.
        /// </summary>
        /// <param name="value">Sveikas skaičius</param>
        public Bit(int value)
        {
            Value = value % 2;
        }

        /// <summary>
        /// Invertuojamas bitą, t.y 0->1; 1->0
        /// </summary>
        /// <returns>Invertuotas bitas</returns>
        public Bit Invert()
        {
            return new Bit(Value + 1);
        }
        
        /// <summary>
        /// Gaunama bito skaitinė reikšmė
        /// </summary>
        /// <param name="binaryInteger">Bitas</param>
        public static implicit operator int (Bit bit)
        {
            return bit.Value;
        }

        /// <summary>
        /// Sveikas skaičius (int) paverčiamas į bitą
        /// </summary>
        /// <param name="integer">Skaičius, iš kurio gaunamas bitas</param>
        public static implicit operator Bit (int integer)
        {
            return new Bit(integer);
        }

        /// <summary>
        /// Sudedami du bitai. Rezultatas - jų suma mod 2.
        /// </summary>
        /// <param name="left">Bitas, prie kurio pridedama</param>
        /// <param name="right">Bitas, kuris pridedamas</param>
        /// <returns></returns>
        public static Bit operator + (Bit left, Bit right)
        {
            return new Bit(left.Value + right.Value);
        }

        /// <summary>
        /// Bitas konvertuojamas į tekstą
        /// </summary>
        /// <returns>Bito tekstinė reprezentacija</returns>
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}