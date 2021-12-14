namespace OldCrypt_Library.Old.Substitution
{
    public class Vigenere : Cipher
    {
        #region Values

        protected string key = null;
        protected byte[] binKey = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Initiates a new instance of the Vigenere cipher class for encryption/decryption in classical mode.
        /// </summary>
        /// <param name="key">Key to be used for encryption/decryption. The key can contain only characters a - z and A - Z (will be converted to a - z in the actual key).</param>
        public Vigenere(string key)
        {
            Constructor(key, null);
        }

        /// <summary>
        /// Initiates a new instance of the Vigenere cipher class for encryption/decryption in binary mode.
        /// </summary>
        /// <param name="binKey">Key to be used for encryption/decryption.</param>
        public Vigenere(byte[] binKey)
        {
            Constructor(null, binKey);
        }

        /// <summary>
        /// Initiates a new instance of the Vigenere cipher class for encryption/decryption in classical and binary mode.
        /// </summary>
        /// <param name="key">Key to be used for encryption/decryption in classical mode.</param>
        /// <param name="binKey">Key to be used for encryption/decryption in binary mode.</param>
        public Vigenere(string key, byte[] binKey)
        {
            Constructor(key, binKey);
        }

        private void Constructor(string key, byte[] binKey)
        {
            if (IsKeyValid(key))
                this.key = key.ToLower();
            else
                this.key = null;

            if (binKey != null)
            {
                if (binKey.Length > 0)
                    this.binKey = binKey;
                else
                    this.binKey = null;
            }
            else
                this.binKey = null;
        }

        #endregion

        #region Getters and Setters

        /// <summary>
        /// Gets the key that's used for classical mode. The key will be null if it's not set or set to invalid value.
        /// </summary>
        public string Key
        {
            get { return key; }
        }

        /// <summary>
        /// Gets the key that's used for binary mode.
        /// </summary>
        public byte[] BinKey
        {
            get { return binKey; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the current <see cref="key"/> into a key, that can be used for encryption/decryption (converts the characters to 0-based numbers).
        /// </summary>
        /// <returns>Key that can be used for encryption/decryption in classical mode. Or null if <see cref="IsKeyValid()"/> returns false.</returns>
        private char[] CreateEncryptionKey()
        {
            if (IsKeyValid())
            {
                char[] tempKey = new char[key.Length];
                for (int i = 0; i < tempKey.Length; i++)
                {
                    tempKey[i] = (char)(key[i] - 97);
                }

                return tempKey;
            }
            else
                return null;
        }

        /// <summary>
        /// Checks whether the current <see cref="key"/> is valid for the Vigenere cipher.<br />
        /// The key is valid if it is not null, has length > 0 and contains only letters a - z or A - Z.
        /// </summary>
        /// <returns>True if key is valid. Otherwise false.</returns>
        private bool IsKeyValid()
        {
            return IsKeyValid(key);
        }

        /// <summary>
        /// Checks whether the key is valid for the Vigenere cipher.<br />
        /// The key is valid if it is not null, has length > 0 and contains only letters a - z or A - Z.
        /// </summary>
        /// <param name="key">Key to be checked.</param>
        /// <returns>True if key is valid. Otherwise false.</returns>
        private bool IsKeyValid(string key)
        {
            if (key != null)
            {
                if (key.Length > 0)
                {
                    foreach (char x in key.ToLower())
                    {
                        if (x > 96 && x < 123)
                            continue;
                        else
                            return false;
                    }

                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// <inheritdoc/><br />
        /// Throws <see cref="Exceptions.InvalidCipherParametersException"/> if the <see cref="Key"/> is empty (null or it's length is 0) or contains invalid characters (anything other that a - z and A - Z).
        /// </summary>
        /// <inheritdoc/>
        public override string Encrypt(string text)
        {
            text = ApplyIgnoreSpaceAndCase(text);

            char[] tempKey = CreateEncryptionKey();

            if (tempKey != null)
            {
                string result = "";
                int currentKeyChar = 0;
                bool wasUpper = false;
                int temp;
                int encryptedChars = 0;

                foreach (char x in text)
                {
                    if (x > 64 && x < 91)
                    {
                        wasUpper = true;
                        temp = x + 32;
                    }
                    else if (x > 96 && x < 123)
                    {
                        wasUpper = false;
                        temp = x;
                    }
                    else
                        temp = -1;

                    if (temp > 96 && temp < 123)
                    {
                        temp = Functions.Modulo((temp - 97) + tempKey[currentKeyChar], 26) + 97;

                        result += wasUpper ? (char)(temp - 32) : (char)temp;
                    }
                    else if (temp == -1)
                        HandleInvalidCharacter(result, x);

                    currentKeyChar++;
                    if (currentKeyChar == tempKey.Length)
                        currentKeyChar = 0;

                    encryptedChars++;
                    progress = (double)encryptedChars / text.Length;
                }

                return result;
            }
            else
                throw new Exceptions.InvalidCipherParametersException(nameof(Key) + " is not set or is invalid. The key for classical mode can only accept characters a - z and A - Z.");
        }

        /// <summary>
        /// <inheritdoc/><br />
        /// Throws <see cref="Exceptions.InvalidCipherParametersException"/> if the <see cref="Key"/> is empty (null or it's length is 0) or contains invalid characters (anything other that a - z and A - Z).
        /// </summary>
        /// <inheritdoc/>
        public override string Decrypt(string text)
        {
            char[] tempKey = CreateEncryptionKey();

            if (tempKey != null)
            {
                string result = "";
                int currentKeyChar = 0;
                bool wasUpper = false;
                int temp;

                foreach (char x in text)
                {
                    if (x > 64 && x < 91)
                    {
                        wasUpper = true;
                        temp = x + 32;
                    }
                    else if (x > 96 && x < 123)
                    {
                        wasUpper = false;
                        temp = x;
                    }
                    else
                        temp = -1;

                    if (temp > 96 && temp < 123)
                    {
                        temp = Functions.Modulo((temp - 97) - tempKey[currentKeyChar], 26) + 97;

                        result += wasUpper ? (char)(temp - 32) : (char)temp;
                    }
                    else if (temp == -1)
                        result += x;

                    currentKeyChar++;
                    if (currentKeyChar == tempKey.Length)
                        currentKeyChar = 0;

                    progress = (double)result.Length / text.Length;
                }

                return result;
            }
            else
                throw new Exceptions.InvalidCipherParametersException(nameof(Key) + " is not set or is invalid. The key for classical mode can only accept characters a - z and A - Z.");
        }

        /// <summary>
        /// <inheritdoc/><br />
        /// Throws an <see cref="Exceptions.InvalidCipherParametersException"/> if the <see cref="Key"/> is not set.
        /// </summary>
        /// <inheritdoc/>
        public override byte[] Encrypt(byte[] data)
        {
            if (binKey != null)
            {
                byte[] result = new byte[data.Length];

                for (int i = 0; i < data.Length; i++)
                {
                    result[i] = (byte)Functions.Modulo(data[i] + binKey[i], 256);
                }

                return result;
            }
            else
                throw new Exceptions.InvalidCipherParametersException(nameof(BinKey) + " is not set therefore binary mode is unavailable.");
        }

        /// <summary>
        /// <inheritdoc/><br />
        /// Throws an <see cref="Exceptions.InvalidCipherParametersException"/> if the <see cref="Key"/> is not set.
        /// </summary>
        /// <inheritdoc/>
        public override byte[] Decrypt(byte[] data)
        {
            if (binKey != null)
            {
                byte[] result = new byte[data.Length];

                for (int i = 0; i < data.Length; i++)
                {
                    result[i] = (byte)Functions.Modulo(data[i] - binKey[i], 256);
                }

                return result;
            }
            else
                throw new Exceptions.InvalidCipherParametersException(nameof(BinKey) + " is not set therefore binary mode is unavailable.");
        }

        #endregion
    }
}
