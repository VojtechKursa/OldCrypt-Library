using OldCrypt_Library.Data;

namespace OldCrypt_Library.Old.Substitution
{
	public class Playfair : Cipher
	{
		protected PlayfairTable table;

		public Playfair()
		{
			table = new PlayfairTable();
		}

		public Playfair(string key)
		{
			table = new PlayfairTable(key);
		}

		public PlayfairTable Table
		{
			get { return table; }
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
				encryptedPair = table.Encrypt(new char[] { text[i], text[i + 1] });
				result += encryptedPair[0].ToString() + encryptedPair[1].ToString();

				progress = (double)result.Length / text.Length;
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
		protected string PrepareForEncryption(string text)
		{
			string result = text.Replace(" ", "").ToUpper().Replace('J', 'I');

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
		protected bool IsValid(string text)
		{
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
			if (text.Length % 2 != 0)
				throw new Exceptions.InvalidInputException("The Playfair cipher decryption only supports even amount of characters.");

			text = text.Replace(" ", "").ToUpper().Replace('J', 'I');

			if (IsValid(text))
			{
				string result = "";

				char[] decryptedPair;
				for (int i = 0; i < text.Length; i += 2)
				{
					decryptedPair = table.Decrypt(new char[] { text[i], text[i + 1] });
					result += decryptedPair[0].ToString() + decryptedPair[1].ToString();

					progress = (double)result.Length / text.Length;
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
