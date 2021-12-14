using System.Security.Cryptography;
using System.Text;

namespace OldCrypt_Library.Modern.Asymmetrical
{
    /// <summary>
    /// Represents the base class for modern Asymmetrical Ciphers.
    /// </summary>
    public abstract class AsymmetricalCipher : Cipher
    {
        #region Values

        protected AsymmetricAlgorithm algorithm;

        #endregion

        #region Getters and Setters

        /// <summary>
        /// Gets or sets the size of the key that's being used by the current algorithm, in bits.<br />
        /// Throws an <see cref="Exceptions.InvalidKeySizeException"/> if an attempt to set KeySize to an unsupported value is made.
        /// </summary>
        /// <returns>The size of the key that's being used by the current algorithm. (in bits)</returns>
        /// <exception cref="Exceptions.InvalidKeySizeException"/>
        public int KeySize
        {
            get => algorithm.KeySize;
            set
            {
                try
                {
                    algorithm.KeySize = value;
                }
                catch
                {
                    throw new Exceptions.InvalidKeySizeException("Invalid key size.");
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of the key that's being used by the current algorithm, in bytes.<br />
        /// Throws an <see cref="Exceptions.InvalidKeySizeException"/> if an attempt to set KeySize to an unsupported value is made.
        /// </summary>
        /// <returns>The size of the key that's being used by the current algorithm. (in bytes)</returns>
        /// <exception cref="Exceptions.InvalidKeySizeException"/>
        public int KeySizeByte
        {
            get => algorithm.KeySize / 8;
            set => KeySize = value * 8;
        }

        /// <summary>
        /// Gets the supported key sizes of the current algorithm.
        /// </summary>
        /// <returns>The supported key sizes of the current algorithm.</returns>
        public KeySizes[] LegalKeySizes
        {
            get => algorithm.LegalKeySizes;
        }

        #endregion

        #region Methods

        #region Encryption and Decryption

        /// <summary>
        /// <inheritdoc/><br />
        /// The string of characters is encoded into bytes using the <see cref="Encoding.UTF8"/>, encrypted and converted a to hexadecimal string.<br />
        /// All the required parameters must be set before calling this method, otherwise throws an <see cref="Exceptions.InvalidCipherParametersException"/>.
        /// </summary>
        /// <returns><inheritdoc/> (In hexadecimal).</returns>
        /// <inheritdoc/>
        public override string Encrypt(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            data = Encrypt(data);
            return Functions.ToHex(data);
        }

        /// <summary>
        /// <inheritdoc/><br />
        /// The string of characters is converted to bytes from the hexadecimal representation, decrypted and decoded back into string using the <see cref="Encoding.UTF8"/>.<br />
        /// All the required parameters must be set before calling this method, otherwise throws an <see cref="Exceptions.InvalidCipherParametersException"/>.
        /// </summary>
        /// <param name="text">The ciphertext in hexadecimal.</param>
        /// <inheritdoc/>
        public override string Decrypt(string text)
        {
            byte[] data = Functions.ToByte(text);
            data = Decrypt(data);
            return Encoding.UTF8.GetString(data);
        }

        #endregion

        #endregion
    }
}
