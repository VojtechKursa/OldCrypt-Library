using System;

namespace OldCrypt.Library.Exceptions
{
	/// <summary>
	/// An <see cref="Exception"/>, that is thrown when the called encryption/decryption method is not available.<br />
	/// Usual reason for the unavailability is the impossibility of implementation caused by the specifics of the selected cipher.
	/// </summary>
	public class CipherUnavailableException : Exception
	{
		/// <summary>
		/// Creates a new instance of <see cref="CipherUnavailableException"/> with no message.
		/// </summary>
		public CipherUnavailableException() : base()
		{ }

		/// <summary>
		/// Creates a new instance of <see cref="CipherUnavailableException"/> with the specified message.
		/// </summary>
		/// <param name="message">The message to be included.</param>
		public CipherUnavailableException(string message) : base(message)
		{ }

		/// <summary>
		/// Creates a new instance of <see cref="CipherUnavailableException"/> with the specified message and inner exception.
		/// </summary>
		/// <param name="message">The message to be included.</param>
		/// <param name="innerException">The inner exception.</param>
		public CipherUnavailableException(string message, Exception innerException) : base(message, innerException)
		{ }
	}
}
