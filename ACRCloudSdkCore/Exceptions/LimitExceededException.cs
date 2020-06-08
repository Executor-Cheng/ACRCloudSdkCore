using System;

namespace ACRCloudSdkCore.Exceptions
{
    /// <summary>
    /// The <see cref="LimitExceededException"/> is thrown when your account limit is exceeded.
    /// </summary>
    public class LimitExceededException : Exception
    {
        public LimitExceededException() : this("Limit exceeded.")
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
