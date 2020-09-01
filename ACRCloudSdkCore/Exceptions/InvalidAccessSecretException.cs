using System;

namespace ACRCloudSdkCore.Exceptions
{
    /// <summary>
    /// The <see cref="InvalidAccessSecretException"/> is thrown when a <see cref="ACRCloudOptions.AccessSecret"/> is invalid.
    /// </summary>
    public class InvalidAccessSecretException : Exception
    {
        private const string DefaultMessage = "Invalid accessSecret.";

        /// <summary>
        /// Gets the value of the <see cref="ACRCloudOptions.AccessSecret"/> that caused the exception.
        /// </summary>
        public string? AccessSecret { get; }

        public InvalidAccessSecretException() : this(null, DefaultMessage)
        {

        }

        public InvalidAccessSecretException(string? accessSecret) : this(accessSecret, DefaultMessage, null)
        {

        }

        public InvalidAccessSecretException(string? accessSecret, string message) : this(accessSecret, message, null)
        {

        }

        public InvalidAccessSecretException(string? message, Exception? innerException) : base(message, innerException)
        {

        }

        public InvalidAccessSecretException(string? accessSecret, string message, Exception? innerException) : base(message, innerException)
        {
            AccessSecret = accessSecret;
        }
    }
}
