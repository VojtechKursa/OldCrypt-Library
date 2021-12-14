using System;

namespace OldCrypt_Library
{
    /// <summary>
    /// A class that contains various useful functions for simplification of other places in the program.
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// Converts the given array of <see cref="byte"/>s to a <see cref="string"/> of hexadecimal numbers.
        /// </summary>
        /// <param name="data">Array of <see cref="byte"/>s to be converted.</param>
        /// <returns><see cref="string"/> containing the converted array.</returns>
        public static string ToHex(byte[] data)
        {
            string result = "";

            foreach (byte x in data)
            {
                result += Convert.ToString(x, 16).ToUpper().PadLeft(2, '0');
            }

            return result;
        }

        /// <summary>
        /// Converts the given <see cref="string"/> of hexadecimal numbers to an array of <see cref="byte"/>s. <br />
        /// If the supplied string is of odd <see cref="string.Length"/>, it will be padded by a 0 from the left. <br />
        /// If an unsupported value is encountered during the conversion, an <see cref="ArgumentException"/> will be thrown.
        /// </summary>
        /// <param name="hex">The <see cref="string"/> of hexadecimal numbers to be converted.</param>
        /// <returns>The array of <see cref="byte"/>s containing the converted string.</returns>
        /// <exception cref="ArgumentException"/>
        public static byte[] ToByte(string hex)
        {
            if (hex.Length % 2 != 0)
                hex = "0" + hex;

            byte[] data = new byte[hex.Length / 2];

            try
            {
                for (int i = 0; i < hex.Length; i += 2)
                {
                    data[i / 2] = Convert.ToByte(hex[i].ToString() + hex[i + 1].ToString(), 16);
                }

            }
            catch
            {
                throw new ArgumentException("An invalid value was encountered during the hexadecimal -> byte conversion.");
            }

            return data;
        }

        /// <summary>
        /// Returns a modulo of a number as a value between 0 and modulo - 1.<br />
        /// (Performs a modulo operation and when the result is less than 0, adds modulo to it.)
        /// </summary>
        /// <param name="number">Number whose modulo to calculate.</param>
        /// <param name="modulo">The modulo.</param>
        /// <returns>Cryptographical modulo of the given number.</returns>
        public static int Modulo(int number, int modulo)
        {
            int result = number % modulo;

            if (result < 0)
                result += modulo;

            return result;
        }

        /// <summary>
        /// Calculates the greatest common divisor of two numbers.<br />
        /// If either of the numbers is less than 1, an <see cref="ArgumentOutOfRangeException"/> will be thrown.
        /// </summary>
        /// <param name="a">Number 1.</param>
        /// <param name="b">Number 2.</param>
        /// <returns>The greatest common divisor of A and B.</returns>
        /// <exception cref="ArgumentOutOfRangeException" />
        /// <!-- Source: https://stackoverflow.com/questions/18541832/c-sharp-find-the-greatest-common-divisor -->
        public static int GreatestCommonDivisor(int a, int b)
        {
            if (a < 1 || b < 1)
                throw new ArgumentOutOfRangeException();

            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a | b;
        }

        /// <summary>
        /// Calculates the Modular Inverse of a given number in context of a given modulo.<br />
        /// If number is less than 1 or modulo is less than 2, an <see cref="ArgumentOutOfRangeException"/> will be thrown.
        /// </summary>
        /// <param name="number">Number whose Modular Inverse is to be calculated.</param>
        /// <param name="modulo">Modulo to be used in the calculation.</param>
        /// <param name="result">Variable to which the result of the calculation will be passed.</param>
        /// <returns>True if the calculation was successful, false if the number doesn't have a Modular Inverse in the given modulo context.</returns>
        /// <exception cref="ArgumentOutOfRangeException" />
        /// <!-- Source: https://stackoverflow.com/questions/7483706/c-sharp-modinverse-function -->
        public static bool TryModInverse(int number, int modulo, out int result)
        {
            if (number < 1) throw new ArgumentOutOfRangeException(nameof(number));
            if (modulo < 2) throw new ArgumentOutOfRangeException(nameof(modulo));

            int n = number;
            int m = modulo, v = 0, d = 1;
            while (n > 0)
            {
                int t = m / n, x = n;
                n = m % x;
                m = x;
                x = d;
                d = checked(v - t * x); // Just in case
                v = x;
            }
            result = v % modulo;
            if (result < 0) result += modulo;
            if ((long)number * result % modulo == 1L) return true;
            result = default;
            return false;
        }
    }
}
