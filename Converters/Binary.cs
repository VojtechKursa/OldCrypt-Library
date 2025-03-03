using System;
using System.Text;

namespace OldCrypt_Library.Converters
{
	public class Binary : Cipher
	{
		/// <inheritdoc/>
		public override string Encrypt(string text)
		{
			string result = "";
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			int encoded = 0;

			foreach (byte x in bytes)
			{
				result += Convert.ToString(x, 2).PadLeft(8, '0') + " ";

				encoded++;
				progress = (double)encoded / bytes.Length;
			}

			return result.Remove(result.Length - 1);
		}

		/// <inheritdoc/>
		public override string Decrypt(string text)
		{
			string[] splitText = text.Split(' ');
			byte[] bytes = new byte[splitText.Length];

			for (int i = 0; i < bytes.Length; i++)
			{
				bytes[i] = Convert.ToByte(splitText[i], 2);

				progress = (double)i / bytes.Length;
			}

			return Encoding.UTF8.GetString(bytes);
		}

		/// <summary>
		/// Throws a <see cref="Exceptions.CipherUnavailableException"/>
		/// </summary>
		public override byte[] Encrypt(byte[] data)
		{
			throw new Exceptions.CipherUnavailableException();
		}

		/// <summary>
		/// Throws a <see cref="Exceptions.CipherUnavailableException"/>
		/// </summary>
		public override byte[] Decrypt(byte[] data)
		{
			throw new Exceptions.CipherUnavailableException();
		}
	}
}
