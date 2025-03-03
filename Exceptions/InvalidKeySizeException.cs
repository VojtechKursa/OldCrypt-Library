using System;

namespace OldCrypt_Library.Exceptions
{
	/// <summary>
	/// An <see cref="Exception"/> that is thrown when key of unsupported length is attempted to be set as a key for modern encryption algorithm.
	/// </summary>
	public class InvalidKeySizeException : Exception
	{
		/// <summary>
		/// Creates a new instance of <see cref="InvalidKeySizeException"/> with no message.
		/// </summary>
		public InvalidKeySizeException() : base()
		{ }

		/// <summary>
		/// Creates a new instance of <see cref="InvalidKeySizeException"/> with the specified message.
		/// </summary>
		/// <param name="message">The message to be included.</param>
		public InvalidKeySizeException(string message) : base(message)
		{ }
	}
}
