using OldCrypt.Library.Data;

namespace OldCrypt.Library.Old.Substitution
{
	public class Polybius : Cipher
	{
		public PolybiusTable Table { get; protected set; }

		public Polybius()
		{
			Table = new PolybiusTable();
		}

		public Polybius(string key)
		{
			Table = new PolybiusTable(key);
		}

		/// <inheritdoc/>
		public override string Encrypt(string text)
		{
			text = ApplyIgnoreSpaceAndCase(text);

			string result = "";
			string temp;

			int encryptedChars = 0;
			foreach (char x in text)
			{
				if (x == ' ')
					result += " ";
				else
				{
					temp = Table.GetCoordinates(x);

					if (temp.Length == 2)   //Valid character
						result += temp + " ";
					else //Invalid character
					{
						if (!Strict)
							result += temp + " ";
					}
				}

				encryptedChars++;
				Progress = (double)encryptedChars / text.Length;
			}

			return result.Remove(result.Length - 1);
		}

		/// <inheritdoc/>
		public override string Decrypt(string text)
		{
			if (text == null)
				return "";

			string[] coordinates = text.Split(' ');
			string result = "";

			int processed = 0;
			foreach (string coordinate in coordinates)
			{
				result += Table.GetCharacter(coordinate);

				processed++;
				Progress = (double)processed / coordinates.Length;
			}

			return result;
		}

		/// <summary>
		/// Throws a <see cref="Exceptions.CipherUnavailableException"/>
		/// </summary>
		public override byte[] Encrypt(byte[] data)
		{
			throw new Exceptions.CipherUnavailableException("Binary mode unavailable for the PolybiusSquare cipher.");
		}

		/// <summary>
		/// Throws a <see cref="Exceptions.CipherUnavailableException"/>
		/// </summary>
		public override byte[] Decrypt(byte[] data)
		{
			throw new Exceptions.CipherUnavailableException("Binary mode unavailable for the PolybiusSquare cipher.");
		}
	}
}
