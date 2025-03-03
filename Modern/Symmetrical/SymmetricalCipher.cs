using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OldCrypt.Library.Modern.Symmetrical
{
	/// <summary>
	/// Represents the base class for Symmetrical modern ciphers.
	/// </summary>
	public abstract class SymmetricalCipher : Cipher
	{
		#region Values

		/// <summary>
		/// Gets the reference to the current instance of the algorithm used.
		/// </summary>
		public SymmetricAlgorithm Algorithm { get; protected set; }

		/// <summary>
		/// Gets the current encryptor used by the current instance of <see cref="SymmetricalCipher"/>.
		/// </summary>
		public ICryptoTransform Encryptor { get; protected set; }

		/// <summary>
		/// Gets the current decryptor used by the current instance of <see cref="SymmetricalCipher"/>.
		/// </summary>
		public ICryptoTransform Decryptor { get; protected set; }

		#endregion

		#region Getters and Setters


		#region Key

		/// <summary>
		/// Gets or sets the Key used by the current algorithm.<br />
		/// <u>Setting:</u><br />
		/// If the key given has different length than the current <see cref="KeySize"/>, the <see cref="KeySize"/> will be changed to match the length of the key,<br />
		/// which can result in <see cref="Exceptions.InvalidKeySizeException"/> when the given key is of unsupported length.<br />
		/// If null is given an <see cref="ArgumentNullException"/> is thrown.
		/// </summary>
		/// <returns><inheritdoc cref="SymmetricAlgorithm.Key"/></returns>
		/// <exception cref="Exceptions.InvalidKeySizeException" />
		/// <exception cref="ArgumentNullException"/>
		public byte[] Key
		{
			get { return Algorithm.Key; }
			set
			{
				if (value != null)
				{
					if (value.Length != KeySizeByte)
						KeySizeByte = value.Length;
				}

				Algorithm.Key = value;
			}
		}


		/// <summary>
		/// Gets or sets the size of the key used by the current algorithm (in bits).<br />
		/// During setting, an <see cref="Exceptions.InvalidKeySizeException"/> can be thrown if a key size that's not supported by the current algorithm is given.
		/// </summary>
		/// <returns><inheritdoc cref="SymmetricAlgorithm.KeySize"/></returns>
		/// <exception cref="Exceptions.InvalidKeySizeException" />
		public int KeySize
		{
			get { return Algorithm.KeySize; }
			set
			{
				Algorithm.KeySize = Algorithm.ValidKeySize(value) ? value : throw new Exceptions.InvalidKeySizeException();
			}
		}

		/// <summary>
		/// Gets or sets the size of the key used by the current algorithm (in bytes).<br />
		/// During setting, an <see cref="Exceptions.InvalidKeySizeException"/> can be thrown if a key size that's not supported by the current algorithm is given.
		/// </summary>
		/// <returns>The size, in <b>bytes</b>, of the secret key used by the current algorithm.</returns>
		/// <exception cref="Exceptions.InvalidKeySizeException" />
		public int KeySizeByte
		{
			get { return Algorithm.KeySize / 8; }
			set
			{
				Algorithm.KeySize = Algorithm.ValidKeySize(value * 8) ? value * 8 : throw new Exceptions.InvalidKeySizeException();
			}
		}

		/// <summary>
		/// Gets the valid key sizes for the current algorithm (in bits).
		/// </summary>
		/// <inheritdoc cref="SymmetricAlgorithm.LegalKeySizes"/>
		public KeySizes[] ValidKeySizes
		{
			get { return Algorithm.LegalKeySizes; }
		}

		#endregion

		/// <summary>
		/// Gets or sets the Initialization Vector used by the current algorithm.
		/// </summary>
		/// <inheritdoc cref="SymmetricAlgorithm.IV"/>
		public byte[] IV
		{
			get { return Algorithm.IV; }
			set { Algorithm.IV = value; }
		}

		#region Block

		/// <summary>
		/// Gets or sets the block size used by the current algorithm (in bits).<br />
		/// If an attempt to set the block size to an invalid value is made, a <see cref="CryptographicException"/> will be thrown.
		/// </summary>
		/// <returns><inheritdoc cref="SymmetricAlgorithm.BlockSize"/></returns>
		/// <exception cref="CryptographicException"/>
		public int BlockSize
		{
			get { return Algorithm.BlockSize; }
			set { Algorithm.BlockSize = value; }
		}

		/// <summary>
		/// Gets or sets the block size used by the current algorithm (in bytes).<br />
		/// If an attempt to set the block size to an invalid value is made, a <see cref="CryptographicException"/> will be thrown.
		/// </summary>
		/// <returns>The block size, in <b>bytes</b>.</returns>
		/// <exception cref="CryptographicException"/>
		public int BlockSizeByte
		{
			get { return Algorithm.BlockSize / 8; }
			set { Algorithm.BlockSize = value * 8; }
		}

		/// <summary>
		/// Gets the valid block sizes for the current algorithm (in bits).
		/// </summary>
		/// <inheritdoc cref="SymmetricAlgorithm.LegalBlockSizes"/>
		public KeySizes[] ValidBlockSizes
		{
			get { return Algorithm.LegalBlockSizes; }
		}

		#endregion

		/// <inheritdoc cref="SymmetricAlgorithm.Mode"/>
		public CipherMode CipherMode
		{
			get { return Algorithm.Mode; }
			set { Algorithm.Mode = value; }
		}

		/// <inheritdoc cref="SymmetricAlgorithm.Padding"/>
		public PaddingMode PaddingMode
		{
			get { return Algorithm.Padding; }
			set { Algorithm.Padding = value; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Generates a random key and sets it as the active key for the cipher.
		/// </summary>
		public void GenerateKey()
		{
			Algorithm.GenerateKey();
		}

		/// <summary>
		/// Generates a random IV and sets it as the active IV for the cipher.
		/// </summary>
		public void GenerateIV()
		{
			Algorithm.GenerateIV();
		}

		/// <summary>
		/// Releases all resouces used by the current <see cref="SymmetricalCipher"/>.
		/// </summary>
		public void Clear()
		{
			ClearEncryptor();
			ClearDecryptor();
			Algorithm.Clear();
		}

		/// <inheritdoc cref="Clear"/>
		public void Dispose()
		{
			Clear();
		}

		#region Encryptor and Decryptor

		/// <summary>
		/// Creates a new encryptor based on the current algorithm and sets it as the current encryptor. Also disposes of the current one if there is any.
		/// </summary>
		public void CreateEncryptor()
		{
			ClearEncryptor();
			Encryptor = Algorithm.CreateEncryptor();
		}

		/// <summary>
		/// Clears the current <see cref="Encryptor"/> and all it's resouces.
		/// </summary>
		public void ClearEncryptor()
		{
			if (Encryptor != null)
			{
				Encryptor.Dispose();
				Encryptor = null;
			}
		}

		/// <summary>
		/// Creates a new decryptor based on the current algorithm and sets it as the current decryptor. Also disposes of the current one if there is any.
		/// </summary>
		public void CreateDecryptor()
		{
			ClearDecryptor();
			Decryptor = Algorithm.CreateDecryptor();
		}

		/// <summary>
		/// Clears the current <see cref="Decryptor"/> and all it's resouces.
		/// </summary>
		public void ClearDecryptor()
		{
			if (Decryptor != null)
			{
				Decryptor.Dispose();
				Decryptor = null;
			}
		}

		#endregion

		/// <summary>
		/// <inheritdoc/><br />
		/// A new <see cref="Encryptor"/> will be generated at the beginning of the decryption process.<br />
		/// This method assumes that the message given to it is complete, therefore it adds padding to it's end and next data you attempt to encrypt will be encrypted using a fresh <see cref="Encryptor"/>.<br />
		/// The string of characters is encoded into bytes using the <see cref="Encoding.UTF8"/>, encrypted and converted a to hexadecimal string.<br />
		/// The <see cref="Key"/> and <see cref="IV"/> must be set before calling this method, otherwise throws an <see cref="Exceptions.InvalidCipherParametersException"/>.
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
		/// A new <see cref="Decryptor"/> will be generated at the beginning of the decryption process.<br />
		/// This method assumes that the message given to it is complete, therefore it attempts to remove padding at it's end and next data you attempt to decrypt will be decrypted using a fresh <see cref="Decryptor"/>.<br />
		/// The string of characters is converted to bytes from the hexadecimal representation, decrypted and decoded back into string using the <see cref="Encoding.UTF8"/>.<br />
		/// The <see cref="Key"/> and <see cref="IV"/> must be set before calling this method, otherwise throws an <see cref="Exceptions.InvalidCipherParametersException"/>.<br />
		/// If the <i>text</i> has incorrect length (not a multiple of <see cref="BlockSizeByte"/>) after converting to bytes, an <see cref="Exceptions.InvalidInputException"/> will be thrown.
		/// </summary>
		/// <param name="text">The ciphertext in hexadecimal.</param>
		/// <inheritdoc/>
		public override string Decrypt(string text)
		{
			byte[] data = Functions.ToByte(text);
			data = Decrypt(data);
			return Encoding.UTF8.GetString(data);
		}

		/// <summary>
		/// <inheritdoc/><br />
		/// A new <see cref="Encryptor"/> will be generated at the beginning of the decryption process.<br />
		/// This method assumes that the data given to it is complete, therefore it adds padding to their end and next data you attempt to encrypt will be encrypted using a fresh <see cref="Encryptor"/>.<br />
		/// The <see cref="Key"/> and <see cref="IV"/> must be set before calling this method, otherwise throws an <see cref="Exceptions.InvalidCipherParametersException"/>.<br />
		/// </summary>
		/// <inheritdoc/>
		public override byte[] Encrypt(byte[] data)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));

			if (ValidSettingsAndInput(true, data.Length))
			{
				CreateEncryptor();

				ICryptoTransform crypto = Encryptor;

				int blockSizeInBytes = BlockSizeByte;
				byte[] result = new byte[data.Length + (blockSizeInBytes - (data.Length % blockSizeInBytes))];
				int finalBlockStartIndex = data.Length - (data.Length % blockSizeInBytes);

				for (int i = 0; i < finalBlockStartIndex; i += blockSizeInBytes)
				{
					crypto.TransformBlock(data, i, blockSizeInBytes, result, i);

					Progress = (double)i / data.Length;
				}
				byte[] final = crypto.TransformFinalBlock(data, finalBlockStartIndex, data.Length - finalBlockStartIndex);

				for (int i = 0; i < final.Length; i++)
				{
					result[finalBlockStartIndex + i] = final[i];
				}

				Progress = 1;
				return result;
			}
			else
				return null;
		}

		/// <summary>
		/// <inheritdoc/><br />
		/// A new <see cref="Decryptor"/> will be generated at the beginning of the decryption process.<br />
		/// This method assumes that the data given to it is complete, therefore it removes padding at their end and next data you attempt to decrypt will be decrypted using a fresh <see cref="Decryptor"/>.<br />
		/// The <see cref="Key"/> and <see cref="IV"/> must be set before calling this method, otherwise throws an <see cref="Exceptions.InvalidCipherParametersException"/>.<br />
		/// If the given <i>data</i> has incorrect length (isn't a multiple of <see cref="BlockSizeByte"/>) an <see cref="Exceptions.InvalidInputException"/> will be thrown.
		/// </summary>
		/// <inheritdoc/>
		public override byte[] Decrypt(byte[] data)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));

			if (ValidSettingsAndInput(false, data.Length))
			{
				CreateDecryptor();

				ICryptoTransform crypto = Decryptor;

				int blockSizeInBytes = BlockSizeByte;
				int finalBlockStartIndex = data.Length - blockSizeInBytes;

				byte[] buffer = new byte[finalBlockStartIndex];
				int start = 0;

				//Drop the first block (that's caused to be all-zeros by these padding modes)
				if (PaddingMode == PaddingMode.PKCS7 || PaddingMode == PaddingMode.ANSIX923 || PaddingMode == PaddingMode.ISO10126)
				{
					byte[] dump = new byte[blockSizeInBytes];
					crypto.TransformBlock(data, 0, blockSizeInBytes, dump, 0);

					start += blockSizeInBytes;
					buffer = new byte[finalBlockStartIndex - blockSizeInBytes];
				}

				//Actual decryption from here
				for (int i = start; i < finalBlockStartIndex; i += blockSizeInBytes)
				{
					crypto.TransformBlock(data, i, blockSizeInBytes, buffer, i - start);

					Progress = (double)i / data.Length;
				}
				byte[] final = crypto.TransformFinalBlock(data, finalBlockStartIndex, blockSizeInBytes);

				if (PaddingMode == PaddingMode.Zeros)   //Remove padding null-bytes for PaddingMode.Zeros (Not done automatically by the crypto)
					final = RemovePaddingZeros(final);

				byte[] result = new byte[buffer.Length + final.Length];
				buffer.CopyTo(result, 0);
				final.CopyTo(result, buffer.Length);

				return result;
			}
			else
				return null;
		}

		/// <inheritdoc/>
		protected override bool FileHandler(BinaryReader input, BinaryWriter output, bool encrypt)
		{
			if (input == null)
				throw new ArgumentNullException(nameof(input));
			if (output == null)
				throw new ArgumentNullException(nameof(output));

			if (ValidSettingsAndInput(encrypt, input.BaseStream.Length))
			{
				ICryptoTransform crypto;

				if (encrypt)
				{
					CreateEncryptor();

					crypto = Encryptor;
				}
				else
				{
					CreateDecryptor();

					crypto = Decryptor;
				}

				int blockSizeInBytes = BlockSizeByte;
				long fileSize = input.BaseStream.Length;

				long lastBlockStartIndex = fileSize - (fileSize % blockSizeInBytes);
				if (!encrypt)
					lastBlockStartIndex -= blockSizeInBytes;

				int start = 0;

				//Drop the first block (that's caused to be all-zeros by these padding modes) during decryption
				if (!encrypt && (PaddingMode == PaddingMode.PKCS7 || PaddingMode == PaddingMode.ANSIX923 || PaddingMode == PaddingMode.ISO10126))
				{
					byte[] dump = new byte[blockSizeInBytes];
					crypto.TransformBlock(input.ReadBytes(blockSizeInBytes), 0, blockSizeInBytes, dump, 0);

					start += blockSizeInBytes;
				}

				byte[] bufferIn;
				byte[] bufferOut;
				for (long i = start; i < lastBlockStartIndex; i += blockSizeInBytes)
				{
					Progress = (double)i / fileSize;

					bufferIn = input.ReadBytes(blockSizeInBytes);
					bufferOut = new byte[bufferIn.Length];

					crypto.TransformBlock(bufferIn, 0, bufferIn.Length, bufferOut, 0);

					output.Write(bufferOut);
				}

				bufferIn = input.ReadBytes((int)(fileSize - lastBlockStartIndex));
				bufferOut = crypto.TransformFinalBlock(bufferIn, 0, bufferIn.Length);

				if (!encrypt && PaddingMode == PaddingMode.Zeros)   //Remove padding null-bytes for PaddingMode.Zeros during decryption (Not done automatically by the crypto)
					bufferOut = RemovePaddingZeros(bufferOut);

				output.Write(bufferOut);

				Progress = 1;
				return true;
			}
			else
				return false;
		}

		private static byte[] RemovePaddingZeros(byte[] data)
		{
			int lastDataByte = -1;

			for (int i = data.Length - 1; i > -1; i--)
			{
				if (data[i] != 0)
				{
					lastDataByte = i;
					break;
				}
			}

			byte[] temp = new byte[lastDataByte + 1];
			for (int i = 0; i < temp.Length; i++)
			{
				temp[i] = data[i];
			}

			return temp;
		}

		/// <summary>
		/// Checks whether the current settings of <see cref="Algorithm"/> are valid and whether given data can be processed using those settings.
		/// </summary>
		/// <param name="encryption">True if attempting encryption, false if attempting decryption.</param>
		/// <param name="dataLength">The length of data that's being attempted to be processed.</param>
		/// <returns>True if all settings are valid. Otherwise throws one of the exceptions with message containing information about the problem.</returns>
		/// <exception cref="Exceptions.InvalidCipherParametersException"/>
		/// <exception cref="Exceptions.InvalidInputException"/>
		protected bool ValidSettingsAndInput(bool encryption, long dataLength)
		{
			if (Key == null)
				throw new Exceptions.InvalidCipherParametersException(nameof(Key) + " is not set.");

			if (IV == null && CipherMode != CipherMode.ECB)
				throw new Exceptions.InvalidCipherParametersException(nameof(IV) + " is not set.");

			if (CipherMode == CipherMode.OFB || CipherMode == CipherMode.CTS)
			{
				if (PaddingMode != PaddingMode.None && PaddingMode != PaddingMode.Zeros)
					throw new Exceptions.InvalidCipherParametersException($"{nameof(PaddingMode)} must be either None or Zeros for selected {nameof(CipherMode)}.");
			}

			if (encryption)
			{
				if (CipherMode != CipherMode.OFB && CipherMode != CipherMode.CTS)
				{
					if (PaddingMode == PaddingMode.None && dataLength % BlockSizeByte != 0)
						throw new Exceptions.InvalidInputException($"Invalid data, length of data must be a multiple of {nameof(BlockSizeByte)} for {nameof(PaddingMode)} \"None\" except for \"Stream-like\" {nameof(CipherMode)}s.");
				}
			}
			else
			{
				if (CipherMode != CipherMode.OFB && CipherMode != CipherMode.CTS)
				{
					if (dataLength % BlockSizeByte != 0)
						throw new Exceptions.InvalidInputException($"Invalid data, length of data must be a multiple of {nameof(BlockSizeByte)} during decryption except for \"Stream-like\" {nameof(CipherMode)}s.");
				}
			}

			return true;
		}

		#endregion
	}
}
