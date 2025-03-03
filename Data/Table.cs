using System;

namespace OldCrypt_Library.Data
{
	/// <summary>
	/// A class that simplifies work with a 2D table/array.
	/// </summary>
	public class Table
	{
		private char[,] array;

		/// <summary>
		/// Initiates a new table with the given array.
		/// </summary>
		/// <param name="array">Array with which the Table will be initiated.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "array value must not be null to avoid unneccessary NullReferenceExceptions.")]
		public Table(char[,] array)
		{
			if (array != null)
				this.array = array;
			else
				array = new char[0, 0];
		}

		/// <summary>
		/// Gets or sets the inner array of the table. If an attempt to set the array to null is made, the array is set to new char[0, 0].
		/// </summary>
		public char[,] Array
		{
			get { return array; }
			set
			{
				if (value != null)
					array = value;
				else
					array = new char[0, 0];
			}
		}

		/// <summary>
		/// Gets a character on the given coordinates
		/// </summary>
		/// <param name="coordinates">Array of coordinates, where on the index 0 is the X coordinate and on the index 1 is the Y coordinate.</param>
		/// <returns>Character on the given coordinates.</returns>
		/// <exception cref="NullReferenceException" />
		/// <exception cref="IndexOutOfRangeException" />
		/// <exception cref="Exceptions.InvalidCoordinateLengthException" />
		public char GetChar(int[] coordinates)
		{
			if (coordinates.Length == 2)
				return GetChar(coordinates[0], coordinates[1]);
			else
				throw new Exceptions.InvalidCoordinateLengthException("The coordinates array must have a length of 2.");
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
			return array[x, y];
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
			for (int x = 0; x < array.GetLength(0); x++)
			{
				for (int y = 0; y < array.GetLength(1); y++)
				{
					if (array[x, y] == character)
						return new int[] { x, y };
				}
			}

			return null;
		}
	}
}
