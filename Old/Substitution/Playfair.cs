using OldCrypt.Library.Data;

namespace OldCrypt.Library.Old.Substitution
{
	public class Playfair : Cipher
	{
		public PlayfairTable Table { get; protected set; }

		public Playfair()
		{
			Table = new PlayfairTable();
		}

		public Playfair(string key)
		{
			Table = new PlayfairTable(key);
		}

		/// <summary>
		/// <inheritdoc/><br />
		/// Throws an <see cref="Exceptions.InvalidInputException"/> if the text is invalid (contains characters other than a - z, A - Z, whitespace).
		/// </summary>
		/// <inheritdoc/>
		public override string Encrypt(string text)
		{
			text = PrepareForEncryption(text);
			string result = "";

			char[] encryptedPair;
			for (int i = 0; i < text.Length; i += 2)
			{
				encryptedPair = Table.Encrypt(new char[] { text[i], text[i + 1] });
				result += encryptedPair[0].ToString() + encryptedPair[1].ToString();

				Progress = (double)result.Length / text.Length;
			}

			return result;
		}

		/// <summary>
		/// Prepares the given text for encryption (removes spaces, converts to upper-case and replaces all 'J' by 'I').<br />
		/// Throws a <see cref="Exceptions.InvalidInputException" /> if invalid characters are detected in the input.
		/// </summary>
		/// <param name="text">Text to prepare.</param>
		/// <returns>Text prepared for encryption.</returns>
		/// <exception cref="Exceptions.InvalidInputException" />
		protected static string PrepareForEncryption(string text)
		{
			if (text == null)
				return "";

			string result = text.Replace(" ", "").ToUpperInvariant().Replace('J', 'I');

			if (IsValid(result))
			{
				for (int i = 0; i < result.Length; i += 2)
				{
					if (i + 1 < result.Length)
					{
						if (result[i] == result[i + 1])
							result = result.Insert(i + 1, "X");
					}
					else
						result += "X";
				}

				return result;
			}
			else
				throw new Exceptions.InvalidInputException("The Playfair cipher only supports latin letters (a - z and A - Z) and whitespaces (' ').");
		}

		/// <summary>
		/// Checks whether the given text is valid for the standard Playfair cipher.
		/// </summary>
		/// <param name="text">The text to check.</param>
		/// <returns>True if the text is valid for the standard Playfair cipher, otherwise false.</returns>
		protected static bool IsValid(string text)
		{
			if (text == null)
				return false;

			foreach (char x in text)
			{
				if (x > 64 && x < 91)
					continue;
				else
					return false;
			}

			return true;
		}

		/// <summary>
		/// <inheritdoc/><br />
		/// Throws an <see cref="Exceptions.InvalidInputException"/> if the amount of characters in the text is odd.<br />
		/// Throws an <see cref="Exceptions.InvalidInputException"/> if the text is invalid (contains characters other than a - z, A - Z, whitespace).
		/// </summary>
		/// <inheritdoc/>
		public override string Decrypt(string text)
		{
			if (text == null)
				return "";

			if (text.Length % 2 != 0)
				throw new Exceptions.InvalidInputException("The Playfair cipher decryption only supports even amount of characters.");

			text = text.Replace(" ", "").ToUpperInvariant().Replace('J', 'I');

			if (IsValid(text))
			{
				string result = "";

				char[] decryptedPair;
				for (int i = 0; i < text.Length; i += 2)
				{
					decryptedPair = Table.Decrypt(new char[] { text[i], text[i + 1] });
					result += decryptedPair[0].ToString() + decryptedPair[1].ToString();

					Progress = (double)result.Length / text.Length;
				}

				return result;
			}
			else
				throw new Exceptions.InvalidInputException("The Playfair cipher only supports latin letters (a - z and A - Z) and whitespaces (' ').");
		}

		/// <summary>
		/// Throws a <see cref="Exceptions.CipherUnavailableException"/>.
		/// </summary>
		public override byte[] Encrypt(byte[] data)
		{
			throw new Exceptions.CipherUnavailableException("Binary mode unavailable for the Playfair cipher.");
		}

		/// <summary>
		/// Throws a <see cref="Exceptions.CipherUnavailableException"/>.
		/// </summary>
		public override byte[] Decrypt(byte[] data)
		{
			throw new Exceptions.CipherUnavailableException("Binary mode unavailable for the Playfair cipher.");
		}
	}
}
