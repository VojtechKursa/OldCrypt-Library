namespace OldCrypt_Library.Old.Transposition
{
    public class ReverseNeighbors : Cipher
    {
        /// <inheritdoc/>
        public override string Encrypt(string text)
        {
            text = base.ApplyIgnoreSpaceAndCase(text);

            string result = "";

            for (int i = 0; i < text.Length; i += 2)
            {
                if (i + 1 < text.Length)
                {
                    result += text[i + 1];
                    result += text[i];
                }
                else
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
        public override byte[] Encrypt(byte[] data)
        {
            byte[] result = new byte[data.Length];

            for (int i = 0; i < data.Length; i += 2)
            {
                if (i + 1 < data.Length)
                {
                    result[i] = data[i + 1];
                    result[i + 1] = data[i];
                }
                else
                    result[i] = data[i];
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
