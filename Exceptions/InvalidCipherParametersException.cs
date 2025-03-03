using System;

namespace OldCrypt.Library.Exceptions
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

		/// <summary>
		/// Creates a new instance of <see cref="InvalidCipherParametersException"/> with the specified message and inner exception.
		/// </summary>
		/// <param name="message">The message to be included.</param>
		/// <param name="innerException">The inner exception.</param>
		public InvalidCipherParametersException(string message, Exception innerException) : base(message, innerException)
		{ }
	}
}
