using System;

namespace OldCrypt.Library.Data
{
	/// <summary>
	/// A class that simplifies work with a 2D table/array.
	/// </summary>
	public class Table
	{
		private char[,] _array;

		/// <summary>
		/// Initiates a new table with the given array.
		/// </summary>
		/// <param name="array">Array with which the Table will be initiated.</param>
		public Table(char[,] array)
		{
			_array = array ?? (new char[0, 0]);
		}

		/// <summary>
		/// Gets or sets the inner array of the table. If an attempt to set the array to null is made, the array is set to new char[0, 0].
		/// </summary>
		public char[,] Array
		{
			get { return _array; }
			set
			{
				_array = value ?? (new char[0, 0]);
			}
		}

		/// <summary>
		/// Gets a character on the given coordinates
		/// </summary>
		/// <param name="coordinates">Array of coordinates, where on the index 0 is the X coordinate and on the index 1 is the Y coordinate.</param>
		/// <returns>Character on the given coordinates.</returns>
		/// <exception cref="NullReferenceException" />
		/// <exception cref="IndexOutOfRangeException" />
		/// <exception cref="ArgumentNullException" />
		/// <exception cref="Exceptions.InvalidCoordinateLengthException" />
		public char GetChar(int[] coordinates)
		{
			return coordinates == null
				? throw new ArgumentNullException(nameof(coordinates))
				: coordinates.Length == 2
				? GetChar(coordinates[0], coordinates[1])
				: throw new Exceptions.InvalidCoordinateLengthException("The coordinates array must have a length of 2.");
		}

		/// <summary>
		/// Gets a character on the given coordinates.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>Character on the given coordinates</returns>
		/// <exception cref="IndexOutOfRangeException" />
		public char GetChar(int x, int y)
		{
			return _array[x, y];
		}

		/// <summary>
		/// Gets coordinates of the first instance of the given character in the table.
		/// </summary>
		/// <param name="character">Character to look for.</param>
		/// <returns>
		///     The coordinates of the first instance of the given character in a form of an array.<br />
		///     In the array the X coordinate is on the index 0 and Y coordinate is on the index 1.<br />
		///     If the character is not found in the table, returns null.
		/// </returns>
		public int[] GetCoordinates(char character)
		{
			for (int x = 0; x < _array.GetLength(0); x++)
			{
				for (int y = 0; y < _array.GetLength(1); y++)
				{
					if (_array[x, y] == character)
						return new int[] { x, y };
				}
			}

			return null;
		}
	}
}
