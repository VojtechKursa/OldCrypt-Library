using System.IO;

namespace OldCrypt_Library.Old.Transposition
{
    public class Reverse : Cipher
    {
        /// <inheritdoc/>
        public override string Encrypt(string text)
        {
            text = base.ApplyIgnoreSpaceAndCase(text);

            string result = "";

            for (int i = text.Length - 1; i > -1; i--)
            {
                result += text[i];

                progress = (double)result.Length / text.Length;
            }

            return result;
        }

        /// <inheritdoc/>
        public override string Decrypt(string text)
        {
            return Encrypt(text);
        }

        /// <inheritdoc/>
        public override bool EncryptFile(BinaryReader input, BinaryWriter output)
        {
            try
            {
                Stream inputStream = input.BaseStream;
                long processed = 0;

                inputStream.Position = inputStream.Length - 1;
                while (inputStream.Position > -1)
                {
                    output.Write(inputStream.ReadByte());
                    inputStream.Position -= 2;
                    processed++;

                    progress = (double)processed / inputStream.Length;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public override bool DecryptFile(BinaryReader input, BinaryWriter output)
        {
            return EncryptFile(input, output);
        }

        /// <inheritdoc/>
        public override byte[] Encrypt(byte[] data)
        {
            byte[] result = new byte[data.Length];
            int lastIndex = 0;

            for (int i = result.Length - 1; i > -1; i--)
            {
                result[lastIndex] = data[i];
                lastIndex++;

                progress = (double)lastIndex / data.Length;
            }

            return result;
        }

        /// <inheritdoc/>
        public override byte[] Decrypt(byte[] data)
        {
            return Encrypt(data);
        }
    }
}
