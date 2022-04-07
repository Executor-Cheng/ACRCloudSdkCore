using System;
using System.IO;

namespace ACRCloudSdkCore
{
    public static unsafe partial class ACRCloudExtractTools
    {
        /// <summary>
        /// Get duration from file buffer of most audio/video file.
        /// </summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="InvalidDataException"/>
        /// <param name="filePath">The file path</param>
        /// <returns>A <see cref="TimeSpan"/> that represents duration</returns>
        public static TimeSpan GetDuration(string filePath)
        {
            CheckFilePath(filePath);
            int ms = NativeMethods.GetDuration(filePath);
            if (ms <= 0)
            {
                throw new InvalidDataException();
            }
            return TimeSpan.FromMilliseconds(ms);
        }
    }
}
