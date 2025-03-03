using System;

namespace OldCrypt.Library.Old.Substitution
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

			text = text.ToUpperInvariant();
			string result = "";

			foreach (char x in text)
			{
				if (x == ' ')
					result += " ";
				else if (x >= 'A' && x <= 'Z')
					result += $"{x - 'A'}" + " ";
				else
					throw new Exceptions.InvalidInputException("The Latin code only supports latin letters (a - z and A - Z) and whitespace (' ').");

				Progress = (double)result.Length / text.Length;
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
			if (text == null)
				return "";

			string result = "";
			string[] textSplit = text.Split(' ');

			try
			{
				foreach (string x in textSplit)
				{
					if (string.IsNullOrEmpty(x))
						result += " ";
					else
					{
						if (!Int32.TryParse(x, out int value))
							return "Error! Invalid input!";

						if (value > 0 && value < 27)
							result += Convert.ToChar(value + 'A');
						else
							throw new Exceptions.InvalidInputException("The Latin code only supports latin letters (a - z and A - Z) and whitespace (' ').");
					}

					Progress = (double)result.Length / text.Length;
				}
			}
			catch (OverflowException)
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
