using System;

namespace OldCrypt.Library.Data
{
	/// <summary>
	/// A class that represents a table used for <see cref="Old.Substitution.Polybius"/> cipher.
	/// </summary>
	public class PolybiusTable : PolybiusSquare
	{
		#region Constructors

		/// <summary>
		/// Initiates a new Polybius table with no key.
		/// </summary>
		public PolybiusTable()
		{
			GenerateTable("");
		}

		/// <summary>
		/// Initiates a new Polybius table based on the given key.
		/// </summary>
		/// <param name="key">Key based on which the table will be generated.</param>
		public PolybiusTable(string key)
		{
			GenerateTable(key);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Returns the Polybius table coordinates of the given character.
		/// </summary>
		/// <param name="character">Character to convert.</param>
		/// <returns>Polybius table coordinates of the given character or the character itself if it was not found in the table.</returns>
		public string GetCoordinates(char character)
		{
			if (character > 96 && character < 123)  //Convert to uppercase
				character = (char)(character - 32);

			int[] coordinates = Table.GetCoordinates(character);

			return coordinates != null ? $"{coordinates[1] + 1}" + $"{coordinates[0] + 1}" : character.ToString();
		}

		/// <summary>
		/// Returns the character on the given coordinates.
		/// </summary>
		/// <param name="coordinates"><see cref="string"/> containing a Polybius table coordinates.</param>
		/// <returns>
		/// If valid coordinates are given, the character on the given coordinates.<br />
		/// If given coordinates are invalid returns:<br />
		/// Whitespace if given string is empty.<br />
		/// The character in the string if given string has just 1 character.<br />
		/// Else '#'.
		/// </returns>
		public char GetCharacter(string coordinates)
		{
			if (coordinates == null)
				coordinates = "";

			try
			{
				int y = Convert.ToInt32(coordinates[0]);
				int x = Convert.ToInt32(coordinates[1]);

				return Table.GetChar(x - 1, y - 1);
			}
			catch (IndexOutOfRangeException)
			{
				return coordinates.Length == 0 ? ' ' : coordinates.Length == 1 ? coordinates[0] : '#';
			}
		}

		#endregion
	}
}
