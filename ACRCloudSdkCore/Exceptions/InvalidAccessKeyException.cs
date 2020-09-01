using System;

namespace ACRCloudSdkCore.Exceptions
{
    /// <summary>
    /// The <see cref="InvalidAccessKeyException"/> is thrown when a <see cref="ACRCloudOptions.AccessKey"/> is invalid.
    /// </summary>
    public class InvalidAccessKeyException : Exception
    {
        private const string DefaultMessage = "Invalid AccessKey.";

        /// <summary>
        /// Gets the value of the <see cref="ACRCloudOptions.AccessKey"/> that caused the exception.
        /// </summary>
        public string? AccessKey { get; }

        public InvalidAccessKeyException() : this(null, DefaultMessage)
        {

        }

        public InvalidAccessKeyException(string? accessKey) : this(accessKey, DefaultMessage, null)
        {

        }

        public InvalidAccessKeyException(string? accessKey, string message) : this(accessKey, message, null)
        {

        }

        public InvalidAccessKeyException(string? message, Exception? innerException) : base(message, innerException)
        {

        }

        public InvalidAccessKeyException(string? accessKey, string message, Exception? innerException) : base(message, innerException)
        {
            AccessKey = accessKey;
        }
    }
}
