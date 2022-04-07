using System;
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
namespace ACRCloudSdkCore
{
    public static unsafe partial class ACRCloudExtractTools
    {
        /// <inheritdoc cref="CreateHummingFingerprintByFile(string, TimeSpan)"/>
        public static byte[] CreateHummingFingerprintByFile(string filePath)
        {
            return CreateHummingFingerprintByFile(filePath, default);
        }

        /// <inheritdoc cref="CreateHummingFingerprintByFile(string, TimeSpan, TimeSpan)"/>
        public static byte[] CreateHummingFingerprintByFile(string filePath, TimeSpan startTime)
        {
            return CreateHummingFingerprintByFile(filePath, startTime, TimeSpan.FromSeconds(DefaultDurationSeconds));
        }

        /// <summary>
        /// Create ACRCloud Humming Fingerprint by file path of most audio/video file.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="InvalidDataException"/>
        /// <param name="filePath">The file path</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file</param>
        /// <param name="duration">Specify <paramref name="duration"/> of audio data you need.</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Humming Fingerprint</returns>
        public static byte[] CreateHummingFingerprintByFile(string filePath, TimeSpan startTime, TimeSpan duration)
        {
            CheckFilePath(filePath);
            int fpBufferLength = NativeMethods.CreateHummingFingerprint(filePath, (int)startTime.TotalSeconds, (int)duration.TotalSeconds, out byte* fpBufferPtr);
            return CreateBufferAndFreePtr(fpBufferPtr, fpBufferLength);
        }

        /// <inheritdoc cref="CreateHummingFingerprintByFile(byte[], TimeSpan)"/>
        public static byte[] CreateHummingFingerprintByFile(byte[] fileBuffer)
        {
            return CreateHummingFingerprintByFile(fileBuffer, default);
        }

        /// <inheritdoc cref="CreateHummingFingerprintByFile(byte[], TimeSpan, TimeSpan)"/>
        public static byte[] CreateHummingFingerprintByFile(byte[] fileBuffer, TimeSpan startTime)
        {
            return CreateHummingFingerprintByFile(fileBuffer, startTime, TimeSpan.FromSeconds(DefaultDurationSeconds));
        }

        /// <param name="fileBuffer">A <see cref="byte"/>[] of input file</param>
        /// <inheritdoc cref="CreateHummingFingerprintByFile(Span{byte}, TimeSpan, TimeSpan)"/>
        public static byte[] CreateHummingFingerprintByFile(byte[] fileBuffer, TimeSpan startTime, TimeSpan duration)
        {
            return CreateHummingFingerprintByFile(fileBuffer.AsSpan(), startTime, duration);
        }

        /// <inheritdoc cref="CreateHummingFingerprintByFile(Span{byte}, TimeSpan)"/>
        public static byte[] CreateHummingFingerprintByFile(Span<byte> fileSpan)
        {
            return CreateHummingFingerprintByFile(fileSpan, default);
        }

        /// <inheritdoc cref="CreateHummingFingerprintByFile(Span{byte}, TimeSpan, TimeSpan)"/>
        public static byte[] CreateHummingFingerprintByFile(Span<byte> fileSpan, TimeSpan startTime)
        {
            return CreateHummingFingerprintByFile(fileSpan, startTime, TimeSpan.FromSeconds(DefaultDurationSeconds));
        }

        /// <summary>
        /// Create ACRCloud Humming Fingerprint by file buffer of most audio/video file.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="InvalidDataException"/>
        /// <param name="fileSpan">A <see cref="Span{T}"/> of input file</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file</param>
        /// <param name="duration">Specify <paramref name="duration"/> of audio data you need.</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Humming Fingerprint</returns>
        public static byte[] CreateHummingFingerprintByFile(Span<byte> fileSpan, TimeSpan startTime, TimeSpan duration)
        {
            CheckBuffer(fileSpan);
            int fpBufferLength = NativeMethods.CreateHummingFingerprint(ref MemoryMarshal.GetReference(fileSpan), fileSpan.Length, (int)startTime.TotalSeconds, (int)duration.TotalSeconds, out byte* fpBufferPtr);
            return CreateBufferAndFreePtr(fpBufferPtr, fpBufferLength);
        }
    }
}
