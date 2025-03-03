using System;

namespace OldCrypt_Library.Old.Substitution
{
	public class Caesar : Cipher
	{
		private int a;

		#region Constructors

		public Caesar()
		{
			a = 0;
		}

		public Caesar(int a)
		{
			this.a = a;
		}

		#endregion


		#region Getters and Setters

		public int A
		{
			get { return a; }
			set { a = value; }
		}

		#endregion


		#region Methods

		/// <inheritdoc/>
		public override string Encrypt(string text)
		{
			text = ApplyIgnoreSpaceAndCase(text);

			if (a == 0)
				return text;

			string result = "";
			int temp;
			bool wasUpper;

			foreach (char x in text)
			{
				if (x == ' ')
				{
					result += " ";
					continue;
				}

				temp = x;

				if (temp > 64 && temp < 91)
				{
					wasUpper = true;
					temp += 32;
				}
				else
					wasUpper = false;

				if (temp > 96 && temp < 123)
				{
					temp = Functions.Modulo((temp - 97) + a, 26);

					temp += 97;
				}
				else
					HandleInvalidCharacter(result, x);

				result += wasUpper ? Convert.ToChar(temp - 32) : Convert.ToChar(temp);

				progress = (double)result.Length / text.Length;
			}

			return result;
		}

		/// <inheritdoc/>
		public override string Decrypt(string text)
		{
			a = -a;
			string result = Encrypt(text);
			a = -a;

			return result;
		}

		/// <inheritdoc/>
		public override byte[] Encrypt(byte[] data)
		{
			byte[] result = new byte[data.Length];
			int temp;

			for (int i = 0; i < result.Length; i++)
			{
				temp = Functions.Modulo(data[i] + a, 256);

				result[i] = (byte)temp;
			}

			return result;
		}

		/// <inheritdoc/>
		public override byte[] Decrypt(byte[] data)
		{
			a = -a;
			byte[] result = Encrypt(data);
			a = -a;

			return result;
		}

		#endregion
	}
}
