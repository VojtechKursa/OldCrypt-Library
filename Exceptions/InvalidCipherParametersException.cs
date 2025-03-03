using System;

namespace OldCrypt_Library.Exceptions
{
	/// <summary>
	/// An <see cref="Exception"/> that is thrown when cipher parameters are not valid for the selected cipher.
	/// </summary>
	public class InvalidCipherParametersException : Exception
	{
		/// <summary>
		/// Creates a new instance of <see cref="InvalidCipherParametersException"/> with no message.
		/// </summary>
		public InvalidCipherParametersException() : base()
		{ }

		/// <summary>
		/// Creates a new instance of <see cref="InvalidCipherParametersException"/> with the specified message.
		/// </summary>
		/// <param name="message">The message to be included.</param>
		public InvalidCipherParametersException(string message) : base(message)
		{ }
	}
}
