using System;

namespace OldCrypt.Library.Old.Substitution
{
	public class Vigenere : Cipher
	{
		#region Values

		/// <summary>
		/// The key that's used for classical mode. The key will be null if it's not set or set to invalid value.
		/// </summary>
		public string Key { get; protected set; }

		/// <summary>
		/// The key that's used for binary mode.
		/// </summary>
		public byte[] BinKey { get; protected set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Initiates a new instance of the Vigenere cipher class for encryption/decryption in classical mode.
		/// </summary>
		/// <param name="key">Key to be used for encryption/decryption. The key can contain only characters a - z and A - Z (will be converted to a - z in the actual key).</param>
		public Vigenere(string key)
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));

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
			if (key == null)
				throw new ArgumentNullException(nameof(key));

			Constructor(key, binKey);
		}

		private void Constructor(string key, byte[] binKey)
		{
			if (key == null)
				throw new ArgumentNullException(nameof(key));

			Key = IsKeyValid(key) ? key.ToUpperInvariant() : null;

			BinKey = binKey != null ? binKey.Length > 0 ? binKey : null : null;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Converts the current <see cref="Key"/> into a key, that can be used for encryption/decryption (converts the characters to 0-based numbers).
		/// </summary>
		/// <returns>Key that can be used for encryption/decryption in classical mode. Or null if <see cref="IsKeyValid()"/> returns false.</returns>
		private char[] CreateEncryptionKey()
		{
			if (IsKeyValid())
			{
				char[] tempKey = new char[Key.Length];
				for (int i = 0; i < tempKey.Length; i++)
				{
					tempKey[i] = (char)(Key[i] - 'A');
				}

				return tempKey;
			}
			else
				return null;
		}

		/// <summary>
		/// Checks whether the current <see cref="Key"/> is valid for the Vigenere cipher.<br />
		/// The key is valid if it is not null, has length > 0 and contains only letters a - z or A - Z.
		/// </summary>
		/// <returns>True if key is valid. Otherwise false.</returns>
		private bool IsKeyValid()
		{
			return IsKeyValid(Key);
		}

		/// <summary>
		/// Checks whether the key is valid for the Vigenere cipher.<br />
		/// The key is valid if it is not null, has length > 0 and contains only letters a - z or A - Z.
		/// </summary>
		/// <param name="key">Key to be checked.</param>
		/// <returns>True if key is valid. Otherwise false.</returns>
		private static bool IsKeyValid(string key)
		{
			if (key != null)
			{
				if (key.Length > 0)
				{
					foreach (char x in key.ToUpperInvariant())
					{
						if (x >= 'A' && x <= 'Z')
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
					if (x >= 'A' && x <= 'Z')
					{
						wasUpper = true;
						temp = x;
					}
					else if (x >= 'a' && x <= 'z')
					{
						wasUpper = false;
						temp = x - 32;
					}
					else
						temp = -1;

					if (temp >= 'A' && temp <= 'Z')
					{
						temp = Functions.Modulo(temp - 'A' + tempKey[currentKeyChar], 26) + 'A';

						result += wasUpper ? (char)temp : (char)(temp + 32);
					}
					else if (temp == -1)
						HandleInvalidCharacter(result, x);

					currentKeyChar++;
					if (currentKeyChar == tempKey.Length)
						currentKeyChar = 0;

					encryptedChars++;
					Progress = (double)encryptedChars / text.Length;
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
		/// <exception cref="ArgumentNullException" />
		public override string Decrypt(string text)
		{
			if (text == null)
				throw new ArgumentNullException(nameof(text));

			char[] tempKey = CreateEncryptionKey();

			if (tempKey != null)
			{
				string result = "";
				int currentKeyChar = 0;
				bool wasUpper = false;
				int temp;

				foreach (char x in text)
				{
					if (x >= 'A' && x <= 'Z')
					{
						wasUpper = true;
						temp = x;
					}
					else if (x >= 'a' && x <= 'z')
					{
						wasUpper = false;
						temp = x - 32;
					}
					else
						temp = -1;

					if (temp >= 'A' && temp <= 'Z')
					{
						temp = Functions.Modulo(temp - 'A' - tempKey[currentKeyChar], 26) + 'A';

						result += wasUpper ? (char)temp : (char)(temp + 32);
					}
					else if (temp == -1)
						result += x;

					currentKeyChar++;
					if (currentKeyChar == tempKey.Length)
						currentKeyChar = 0;

					Progress = (double)result.Length / text.Length;
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
		/// <exception cref="ArgumentNullException" />
		public override byte[] Encrypt(byte[] data)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));

			if (BinKey != null)
			{
				byte[] result = new byte[data.Length];

				for (int i = 0; i < data.Length; i++)
				{
					result[i] = (byte)Functions.Modulo(data[i] + BinKey[i], 256);
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
			if (data == null)
				throw new ArgumentNullException(nameof(data));

			if (BinKey != null)
			{
				byte[] result = new byte[data.Length];

				for (int i = 0; i < data.Length; i++)
				{
					result[i] = (byte)Functions.Modulo(data[i] - BinKey[i], 256);
				}

				return result;
			}
			else
				throw new Exceptions.InvalidCipherParametersException(nameof(BinKey) + " is not set therefore binary mode is unavailable.");
		}

		#endregion
	}
}
