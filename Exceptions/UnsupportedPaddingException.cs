using System;

namespace OldCrypt.Library.Exceptions
{
	/// <summary>
	/// An <see cref="Exception"/> that is thrown when a table lookup is attempted with an array of coordinates that has <see cref="Array.Length"/> not equal to the <see cref="Array.Rank"/> of the table.
	/// </summary>
	public class UnsupportedPaddingException : Exception
	{
		/// <summary>
		/// Creates a new instance of <see cref="UnsupportedPaddingException "/> with no message.
		/// </summary>
		public UnsupportedPaddingException() : base()
		{ }

		/// <summary>
		/// Creates a new instance of <see cref="UnsupportedPaddingException "/> with the specified message.
		/// </summary>
		/// <param name="message">The message to be included.</param>
		public UnsupportedPaddingException(string message) : base(message)
		{ }

		/// <summary>
		/// Creates a new instance of <see cref="UnsupportedPaddingException "/> with the specified message and inner exception.
		/// </summary>
		/// <param name="message">The message to be included.</param>
		/// <param name="innerException">The inner exception.</param>
		public UnsupportedPaddingException(string message, Exception innerException) : base(message, innerException)
		{ }
	}
}
