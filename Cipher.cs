using System;
using System.IO;

namespace OldCrypt.Library
{
	/// <summary>
	/// A base class for all encryption methods in the <see cref="OldCrypt_Library"/>.
	/// </summary>
	public abstract class Cipher
	{
		#region Values

		/// <summary>
		/// A value that stores the progress of the current encryption or decryption operation as a <see cref="double"/> between 0 and 1
		/// </summary>
		/// <returns>The progress of the current operation.</returns>
		public double Progress { get; protected set; }

		/// <summary>
		/// A value indicating whether strict input filtering should be used. This has effect only on the <see cref="Encrypt(string)"/> method.<br />
		/// <u>Expected behaviour:</u><br />
		/// On/True - Characters unsupported by the cipher (characters that cannot be encrypted) are dropped during the encryption process.<br />
		/// Off/False - Characters unsupported by the cipher are, if possible, passed to the output without change.
		/// </summary>
		public bool Strict { get; set; }

		/// <summary>
		/// A value indicating whether the case of the characters should be ignored or respected during the encryption.<br />
		/// If true, the output should be all upper-case.<br />
		/// If false, the output should have the same case as the input, if possible.
		/// </summary>
		public bool IgnoreCase { get; set; }

		/// <summary>
		/// A value indicating whether spaces should be ignored during the encryption.<br />
		/// If true, the output should be without whitespaces.<br />
		/// If false, the output should have whitespaces in the correct position so they reappear correctly after decryption.
		/// </summary>
		public bool IgnoreSpace { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Encrypts a string of characters using the specified encryption method.
		/// </summary>
		/// <param name="text">Text to encrypt.</param>
		/// <returns>A <see cref="string"/> containing the encrypted text.</returns>
		/// <exception cref="Exceptions.InvalidInputException" />
		/// <exception cref="Exceptions.InvalidCipherParametersException" />
		public abstract string Encrypt(string text);

		/// <summary>
		/// Decrypts a string of characters using the specified encryption method.
		/// </summary>
		/// <param name="text">Text to decrypt.</param>
		/// <returns>A <see cref="string"/> containing the decrypted text.</returns>
		/// <exception cref="Exceptions.InvalidInputException" />
		/// <exception cref="Exceptions.InvalidCipherParametersException" />
		public abstract string Decrypt(string text);

		/// <summary>
		/// Encrypts a file using the selected encryption method.<br />
		/// Note: This method doesn't automatically close the reader nor writer when it's finished in case they would be required elsewhere in the program. If you want to close them afterwards, you have to do it manually.
		/// </summary>
		/// <param name="input">The <see cref="BinaryReader"/> opened on the input file.</param>
		/// <param name="output">The <see cref="BinaryWriter"/> opened on the output file.</param>
		/// <returns>True if successful. False if <see cref="Exception"/> unrelated to the encryption was encountered during the processing.</returns>
		/// <exception cref="Exceptions.CipherUnavailableException" />
		/// <exception cref="Exceptions.InvalidCipherParametersException" />
		/// <exception cref="Exceptions.InvalidInputException" />
		public virtual bool EncryptFile(BinaryReader input, BinaryWriter output)
		{
			return FileHandler(input, output, true);
		}

		/// <summary>
		/// Decrypts a file using the selected encryption method.<br />
		/// Note: This method doesn't automatically close the reader nor writer when it's finished in case they would be required elsewhere in the program. If you want to close them afterwards, you have to do it manually.
		/// </summary>
		/// <param name="input">The <see cref="BinaryReader"/> opened on the input file.</param>
		/// <param name="output">The <see cref="BinaryWriter"/> opened on the output file.</param>
		/// <returns>True if successful. False if <see cref="Exception"/> unrelated to the decryption was encountered during the processing.</returns>
		/// <exception cref="Exceptions.CipherUnavailableException" />
		/// <exception cref="Exceptions.InvalidCipherParametersException" />
		/// <exception cref="Exceptions.InvalidInputException" />
		public virtual bool DecryptFile(BinaryReader input, BinaryWriter output)
		{
			return FileHandler(input, output, false);
		}

		/// <summary>
		/// Handles the actual file encryption/decryption for binary encryption mode.<br />
		/// Both <see cref="EncryptFile(BinaryReader, BinaryWriter)"/> and <see cref="DecryptFile(BinaryReader, BinaryWriter)"/> call this method by default.
		/// </summary>
		/// <param name="input">Reference to the input reader.</param>
		/// <param name="output">Reference to the output writer.</param>
		/// <param name="encrypt">True if the file should be encrypted, false if decrypted.</param>
		/// <returns>True if successful. False if <see cref="Exception"/> unrelated to the encryption/decryption was encountered during the processing.</returns>
		/// <exception cref="Exceptions.CipherUnavailableException" />
		/// <exception cref="Exceptions.InvalidCipherParametersException" />
		/// <exception cref="Exceptions.InvalidInputException" />
		/// <exception cref="ArgumentNullException" />
		protected virtual bool FileHandler(BinaryReader input, BinaryWriter output, bool encrypt)
		{
			if (input == null)
				throw new ArgumentNullException(nameof(input));

			if (output == null)
				throw new ArgumentNullException(nameof(output));

			long fileSize = input.BaseStream.Length;
			long processed = 0;
			byte[] bytes;

			while (processed < fileSize)
			{
				bytes = fileSize - processed >= 1024 ? input.ReadBytes(1024) : input.ReadBytes((int)(fileSize - processed));

				bytes = encrypt ? Encrypt(bytes) : Decrypt(bytes);

				output.Write(bytes);

				processed += bytes.Length;
				Progress = (double)processed / fileSize;
			}

			return true;
		}

		/// <summary>
		/// Encrypts an array of <see cref="byte"/>s using the specified encryption method.
		/// </summary>
		/// <param name="data">Array of <see cref="byte"/>s to encrypt.</param>
		/// <returns>Encrypted array of <see cref="byte"/>s.</returns>
		/// <exception cref="Exceptions.InvalidInputException" />
		/// <exception cref="Exceptions.CipherUnavailableException" />
		/// <exception cref="Exceptions.InvalidCipherParametersException" />
		public abstract byte[] Encrypt(byte[] data);

		/// <summary>
		/// Decrypts an array of <see cref="byte"/>s using the specified encryption method.
		/// </summary>
		/// <param name="data">Array of <see cref="byte"/>s to decrypt.</param>
		/// <returns>Decrypted array of <see cref="byte"/>s.</returns>
		/// <exception cref="Exceptions.InvalidInputException" />
		/// <exception cref="Exceptions.CipherUnavailableException" />
		/// <exception cref="Exceptions.InvalidCipherParametersException" />
		public abstract byte[] Decrypt(byte[] data);

		/// <summary>
		/// Applies the <see cref="ignoreSpace"/> and <see cref="ignoreCase"/> parameters to the text.
		/// </summary>
		/// <param name="text">Text to process.</param>
		/// <returns>Processed <i>text</i>, that has all whitespaces removed if <see cref="ignoreSpace"/> is <i>true</i> and all characters converted to upper-case if <see cref="ignoreCase"/> is <i>true</i>.</returns>
		protected string ApplyIgnoreSpaceAndCase(string text)
		{
			string returnValue = text ?? "";

			if (IgnoreSpace)
			{
				while (returnValue.Contains(" "))
				{
					returnValue = returnValue.Remove(returnValue.IndexOf(' '), 1);
				}
			}

			if (IgnoreCase)
				returnValue = returnValue.ToUpperInvariant();

			return returnValue;
		}

		/// <summary>
		/// Provides a standardized way of handling invalid (unencryptable) characters.
		/// </summary>
		/// <param name="input">The string that's being build as the output of the encryption process.</param>
		/// <param name="invalidCharacter">The character that was detected as invalid.</param>
		/// <returns>The <i>input</i> that either has the <i>invalidCharacter</i> attached to it's end or not, based on the character, <see cref="strict"/> and <see cref="ignoreSpace"/>.</returns>
		protected string HandleInvalidCharacter(string input, char invalidCharacter)
		{
			if (invalidCharacter == ' ')
			{
				if (!IgnoreSpace)
					input += invalidCharacter;
			}
			else if (!Strict)
				input += invalidCharacter;

			return input;
		}

		#endregion
	}
}
