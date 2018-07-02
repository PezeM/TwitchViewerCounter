using System;

namespace TwitchViewerCounter.Core.Exceptions
{
    public class ClientIdNotSetException : Exception
    {
        public ClientIdNotSetException()
        {
        }

        public ClientIdNotSetException(string message) : base(message)
        {
        }

        public ClientIdNotSetException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
