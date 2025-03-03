using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OldCrypt_Library.Hashing
{
	/// <summary>
	/// A base class for all Hash algorithms in the <see cref="OldCrypt_Library"/>.
	/// </summary>
	public abstract class Hash : Cipher
	{
		protected HashAlgorithm hashAlgorithm;

		/// <summary>
		/// Encodes the given <i>text</i> using UTF-8 and computes hash from the resulting data. Then converts the result to uppercase hexadecimal.
		/// </summary>
		/// <param name="text">Text from which the hash is to be calculated.</param>
		/// <returns>The hash of the given text in uppercase hexadecimal.</returns>
		public override string Encrypt(string text)
		{
			byte[] hash = Encrypt(Encoding.UTF8.GetBytes(text));

			string result = "";

			foreach (byte x in hash)
			{
				result += Convert.ToString(x, 16).ToUpper().PadLeft(2, '0');
			}

			return result;
		}

		/// <summary>
		/// Throws a <see cref="Exceptions.CipherUnavailableException"/>.
		/// </summary>
		public override string Decrypt(string text)
		{
			throw new Exceptions.CipherUnavailableException("Decryption impossible for hashes");
		}

		/// <summary>
		/// Performs initialization of the HashAlgorithm and computes a hash of the given array of bytes.
		/// </summary>
		/// <param name="data">Data from which the hash is to be calculated.</param>
		/// <returns>The hash calculated from the data.</returns>
		/// <inheritdoc cref="HashAlgorithm.ComputeHash(byte[])"/>
		public override byte[] Encrypt(byte[] data)
		{
			hashAlgorithm.Initialize();

			return hashAlgorithm.ComputeHash(data);
		}

		/// <summary>
		/// Throws a <see cref="Exceptions.CipherUnavailableException"/>.
		/// </summary>
		public override byte[] Decrypt(byte[] data)
		{
			throw new Exceptions.CipherUnavailableException("Decryption impossible for hashes");
		}

		/// <summary>
		/// Resets (re-initializes) the hashing module, calculates the hash of the input file and saves it to the output file.<br />
		/// The result is saved to the output file in a form of uppercase hexadecimal numbers encoded in UTF-8.
		/// </summary>
		/// <param name="input">The file to hash.</param>
		/// <param name="output">The file where the resulting hash will be stored.</param>
		/// <returns>True if the hashing was successful, otherwise false.</returns>
		public override bool EncryptFile(BinaryReader input, BinaryWriter output)
		{
			try
			{
				hashAlgorithm.Initialize();

				byte[] buffer;

				while (true)
				{
					buffer = input.ReadBytes(1024);

					if (buffer.Length == 1024)
						hashAlgorithm.TransformBlock(buffer, 0, buffer.Length, null, 0);
					else
					{
						hashAlgorithm.TransformFinalBlock(buffer, 0, buffer.Length);
						break;
					}
				}

				string hash = "";
				foreach (byte x in hashAlgorithm.Hash)
				{
					hash += Convert.ToString(x, 16).ToUpper().PadLeft(2, '0');
				}

				output.Write(Encoding.UTF8.GetBytes(hash));
			}
			catch
			{ return false; }

			return true;
		}

		/// <summary>
		/// Throws a <see cref="Exceptions.CipherUnavailableException"/>.
		/// </summary>
		public override bool DecryptFile(BinaryReader input, BinaryWriter output)
		{
			throw new Exceptions.CipherUnavailableException("Decryption impossible for hashes");
		}
	}
}
