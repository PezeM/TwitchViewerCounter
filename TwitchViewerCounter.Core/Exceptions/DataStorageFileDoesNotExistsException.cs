using System;

namespace TwitchViewerCounter.Core.Exceptions
{
    public class DataStorageFileDoesNotExistsException : Exception
    {
        public DataStorageFileDoesNotExistsException()
        {
        }

        public DataStorageFileDoesNotExistsException(string message) : base(message)
        {
        }

        public DataStorageFileDoesNotExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
