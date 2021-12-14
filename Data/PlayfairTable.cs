using System;

namespace OldCrypt_Library.Data
{
    /// <summary>
    /// A class that represents a table used for <see cref="Old.Substitution.Playfair"/> cipher.
    /// </summary>
    public class PlayfairTable : PolybiusSquare
    {
        /// <summary>
        /// Creates a new instance of the <see cref="PlayfairTable"/> with no key.
        /// </summary>
        public PlayfairTable()
        {
            GenerateTable("");
        }

        /// <summary>
        /// Creates a new instance of the <see cref="PlayfairTable"/> with the specified key.
        /// </summary>
        /// <param name="key">Key based on which the <see cref="PlayfairTable"/> is to be generated.</param>
        public PlayfairTable(string key)
        {
            GenerateTable(key);
        }

        /// <summary>
        /// Encrypts the given pair of characters using the Playfair cipher rules and the current table.
        /// </summary>
        /// <param name="characters">Array containing ONE PAIR of characters to encrypt.</param>
        /// <returns>The encrypted pair of characters or null if <see cref="Array.Length"/> of "characters" != 2 or at least one of the characters is not found in the table.</returns>
        public char[] Encrypt(char[] characters)
        {
            if (characters.Length == 2)
            {
                int[] char1Coords = table.GetCoordinates(characters[0]);
                int[] char2Coords = table.GetCoordinates(characters[1]);

                if (char1Coords == null || char2Coords == null)
                    return null;

                if (char1Coords[1] == char2Coords[1])   //same row -> shift right
                {
                    char1Coords[0] = (char1Coords[0] + 1) % 5;
                    char2Coords[0] = (char2Coords[0] + 1) % 5;
                }
                else if (char1Coords[0] == char2Coords[0])   //same collumn -> shift down
                {
                    char1Coords[1] = (char1Coords[1] + 1) % 5;
                    char2Coords[1] = (char2Coords[1] + 1) % 5;
                }
                else
                {
                    int temp = char1Coords[0];
                    char1Coords[0] = char2Coords[0];
                    char2Coords[0] = temp;
                }

                return new char[] { table.GetChar(char1Coords), table.GetChar(char2Coords) };
            }
            else
                return null;
        }

        /// <summary>
        /// Decrypts the given pair of characters using the Playfair cipher rules and the current table.
        /// </summary>
        /// <param name="characters">Array containing ONE PAIR of characters to decrypt.</param>
        /// <returns>The decrypted pair of characters or null if <see cref="Array.Length"/> of "characters" != 2 or at least one of the characters is not found in the table.</returns>
        public char[] Decrypt(char[] characters)
        {
            if (characters.Length == 2)
            {
                int[] char1Coords = table.GetCoordinates(characters[0]);
                int[] char2Coords = table.GetCoordinates(characters[1]);

                if (char1Coords == null || char2Coords == null)
                    return null;

                if (char1Coords[1] == char2Coords[1])   //same row -> shift left
                {
                    char1Coords[0] = (char1Coords[0] + 4) % 5;
                    char2Coords[0] = (char2Coords[0] + 4) % 5;
                }
                else if (char1Coords[0] == char2Coords[0])   //same collumn -> shift up
                {
                    char1Coords[1] = (char1Coords[1] + 4) % 5;
                    char2Coords[1] = (char2Coords[1] + 4) % 5;
                }
                else
                {
                    int temp = char1Coords[0];
                    char1Coords[0] = char2Coords[0];
                    char2Coords[0] = temp;
                }

                return new char[] { table.GetChar(char1Coords), table.GetChar(char2Coords) };
            }
            else
                return null;
        }
    }
}
