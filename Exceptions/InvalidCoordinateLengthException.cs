using System;

namespace OldCrypt_Library.Exceptions
{
    /// <summary>
    /// An <see cref="Exception"/> that is thrown when a table lookup is attempted with an array of coordinates that has <see cref="Array.Length"/> not equal to the <see cref="Array.Rank"/> of the table.
    /// </summary>
    public class InvalidCoordinateLengthException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="InvalidCoordinateLengthException"/> with no message.
        /// </summary>
        public InvalidCoordinateLengthException() : base()
        { }

        /// <summary>
        /// Creates a new instance of <see cref="InvalidCoordinateLengthException"/> with the specified message.
        /// </summary>
        /// <param name="message">The message to be included.</param>
        public InvalidCoordinateLengthException(string message) : base(message)
        { }
    }
}
