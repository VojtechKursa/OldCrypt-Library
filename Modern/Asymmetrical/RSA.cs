using System;
using System.IO;
using System.Security.Cryptography;

namespace OldCrypt_Library.Modern.Asymmetrical
{
    public class RSA : AsymmetricalCipher
    {
        protected readonly System.Security.Cryptography.RSA rsa;
        public RSAEncryptionPadding DefaultPaddingMode { get; set; } = RSAEncryptionPadding.OaepSHA256;

        #region Constructors

        public RSA()
        {
            rsa = System.Security.Cryptography.RSA.Create();
            algorithm = rsa;
        }

        public RSA(int keySizeInBits)
        {
            rsa = System.Security.Cryptography.RSA.Create(keySizeInBits);
            algorithm = rsa;
        }

        public RSA(RSAParameters parameters)
        {
            rsa = System.Security.Cryptography.RSA.Create(parameters);
            algorithm = rsa;
        }

        #endregion

        #region Getters and Setters



        #endregion

        #region Methods

        #region Encryption and Decryption

        /// <summary>
        /// <inheritdoc/><br />
        /// Uses the <see cref="DefaultPaddingMode"/> as padding.
        /// </summary>
        /// <inheritdoc/>
        public override byte[] Encrypt(byte[] data)
        {
            return Encrypt(data, DefaultPaddingMode);
        }

        /// <summary>
        /// <inheritdoc/><br />
        /// Uses the <see cref="DefaultPaddingMode"/> as padding.<br />
        /// Throws an <see cref="Exceptions.InvalidCipherParametersException"/> if the private key isn't specified.
        /// </summary>
        /// <inheritdoc/>
        public override byte[] Decrypt(byte[] data)
        {
            return Decrypt(data, DefaultPaddingMode);
        }

        /// <param name="padding"><see cref="RSAEncryptionPadding"/> to use as a padding.</param>
        /// <inheritdoc cref="Cipher.Encrypt(byte[])"/>
        public virtual byte[] Encrypt(byte[] data, RSAEncryptionPadding padding)
        {
            //Preparation
            int blockSize = MaxEncryptableDataSize(padding);
            if (blockSize < 1)
                throw new Exceptions.InvalidCipherParametersException(String.Format("Encryption not possible using current {0} and selected padding mode.", nameof(KeySize)));

            int lastBlockStartIndex = data.Length - (data.Length % blockSize);
            int numberOfBlocks = lastBlockStartIndex / blockSize;   //Number of blocks without the last one

            byte[] resultBuffer = new byte[KeySizeByte * (numberOfBlocks + 1)];
            byte[] preBuffer;
            byte[] postBuffer;


            //All blocks except last one
            int currentBlockNumber = 0;
            for (int i = 0; i < lastBlockStartIndex; i += blockSize)
            {
                preBuffer = new byte[blockSize];
                for (int x = 0; x < blockSize; x++)
                {
                    preBuffer[x] = data[i + x];
                }

                postBuffer = rsa.Encrypt(preBuffer, padding);
                postBuffer.CopyTo(resultBuffer, currentBlockNumber * KeySizeByte);

                currentBlockNumber++;
            }

            //Last block
            preBuffer = new byte[data.Length % blockSize];
            for (int i = lastBlockStartIndex; i < data.Length; i++)
            {
                preBuffer[i - lastBlockStartIndex] = data[i];
            }

            postBuffer = rsa.Encrypt(preBuffer, padding);
            postBuffer.CopyTo(resultBuffer, currentBlockNumber * KeySizeByte);

            return resultBuffer;
        }

