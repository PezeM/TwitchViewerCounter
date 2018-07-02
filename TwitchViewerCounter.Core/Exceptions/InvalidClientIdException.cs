using System;

namespace TwitchViewerCounter.Core.Exceptions
{
    public class InvalidClientIdException : Exception
    {
        public InvalidClientIdException()
        {
        }

        public InvalidClientIdException(string message) : base(message)
        {
        }

        public InvalidClientIdException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
