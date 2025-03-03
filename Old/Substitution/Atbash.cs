namespace OldCrypt_Library.Old.Substitution
{
	public class Atbash : Cipher
	{
		/// <inheritdoc/>
		public override string Encrypt(string text)
		{
			text = ApplyIgnoreSpaceAndCase(text);

			string result = "";
			bool wasUpper;
			int temp;

			foreach (char x in text)
			{
				if (x > 64 && x < 91)
				{
					wasUpper = true;
					temp = x + 32;
				}
				else
				{
					wasUpper = false;
					temp = x;
				}

				if (x > 96 && x < 123)
				{
					temp = (25 - (temp - 97)) + 97;

					if (wasUpper)
						temp -= 32;

					result += (char)temp;
				}
				else
					HandleInvalidCharacter(result, x);

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

			for (int i = 0; i < data.Length; i++)
			{
				result[i] = (byte)(255 - data[i]);
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
