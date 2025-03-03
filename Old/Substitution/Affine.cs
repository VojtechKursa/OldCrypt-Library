using System;

namespace OldCrypt.Library.Old.Substitution
{
	public class Affine : Cipher
	{
		#region Values

		public int A { get; protected set; }
		public int B { get; protected set; }

		#endregion

		#region Constructors

		public Affine(int a, int b)
		{
			A = a;
			B = b;
		}

		#endregion

		#region Methods

		/// <summary>
		/// <inheritdoc/><br />
		/// Throws an <see cref="Exceptions.InvalidCipherParametersException"/> if <see cref="A"/> is invalid (lower than 1 or not coprime with size of the alphabet that's being encrypted (26 in this case)).
		/// </summary>
		/// <inheritdoc/>
		public override string Encrypt(string text)
		{
			text = ApplyIgnoreSpaceAndCase(text);

			if (A > 0)
			{
				if (Functions.GreatestCommonDivisor(A, 26) == 1)
				{
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
							temp = Functions.Modulo((A * (temp - 97)) + B, 26) + 97;
							result += wasUpper ? (char)(temp - 32) : (char)temp;
						}
						else
							HandleInvalidCharacter(result, x);

						Progress = (double)result.Length / text.Length;
					}

					return result;
				}
				else
					throw new Exceptions.InvalidCipherParametersException("With this cipher, the " + nameof(A) + " must be coprime with the size of the alphabet that's being encrypted (26 in case of the latin alphabet).");
			}
			else
				throw new Exceptions.InvalidCipherParametersException(nameof(A) + " must be higher than 0.");
		}

		/// <summary>
		/// <inheritdoc/><br />
		/// Throws an <see cref="Exceptions.InvalidCipherParametersException"/> if <see cref="A"/> is invalid (lower than 1 or not coprime with size of the alphabet that's being decrypted (26 in this case)).
		/// </summary>
		/// <inheritdoc/>
		public override string Decrypt(string text)
		{
			if (text == null)
				throw new ArgumentNullException(nameof(text));

			if (A > 0)
			{
				if (Functions.TryModInverse(A, 26, out int inverseA))
				{
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
							temp = Functions.Modulo(inverseA * (temp - 97 - B), 26) + 97;

							result += wasUpper ? (char)(temp - 32) : (char)temp;
						}
						else
							result += x;

						Progress = (double)result.Length / text.Length;
					}

					return result;
				}
				else
					throw new Exceptions.InvalidCipherParametersException("With this cipher, the " + nameof(A) + " must be coprime with the size of the alphabet that's being decrypted (26 in case of the latin alphabet).\nOtherwise there is no Modular Inverse and the encryption cannot be reversed.");
			}
			else
				throw new Exceptions.InvalidCipherParametersException(nameof(A) + " must be higher than 0.");
		}

		/// <summary>
		/// <inheritdoc/><br />
		/// Throws an <see cref="Exceptions.InvalidCipherParametersException"/> if <see cref="A"/> is invalid (lower than 1 or not coprime with size of the alphabet that's being encrypted (256 in this case)).
		/// </summary>
		/// <inheritdoc/>
		public override byte[] Encrypt(byte[] data)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));

			if (A > 0)
			{
				if (Functions.GreatestCommonDivisor(A, 256) == 1)
				{
					byte[] result = new byte[data.Length];

					for (int i = 0; i < data.Length; i++)
					{
						result[i] = (byte)Functions.Modulo((A * data[i]) + B, 256);
					}

					return result;
				}
				else
					throw new Exceptions.InvalidCipherParametersException("With this cipher, the " + nameof(A) + " must be coprime with the size of the alphabet that's being encrypted (256 in case of binary mode).");
			}
			else
				throw new Exceptions.InvalidCipherParametersException(nameof(A) + " must be higher than 0.");
		}

		/// <summary>
		/// <inheritdoc/><br />
		/// Throws an <see cref="Exceptions.InvalidCipherParametersException"/> if <see cref="A"/> is invalid (lower than 1 or not coprime with size of the alphabet that's being decrypted (256 in this case)).
		/// </summary>
		/// <inheritdoc/>
		public override byte[] Decrypt(byte[] data)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));

			if (A > 0)
			{
				if (Functions.TryModInverse(A, 26, out int inverseA))
				{
					byte[] result = new byte[data.Length];

					for (int i = 0; i < data.Length; i++)
					{
						result[i] = (byte)Functions.Modulo(inverseA * (data[i] - B), 256);
					}

					return result;
				}
				else
					throw new Exceptions.InvalidCipherParametersException("With this cipher, the " + nameof(A) + " must be coprime with the size of the alphabet that's being decrypted (256 in case of binary mode).\nOtherwise there is no Modular Inverse and the encryption cannot be reversed.");
			}
			else
				throw new Exceptions.InvalidCipherParametersException(nameof(A) + " must be higher than 0.");
		}

		#endregion
	}
}