        /// <summary>
        /// <inheritdoc cref="Cipher.Decrypt(byte[])"/><br />
        /// Throws an <see cref="Exceptions.InvalidCipherParametersException"/> if the private key isn't specified.
        /// </summary>
        /// <param name="padding"><see cref="RSAEncryptionPadding"/> to use as a padding.</param>
        /// <inheritdoc cref="Cipher.Decrypt(byte[])"/>
        public virtual byte[] Decrypt(byte[] data, RSAEncryptionPadding padding)
        {
            if (data.Length % KeySizeByte == 0)
            {
                try
                {
                    //Preparation
                    int blockSize = KeySizeByte;

                    int lastBlockStartIndex = data.Length - blockSize;
                    int numberOfBlocks = data.Length / KeySizeByte - 1; //Number of blocks except the last one
                    int dataBlockSize = MaxEncryptableDataSize(padding);

                    byte[] resultBuffer = new byte[numberOfBlocks * dataBlockSize];
                    byte[] preBuffer;
                    byte[] postBuffer;


                    //All blocks except last one
                    int currentBlockNumber = 0;
                    for (int i = 0; i < lastBlockStartIndex; i += blockSize)
                    {
                        preBuffer = new byte[blockSize];
                        for (int x = 0; x < blockSize; x++)
                        {
                            preBuffer[x] = data[i + x];
                        }

                        postBuffer = rsa.Decrypt(preBuffer, padding);
                        postBuffer.CopyTo(resultBuffer, dataBlockSize * currentBlockNumber);

                        currentBlockNumber++;
                    }

                    //Last block
                    preBuffer = new byte[blockSize];
                    for (int i = lastBlockStartIndex; i < data.Length; i++)
                    {
                        preBuffer[i - lastBlockStartIndex] = data[i];
                    }

                    postBuffer = rsa.Decrypt(preBuffer, padding);


                    //Final completion
                    byte[] result = new byte[resultBuffer.Length + postBuffer.Length];
                    resultBuffer.CopyTo(result, 0);
                    postBuffer.CopyTo(result, resultBuffer.Length);

                    return result;
                }
                catch
                {
                    throw new Exceptions.InvalidCipherParametersException("Decryption encountered an error, the RSA instance propably doesn't have a private key specified.");
                }
            }
            else
                throw new Exceptions.InvalidInputException(String.Format("Invalid data, the input data must have length that's a multiple of {0} during decryption.", nameof(KeySizeByte)));
        }

        protected override bool FileHandler(BinaryReader input, BinaryWriter output, bool encrypt)
        {
            return FileHandler(input, output, encrypt, DefaultPaddingMode);
        }

        protected bool FileHandler(BinaryReader input, BinaryWriter output, bool encrypt, RSAEncryptionPadding padding)
        {
            long dataLength = input.BaseStream.Length;

            if (!encrypt)
            {
                if (dataLength % KeySizeByte != 0)
                    throw new Exceptions.InvalidInputException(String.Format("Invalid data, the input data must have length that's a multiple of {0} during decryption.", nameof(KeySizeByte)));

                //TO DO: Add checking for private key
            }

            //Preparation
            int step = encrypt ? MaxEncryptableDataSize(padding) : KeySizeByte;
            if (step < 1)
                throw new Exceptions.InvalidCipherParametersException(String.Format("Encryption not possible using current {0} and selected padding mode.", nameof(KeySize)));

            long lastBlockStartIndex = encrypt ? dataLength - (dataLength % step) : dataLength - step;

            byte[] preBuffer;
            byte[] postBuffer;


            //All blocks except last one
            for (int i = 0; i < lastBlockStartIndex; i += step)
            {
                preBuffer = input.ReadBytes(step);

                postBuffer = encrypt ? rsa.Encrypt(preBuffer, padding) : rsa.Decrypt(preBuffer, padding);

                output.Write(postBuffer);
            }

            //Last block
            preBuffer = input.ReadBytes((int)(dataLength - lastBlockStartIndex));

            postBuffer = encrypt ? rsa.Encrypt(preBuffer, padding) : rsa.Decrypt(preBuffer, padding);

            output.Write(postBuffer);

            return true;
        }

        #endregion

        #region Parameters

