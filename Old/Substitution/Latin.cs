using System;

namespace OldCrypt_Library.Old.Substitution
{
    public class Latin : Cipher
    {
        #region Methods

        /// <summary>
        /// <inheritdoc/><br />
        /// Throws an <see cref="Exceptions.InvalidInputException"/> if the text is invalid (contains characters other than a - z, A - Z, whitespace).
        /// </summary>
        /// <inheritdoc/>
        public override string Encrypt(string text)
        {
            text = ApplyIgnoreSpaceAndCase(text);

            text = text.ToUpper();
            string result = "";

            foreach (char x in text)
            {
                if (x == ' ')
                    result += " ";
                else if (x > 64 && x < 91)
                    result += (x - 64).ToString() + " ";
                else
                    throw new Exceptions.InvalidInputException("The Latin code only supports latin letters (a - z and A - Z) and whitespace (' ').");

                progress = (double)result.Length / text.Length;
            }

            return result.Remove(result.Length - 1);
        }

        /// <summary>
        /// <inheritdoc/><br />
        /// Throws an <see cref="Exceptions.InvalidInputException"/> if the text is invalid (contains characters other than numbers from 1 to 26).
        /// </summary>
        /// <inheritdoc/>
        public override string Decrypt(string text)
        {
            string result = "";
            string[] textSplit = text.Split(' ');
            int value;

            try
            {
                foreach (string x in textSplit)
                {
                    if (x == "")
                        result += " ";
                    else
                    {
                        value = Convert.ToInt32(x);

                        if (value > 0 && value < 27)
                            result += Convert.ToChar(value + 64);
                        else
                            throw new Exceptions.InvalidInputException("The Latin code only supports latin letters (a - z and A - Z) and whitespace (' ').");
                    }

                    progress = (double)result.Length / text.Length;
                }
            }
            catch
            {
                return "Error! Invalid input!";
            }

            return result;
        }

        ///<summary>
        ///Throws a <see cref="Exceptions.CipherUnavailableException" />.
        ///</summary>
        public override byte[] Encrypt(byte[] data)
        {
            throw new Exceptions.CipherUnavailableException("Binary mode unavailable for the Latin code.");
        }

        ///<summary>
        ///Throws a <see cref="Exceptions.CipherUnavailableException" />.
        ///</summary>
        public override byte[] Decrypt(byte[] data)
        {
            throw new Exceptions.CipherUnavailableException("Binary mode unavailable for the Latin code.");
        }

        #endregion
    }
}
