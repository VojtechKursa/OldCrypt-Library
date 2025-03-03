using System;
using System.Collections.Generic;

namespace OldCrypt.Library.Data
{
	/// <summary>
	/// The base class for <see cref="PolybiusTable"/> and <see cref="PlayfairTable"/>.
	/// </summary>
	public class PolybiusSquare
	{
		public Table Table { get; protected set; }

		/// <summary>
		/// Generates a new table based on the specified key and sets it as <see cref="Table"/>.
		/// </summary>
		/// <param name="key">The key based on which the <see cref="Table"/> is to be generated.</param>
		protected void GenerateTable(string key)
		{
			char[,] table = new char[5, 5];
			List<char> usedCharacters = new List<char>
			{
				'J'    //So it won't be added when finishing the table
            };

			if (key == null)
				key = "";
			key = key.ToUpperInvariant().Replace('J', 'I');

			int insertedChars = 0;
			foreach (char x in key)
			{
				if (x > 64 && x < 91)
				{
					if (usedCharacters.Contains(x))
						continue;
					else
					{
						table[(int)Math.Floor((double)insertedChars / 5), insertedChars % 5] = x;
						insertedChars++;
						usedCharacters.Add(x);
					}
				}
			}

			char character;
			for (int i = 65; i < 91; i++)
			{
				if (insertedChars == 25)
					break;

				character = (char)i;

				if (usedCharacters.Contains(character))
					continue;
				else
				{
					table[insertedChars % 5, (int)Math.Floor((double)insertedChars / 5)] = character;
					insertedChars++;
					usedCharacters.Add(character);
				}
			}

			Table = new Table(table);
		}
	}
}
