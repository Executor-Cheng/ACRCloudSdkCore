﻿using System;

namespace ACRCloudSdkCore.Exceptions
{
    /// <summary>
    /// The <see cref="UnknownResponseException"/> is thrown when the server responded unexpectedly.
    /// </summary>
    public sealed class UnknownResponseException : Exception
    {
        /// <summary>
        /// Gets the server responded message.
        /// </summary>
        public string? Response { get; }

        public UnknownResponseException() { }

        public UnknownResponseException(string? response) : this(response, "Unknown response.", null) { }

        public UnknownResponseException(string? response, string message) : this(response, message, null) { }

        public UnknownResponseException(string? response, Exception? innerException) : this(response, "Unknown response.", innerException) { }

        public UnknownResponseException(string? response, string message, Exception? innerException) : base(message, innerException)
            => Response = response;
    }
}
