using System.Collections.Generic;
using System.IO;

namespace OldCrypt_Library.Old.Transposition
{
    public class RailFence : Cipher
    {
        private readonly int rowCount;

        /// <summary>
        /// Initiates a new instance of the Rail fence cipher.
        /// </summary>
        /// <param name="rowCount">The amount of rows to be used for encryption/decryption. Must be more than 0, otherwise throws <see cref="Exceptions.InvalidCipherParametersException"/></param>
        /// <exception cref="Exceptions.InvalidCipherParametersException" />
        public RailFence(int rowCount)
        {
            if (rowCount > 0)
                this.rowCount = rowCount;
            else
                throw new Exceptions.InvalidCipherParametersException(nameof(rowCount) + " must be higher than 0.");
        }

        public int RowCount
        {
            get { return rowCount; }
        }

        /// <inheritdoc/>
        public override string Encrypt(string text)
        {
            text = base.ApplyIgnoreSpaceAndCase(text);

            if (rowCount > 0)
            {
                string result = "";
                List<string> lines = new List<string>();
                int currentRow = 0;
                bool upDown = true;

                //initialize lines
                for (int i = 0; i < rowCount; i++)
                {
                    lines.Add("");
                }

                //assign characters to lines
                foreach (char character in text)
                {
                    lines[currentRow] += character;

                    if (currentRow == rowCount - 1)
                        upDown = false;
                    else if (currentRow == 0)
                        upDown = true;

                    if (upDown)
                        currentRow++;
                    else
                        currentRow--;
                }

                //read results
                foreach (string line in lines)
                {
                    result += line;
                }

                return result;
            }
            else
                throw new Exceptions.InvalidCipherParametersException(nameof(RowCount) + " must be higher than 0.");
        }

        /// <inheritdoc/>
        public override string Decrypt(string text)
        {
            if (rowCount > 0)
            {
                string result = "";
                List<List<char>> lines = new List<List<char>>();
                bool upDown = true;
                int currentRow = 0;

                //initialize lines
                for (int i = 0; i < rowCount; i++)
                {
                    lines.Add(new List<char>());
                }

                //determine number of characters on each line
                foreach (char character in text)
                {
                    lines[currentRow].Add(' ');

                    if (currentRow == rowCount - 1)
                        upDown = false;
                    else if (currentRow == 0)
                        upDown = true;

                    if (upDown)
                        currentRow++;
                    else
                        currentRow--;
                }

                //fill lines based on number of characters on each line
                int currentCharacter = 0;
                foreach (List<char> line in lines)
                {
                    for (int i = 0; i < line.Count; i++)
                    {
                        line[i] = text[currentCharacter];
                        currentCharacter++;
                    }
                }

                //read lines
                currentRow = 0;
                upDown = true;
                List<int> currentCharacters = new List<int>();
                for (int i = 0; i < rowCount; i++)
                {
                    currentCharacters.Add(0);
                }
                for (int i = 0; i < text.Length; i++)
                {
                    result += lines[currentRow][currentCharacters[currentRow]];
                    currentCharacters[currentRow]++;

                    if (currentRow == rowCount - 1)
                        upDown = false;
                    else if (currentRow == 0)
                        upDown = true;

                    if (upDown)
                        currentRow++;
                    else
                        currentRow--;
                }

                return result;
            }
            else
                throw new Exceptions.InvalidCipherParametersException(nameof(RowCount) + " must be higher than 0.");
        }

        /// <inheritdoc/>
        public override byte[] Encrypt(byte[] data)
        {
            if (rowCount > 0)
            {
                byte[] output = new byte[data.Length];
                List<List<byte>> lines = new List<List<byte>>();
                int currentRow = 0;
                bool upDown = true;

                //initiate lines
                for (int i = 0; i < rowCount; i++)
                {
                    lines.Add(new List<byte>());
                }

                //assign characters to lines
                foreach (byte x in data)
                {
                    lines[currentRow].Add(x);

                    if (currentRow == rowCount - 1)
                        upDown = false;
                    else if (currentRow == 0)
                        upDown = true;

                    if (upDown)
                        currentRow++;
                    else
                        currentRow--;
                }

                //read result
                int nextByte = 0;
                for (int y = 0; y < rowCount; y++)
                {
                    for (int x = 0; x < lines[y].Count; x++)
                    {
                        output[nextByte] = lines[y][x];
                        nextByte++;
                    }
                }

                return output;
            }
            else
                throw new Exceptions.InvalidCipherParametersException(nameof(RowCount) + " must be higher than 0.");
        }

        /// <inheritdoc/>
        public override byte[] Decrypt(byte[] data)
        {
            if (rowCount > 0)
            {
                byte[] output = new byte[data.Length];
                List<List<byte>> lines = new List<List<byte>>();
                bool upDown = true;
                int currentRow = 0;

                //initiate lines
                for (int i = 0; i < rowCount; i++)
                {
                    lines.Add(new List<byte>());
                }

                //determine number of bytes on each line
                foreach (byte x in data)
                {
                    lines[currentRow].Add(0);

                    if (currentRow == rowCount - 1)
                        upDown = false;
                    else if (currentRow == 0)
                        upDown = true;

                    if (upDown)
                        currentRow++;
                    else
                        currentRow--;
                }

                //fill lines based on amount of bytes on each line
                int currentByte = 0;
                foreach (List<byte> line in lines)
                {
                    for (int i = 0; i < line.Count; i++)
                    {
                        line[i] = data[currentByte];
                        currentByte++;
                    }
                }

                //read lines
                int nextByte = 0;
                currentRow = 0;
                upDown = true;
                List<int> currentBytes = new List<int>();
                for (int i = 0; i < rowCount; i++)
                {
                    currentBytes.Add(0);
                }
                for (int i = 0; i < data.Length; i++)
                {
                    output[nextByte] = lines[currentRow][currentBytes[currentRow]];
                    nextByte++;
                    currentBytes[currentRow]++;

                    if (currentRow == rowCount - 1)
                        upDown = false;
                    else if (currentRow == 0)
                        upDown = true;

                    if (upDown)
                        currentRow++;
                    else
                        currentRow--;
                }

                return output;
            }
            else
                throw new Exceptions.InvalidCipherParametersException(nameof(RowCount) + " must be higher than 0.");
        }

        /// <inheritdoc/>
        protected override bool FileHandler(BinaryReader input, BinaryWriter output, bool encrypt)
        {
            try
            {
                long fileSize = input.BaseStream.Length;

                if (fileSize > int.MaxValue)
                    return false;

                byte[] bytes = input.ReadBytes((int)fileSize);

                if (encrypt)
                    bytes = Encrypt(bytes);
                else
                    bytes = Decrypt(bytes);

                output.Write(bytes);

                progress = 1;
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// <inheritdoc/><br />
        /// <b>Rail fence cipher must encrypt the whole file at once. If an attempt to encrypt file larger than <see cref="int.MaxValue"/> is made, the encryption will fail.</b>
        /// </summary>
        /// <inheritdoc/>
        public override bool EncryptFile(BinaryReader input, BinaryWriter output)
        {
            return FileHandler(input, output, true);
        }

        /// <summary>
        /// <inheritdoc/><br />
        /// <b>Rail fence cipher must decrypt the whole file at once. If an attempt to decrypt file larger than <see cref="int.MaxValue"/> is made, the decryption will fail.</b>
        /// </summary>
        /// <inheritdoc/>
        public override bool DecryptFile(BinaryReader input, BinaryWriter output)
        {
            return FileHandler(input, output, false);
        }
    }
}
