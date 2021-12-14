using System.Collections.Generic;
using System.IO;

namespace OldCrypt_Library.Old.Transposition
{
    public class Scytale : Cipher
    {
        private readonly int columnCount;

        /// <summary>
        /// Initiates a new instance of a Scytale cipher.
        /// </summary>
        /// <param name="columnCount">The amount of columns that's to be used in encryption/decryption. Must be more than 0, otherwise throws <see cref="Exceptions.InvalidCipherParametersException"/></param>
        /// <exception cref="Exceptions.InvalidCipherParametersException" />
        public Scytale(int columnCount)
        {
            if (columnCount > 0)
                this.columnCount = columnCount;
            else
                throw new Exceptions.InvalidCipherParametersException(nameof(columnCount) + " must be higher than 0.");
        }

        /// <summary>
        /// Gets the number of columns used for encryption/decryption.
        /// </summary>
        public int ColumnCount
        {
            get { return columnCount; }
        }

        /// <inheritdoc/>
        public override string Encrypt(string text)
        {
            text = base.ApplyIgnoreSpaceAndCase(text);

            if (columnCount > 0)
            {
                string result = "";
                List<string> lines = new List<string>();

                int lineCount = text.Length / columnCount;
                if (text.Length % columnCount > 0)
                    lineCount++;

                for (int i = 0; i < lineCount; i++)
                {
                    lines.Add("");
                }

                for (int y = 0; y < lineCount; y++)
                {
                    for (int x = 0; x < columnCount && y * columnCount + x < text.Length; x++)
                    {
                        lines[y] += text[y * columnCount + x];
                    }
                }

                for (int x = 0; x < columnCount; x++)
                {
                    for (int y = 0; y < lineCount; y++)
                    {
                        if (x < lines[lines.Count - 1].Length)
                            result += lines[y][x];
                        else
                        {
                            if (y < lineCount - 1)
                                result += lines[y][x];
                        }
                    }
                }

                return result;
            }
            else
                throw new Exceptions.InvalidCipherParametersException(nameof(ColumnCount) + " must be higher than 0.");
        }

        /// <inheritdoc/>
        public override string Decrypt(string text)
        {
            if (columnCount > 0)
            {
                string result = "";
                List<string> lines = new List<string>();
                int readCharacter = 0;

                int lineCount = text.Length / columnCount;
                if (text.Length % columnCount > 0)
                    lineCount++;

                for (int i = 0; i < lineCount; i++)
                {
                    lines.Add("");
                }

                //table filling
                int breakpoint = text.Length % columnCount;
                if (breakpoint == 0)
                    breakpoint = columnCount;
                for (int x = 0; x < columnCount; x++)
                {
                    for (int y = 0; y < lineCount; y++)
                    {
                        if (x >= breakpoint)
                        {
                            if (y < lineCount - 1)
                            {
                                lines[y] += text[readCharacter];
                                readCharacter++;
                            }
                        }
                        else
                        {
                            lines[y] += text[readCharacter];
                            readCharacter++;
                        }
                    }
                }

                //table reading
                for (int y = 0; y < lineCount; y++)
                {
                    for (int x = 0; x < lines[y].Length; x++)
                    {
                        result += lines[y][x];
                    }
                }

                return result;
            }
            else
                throw new Exceptions.InvalidCipherParametersException(nameof(ColumnCount) + " must be higher than 0.");
        }

        /// <inheritdoc/>
        public override byte[] Encrypt(byte[] data)
        {
            if (columnCount > 0)
            {
                byte[] output = new byte[data.Length];
                List<List<byte>> lines = new List<List<byte>>();

                int lineCount = data.Length / columnCount;
                if (data.Length % columnCount > 0)
                    lineCount++;

                for (int i = 0; i < lineCount; i++)
                {
                    lines.Add(new List<byte>());
                }

                for (int y = 0; y < lineCount; y++)
                {
                    for (int x = 0; x < columnCount && y * columnCount + x < data.Length; x++)
                    {
                        lines[y].Add(data[y * columnCount + x]);
                    }
                }

                int nextByte = 0;
                for (int x = 0; x < columnCount; x++)
                {
                    for (int y = 0; y < lineCount; y++)
                    {
                        if (x < lines[lines.Count - 1].Count)
                        {
                            output[nextByte] = lines[y][x];
                            nextByte++;
                        }
                        else
                        {
                            if (y < lineCount - 1)
                            {
                                output[nextByte] = lines[y][x];
                                nextByte++;
                            }
                        }
                    }
                }

                return output;
            }
            else
                throw new Exceptions.InvalidCipherParametersException(nameof(ColumnCount) + " must be higher than 0.");
        }

        /// <inheritdoc/>
        public override byte[] Decrypt(byte[] data)
        {
            if (columnCount > 0)
            {
                byte[] output = new byte[data.Length];
                List<List<byte>> lines = new List<List<byte>>();
                int readByte = 0;

                int lineCount = data.Length / columnCount;
                if (data.Length % columnCount > 0)
                    lineCount++;

                for (int i = 0; i < lineCount; i++)
                {
                    lines.Add(new List<byte>());
                }

                //table filling
                int breakpoint = data.Length % columnCount;
                if (breakpoint == 0)
                    breakpoint = columnCount;
                for (int x = 0; x < columnCount; x++)
                {
                    for (int y = 0; y < lineCount; y++)
                    {
                        if (x >= breakpoint)
                        {
                            if (y < lineCount - 1)
                            {
                                lines[y].Add(data[readByte]);
                                readByte++;
                            }
                        }
                        else
                        {
                            lines[y].Add(data[readByte]);
                            readByte++;
                        }
                    }
                }

                //table reading
                int nextByte = 0;
                for (int y = 0; y < lineCount; y++)
                {
                    for (int x = 0; x < lines[y].Count; x++)
                    {
                        output[nextByte] += lines[y][x];
                        nextByte++;
                    }
                }

                return output;
            }
            else
                throw new Exceptions.InvalidCipherParametersException(nameof(ColumnCount) + " must be higher than 0.");
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
        /// <b>Scytale cipher must encrypt the whole file at once. If an attempt to encrypt file larger than <see cref="int.MaxValue"/> is made, the encryption will fail.</b>
        /// </summary>
        /// <inheritdoc/>
        public override bool EncryptFile(BinaryReader input, BinaryWriter output)
        {
            return FileHandler(input, output, true);
        }

        /// <summary>
        /// <inheritdoc/><br />
        /// <b>Scytale cipher must decrypt the whole file at once. If an attempt to decrypt file larger than <see cref="int.MaxValue"/> is made, the decryption will fail.</b>
        /// </summary>
        /// <inheritdoc/>
        public override bool DecryptFile(BinaryReader input, BinaryWriter output)
        {
            return FileHandler(input, output, false);
        }
    }
}
