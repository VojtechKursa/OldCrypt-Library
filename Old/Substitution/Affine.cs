namespace OldCrypt_Library.Old.Substitution
{
    public class Affine : Cipher
    {
        #region Values

        protected int a;
        protected int b;

        #endregion

        #region Constructors

        public Affine(int a, int b)
        {
            this.a = a;
            this.b = b;
        }

        #endregion

        #region Getters and Setters

        public int A
        {
            get { return a; }
        }

        public int B
        {
            get { return b; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// <inheritdoc/><br />
        /// Throws an <see cref="Exceptions.InvalidCipherParametersException"/> if <see cref="A"/> is invalid (lower than 1 or not coprime with size of the alphabet that's being encrypted (26 in this case)).
        /// </summary>
        /// <inheritdoc/>
        public override string Encrypt(string text)
        {
            text = ApplyIgnoreSpaceAndCase(text);

            if (a > 0)
            {
                if (Functions.GreatestCommonDivisor(a, 26) == 1)
                {
                    string result = "";
                    bool wasUpper;
                    int temp;

                    foreach (char x in text)
                    {
                        if (x > 64 && x < 91)
                        {
                            wasUpper = true;
                            temp = x + 32;
                        }
                        else
                        {
                            wasUpper = false;
                            temp = x;
                        }

                        if (x > 96 && x < 123)
                        {
                            temp = Functions.Modulo(a * (temp - 97) + b, 26) + 97;
                            result += wasUpper ? (char)(temp - 32) : (char)temp;
                        }
                        else
                            HandleInvalidCharacter(result, x);

                        progress = (double)result.Length / text.Length;
                    }

                    return result;
                }
                else
                    throw new Exceptions.InvalidCipherParametersException("With this cipher, the " + nameof(A) + " must be coprime with the size of the alphabet that's being encrypted (26 in case of the latin alphabet).");
            }
            else
                throw new Exceptions.InvalidCipherParametersException(nameof(A) + " must be higher than 0.");
        }

        /// <summary>
        /// <inheritdoc/><br />
        /// Throws an <see cref="Exceptions.InvalidCipherParametersException"/> if <see cref="A"/> is invalid (lower than 1 or not coprime with size of the alphabet that's being decrypted (26 in this case)).
        /// </summary>
        /// <inheritdoc/>
        public override string Decrypt(string text)
        {
            if (a > 0)
            {
                if (Functions.TryModInverse(a, 26, out int inverseA))
                {
                    string result = "";
                    bool wasUpper;
                    int temp;

                    foreach (char x in text)
                    {
                        if (x > 64 && x < 91)
                        {
                            wasUpper = true;
                            temp = x + 32;
                        }
                        else
                        {
                            wasUpper = false;
                            temp = x;
                        }

                        if (x > 96 && x < 123)
                        {
                            temp = Functions.Modulo(inverseA * ((temp - 97) - b), 26) + 97;

                            result += wasUpper ? (char)(temp - 32) : (char)temp;
                        }
                        else
                            result += x;

                        progress = (double)result.Length / text.Length;
                    }

                    return result;
                }
                else
                    throw new Exceptions.InvalidCipherParametersException("With this cipher, the " + nameof(A) + " must be coprime with the size of the alphabet that's being decrypted (26 in case of the latin alphabet).\nOtherwise there is no Modular Inverse and the encryption cannot be reversed.");
            }
            else
                throw new Exceptions.InvalidCipherParametersException(nameof(A) + " must be higher than 0.");
        }

        /// <summary>
        /// <inheritdoc/><br />
        /// Throws an <see cref="Exceptions.InvalidCipherParametersException"/> if <see cref="A"/> is invalid (lower than 1 or not coprime with size of the alphabet that's being encrypted (256 in this case)).
        /// </summary>
        /// <inheritdoc/>
        public override byte[] Encrypt(byte[] data)
        {
            if (a > 0)
            {
                if (Functions.GreatestCommonDivisor(a, 256) == 1)
                {
                    byte[] result = new byte[data.Length];

                    for (int i = 0; i < data.Length; i++)
                    {
                        result[i] = (byte)Functions.Modulo(a * data[i] + b, 256);
                    }

                    return result;
                }
                else
                    throw new Exceptions.InvalidCipherParametersException("With this cipher, the " + nameof(A) + " must be coprime with the size of the alphabet that's being encrypted (256 in case of binary mode).");
            }
            else
                throw new Exceptions.InvalidCipherParametersException(nameof(A) + " must be higher than 0.");
        }

        /// <summary>
        /// <inheritdoc/><br />
        /// Throws an <see cref="Exceptions.InvalidCipherParametersException"/> if <see cref="A"/> is invalid (lower than 1 or not coprime with size of the alphabet that's being decrypted (256 in this case)).
        /// </summary>
        /// <inheritdoc/>
        public override byte[] Decrypt(byte[] data)
        {
            if (a > 0)
            {
                if (Functions.TryModInverse(a, 26, out int inverseA))
                {
                    byte[] result = new byte[data.Length];

                    for (int i = 0; i < data.Length; i++)
                    {
                        result[i] = (byte)Functions.Modulo(inverseA * (data[i] - b), 256);
                    }

                    return result;
                }
                else
                    throw new Exceptions.InvalidCipherParametersException("With this cipher, the " + nameof(A) + " must be coprime with the size of the alphabet that's being decrypted (256 in case of binary mode).\nOtherwise there is no Modular Inverse and the encryption cannot be reversed.");
            }
            else
                throw new Exceptions.InvalidCipherParametersException(nameof(A) + " must be higher than 0.");
        }

        #endregion
    }
}
