using System;

namespace ACRCloudSdkCore.Exceptions
{
    /// <summary>
    /// The <see cref="LimitExceededException"/> is thrown when your account limit is exceeded.
    /// </summary>
    public class LimitExceededException : Exception
    {
        private const string DefaultMessage = "Limit exceeded.";

        public LimitExceededException() : this(DefaultMessage)
        {

        }

        public LimitExceededException(string message) : base(message)
        {

        }

        public LimitExceededException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
