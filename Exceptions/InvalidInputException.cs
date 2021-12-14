using System;

namespace OldCrypt_Library.Exceptions
{
    /// <summary>
    /// An <see cref="Exception"/> that is thrown when input that is not supported by the selected cipher is fed into it.<br />
    /// For example an attempt to encrypt a number is made, but the selected cipher only supports letters.
    /// </summary>
    public class InvalidInputException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="InvalidInputException"/> with no message.
        /// </summary>
        public InvalidInputException() : base()
        { }

        /// <summary>
        /// Creates a new instance of <see cref="InvalidInputException"/> with the specified message.
        /// </summary>
        /// <param name="message">The message to be included.</param>
        public InvalidInputException(string message) : base(message)
        { }
    }
}
