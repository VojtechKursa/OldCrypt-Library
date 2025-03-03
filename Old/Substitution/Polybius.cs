using OldCrypt_Library.Data;

namespace OldCrypt_Library.Old.Substitution
{
	public class Polybius : Cipher
	{
		protected PolybiusTable table;

		public Polybius()
		{
			table = new PolybiusTable();
		}

		public Polybius(string key)
		{
			table = new PolybiusTable(key);
		}

		public PolybiusTable Table
		{
			get { return table; }
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
					temp = table.GetCoordinates(x);

					if (temp.Length == 2)   //Valid character
						result += temp + " ";
					else //Invalid character
					{
						if (!strict)
							result += temp + " ";
					}
				}

				encryptedChars++;
				progress = (double)encryptedChars / text.Length;
			}

			return result.Remove(result.Length - 1);
		}

		/// <inheritdoc/>
		public override string Decrypt(string text)
		{
			string[] coordinates = text.Split(' ');
			string result = "";

			int processed = 0;
			foreach (string coordinate in coordinates)
			{
				result += table.GetCharacter(coordinate);

				processed++;
				progress = (double)processed / coordinates.Length;
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
