using ACRCloudSdkCore.Exceptions;
using System;

namespace ACRCloudSdkCore
{
    /// <summary>
    /// Represents an ACRCloud project's configuration
    /// </summary>
    public class ACRCloudOptions
    {
        /// <summary>
        /// The host to access ACRCloud api
        /// </summary>
        public string Host { get; }
        /// <summary>
        /// The access key in your project
        /// </summary>
        public string AccessKey { get; }
        /// <summary>
        /// The access secret in your project
        /// </summary>
        public string AccessSecret { get; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ACRCloudOptions"/> <see langword="class"/> for the specified <paramref name="host"/>, <paramref name="accessKey"/> and <paramref name="accessSecret"/>
        /// </summary>
        /// <param name="host">Your project's host</param>
        /// <param name="accessKey">Your project's access key</param>
        /// <param name="accessSecret">Your project's access secret</param>
        public ACRCloudOptions(string host, string accessKey, string accessSecret)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentException("Value for host must not be null or empty.");
            }
            if (string.IsNullOrEmpty(accessKey))
            {
                throw new InvalidAccessKeyException(accessKey, "Value for accessKey must not be null or empty.");
            }
            if (accessKey.Length != 32)
            {
                throw new InvalidAccessKeyException(accessKey, "The length of accessKey should be 32.");
            }
            if (string.IsNullOrEmpty(accessSecret))
            {
                throw new InvalidAccessSecretException(accessSecret, "Value for accessSecret must not be null or empty.");
            }
            if (accessSecret.Length != 40)
            {
                throw new InvalidAccessSecretException(accessSecret, "The length of accessSecret should be 40.");
            }
            Host = host;
            AccessKey = accessKey;
            AccessSecret = accessSecret;
        }
    }
}