        /// <summary>
        /// Returns the parameters used by the current instance of <see cref="RSA"/> cipher.<br />
        /// Equivalent to <see cref="System.Security.Cryptography.RSA.ExportParameters(bool)"/>.
        /// </summary>
        /// <param name="includePrivateParameters">Whether to include private parameters or not.</param>
        /// <returns>The parameters used by the current instance of <see cref="RSA"/> cipher.</returns>
        public RSAParameters GetParameters(bool includePrivateParameters)
        {
            return rsa.ExportParameters(includePrivateParameters);
        }

        /// <summary>
        /// Sets the parameters to be used by the current instance of <see cref="RSA"/> cipher.<br />
        /// When only public parameters are supplied, only encryption will be possible.<br />
        /// Equivalent to <see cref="System.Security.Cryptography.RSA.ImportParameters(RSAParameters)"/>.
        /// </summary>
        /// <param name="parameters">Parameters to be used.</param>
        public void SetParameters(RSAParameters parameters)
        {
            rsa.ImportParameters(parameters);
        }

        /// <summary>
        /// Returns the parameters used by the current instance of <see cref="RSA"/> cipher in a format of XML string.<br />
        /// Equivalent to <see cref="System.Security.Cryptography.RSA.ToXmlString(bool)"/>.
        /// </summary>
        /// <param name="includePrivateParameters">Whether to include private parameters or not.</param>
        /// <returns>The parameters used by the current instance of <see cref="RSA"/> cipher, in XML string format.</returns>
        public string GetParametersXML(bool includePrivateParameters)
        {
            return rsa.ToXmlString(includePrivateParameters);
        }

        /// <summary>
        /// Sets the parameters to be used by the current instance of <see cref="RSA"/> cipher.<br />
        /// When only public parameters are supplied, only encryption will be possible.<br />
        /// Equivalent to <see cref="System.Security.Cryptography.RSA.FromXmlString(string)"/>.
        /// </summary>
        /// <param name="parameters">Parameters to be used, in XML string format.</param>
        public void SetParametersXML(string parameters)
        {
            rsa.FromXmlString(parameters);
        }

        #endregion

        #region Support methods

        /// <summary>
        /// Returns the maximum amount of bytes that can be encrypted at once by RSA, depending on the selected <see cref="RSAEncryptionPadding"/> scheme and size of Key.
        /// </summary>
        /// <param name="padding">The desired <see cref="RSAEncryptionPadding"/> scheme.</param>
        /// <returns>
        /// The maximum amount of bytes that RSA can encrypt at once.<br />
        /// This value can be negative if encryption is not possible using the given padding scheme and key size.<br />
        /// -1 if the <i>padding</i> is not supported by this method.
        /// </returns>
        public virtual int MaxEncryptableDataSize(RSAEncryptionPadding padding)
        {
            return KeySizeByte - GetOverhead(padding);
        }

        /// <summary>
        /// Calculates the minimum overhead required by the selected <see cref="RSAEncryptionPadding"/> scheme, in bytes.
        /// </summary>
        /// <param name="padding">The selected <see cref="RSAEncryptionPadding"/> scheme.</param>
        /// <returns>The minimum overhead required by the selected <see cref="RSAEncryptionPadding"/> scheme. (in bytes)</returns>
        public virtual int GetOverhead(RSAEncryptionPadding padding)
        {
            if (padding == RSAEncryptionPadding.Pkcs1)
                return 11;
            else if (
                padding == RSAEncryptionPadding.OaepSHA1 ||
                padding == RSAEncryptionPadding.OaepSHA256 ||
                padding == RSAEncryptionPadding.OaepSHA384 ||
                padding == RSAEncryptionPadding.OaepSHA512)
            {
                int hashLength = 0;

                if (padding == RSAEncryptionPadding.OaepSHA1)
                    hashLength = 20;
                else if (padding == RSAEncryptionPadding.OaepSHA256)
                    hashLength = 32;
                else if (padding == RSAEncryptionPadding.OaepSHA384)
                    hashLength = 48;
                else if (padding == RSAEncryptionPadding.OaepSHA512)
                    hashLength = 64;

                return 2 * hashLength + 2;
            }
            else
                throw new Exception("The selected padding sheme is not supported by this method.");
        }

        #endregion

        #endregion
    }
}
