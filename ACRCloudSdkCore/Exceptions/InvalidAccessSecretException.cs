using System;

namespace ACRCloudSdkCore.Exceptions
{
    /// <summary>
    /// The <see cref="InvalidAccessSecretException"/> is thrown when a <see cref="ACRCloudOptions.AccessSecret"/> is invalid.
    /// </summary>
    public class InvalidAccessSecretException : Exception
    {
        /// <summary>
        /// Gets the value of the <see cref="ACRCloudOptions.AccessSecret"/> that caused the exception.
        /// </summary>
        public string AccessSecret { get; }

        public InvalidAccessSecretException() : this(null, "Invalid accessSecret.")
        {

        }

        public InvalidAccessSecretException(string accessSecret) : this(accessSecret, "Invalid accessSecret.", null)
        {

        }

        public InvalidAccessSecretException(string accessSecret, string message) : this(accessSecret, message, null)
        {

        }

        public InvalidAccessSecretException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public InvalidAccessSecretException(string accessSecret, string message, Exception innerException) : base(message, innerException)
        {
            AccessSecret = accessSecret;
        }
    }
}
