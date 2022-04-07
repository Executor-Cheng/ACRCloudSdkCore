using System;
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
namespace ACRCloudSdkCore
{
    public static unsafe partial class ACRCloudExtractTools
    {
        /// <inheritdoc cref="DecodeAudio(string, TimeSpan)"/>
        public static byte[] DecodeAudio(string filePath)
        {
            return DecodeAudio(filePath, default);
        }

        /// <inheritdoc cref="DecodeAudio(string, TimeSpan, TimeSpan)"/>
        public static byte[] DecodeAudio(string filePath, TimeSpan startTime)
        {
            return DecodeAudio(filePath, startTime, TimeSpan.FromSeconds(DefaultDurationSeconds));
        }

        /// <summary>
        /// Decode audio from file path of most audio/video file.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="InvalidDataException"/>
        /// <param name="filePath">The file path</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file</param>
        /// <param name="duration">Specify <paramref name="duration"/> of audio data you need</param>
        /// <returns>A <see cref="byte"/>[] that represents audio data(formatter:RIFF (little-endian) data, WAVE audio, Microsoft PCM, 16 bit, mono 8000 Hz)</returns>
        public static byte[] DecodeAudio(string filePath, TimeSpan startTime, TimeSpan duration)
        {
            CheckFilePath(filePath);
            int fpBufferLength = NativeMethods.DecodeAudio(filePath, (int)startTime.TotalSeconds, (int)duration.TotalSeconds, out byte* fpBufferPtr);
            return CreateBufferAndFreePtr(fpBufferPtr, fpBufferLength);
        }

        /// <inheritdoc cref="DecodeAudio(byte[], TimeSpan)"/>
        public static byte[] DecodeAudio(byte[] fileBuffer)
        {
            return DecodeAudio(fileBuffer, default);
        }

        /// <inheritdoc cref="DecodeAudio(byte[], TimeSpan, TimeSpan)"/>
        public static byte[] DecodeAudio(byte[] fileBuffer, TimeSpan startTime)
        {
            return DecodeAudio(fileBuffer, startTime, TimeSpan.FromSeconds(DefaultDurationSeconds));
        }

        /// <param name="fileBuffer">A <see cref="byte"/>[] of input file</param>
        /// <inheritdoc cref="DecodeAudio(Span{byte}, TimeSpan, TimeSpan)"/>
        public static byte[] DecodeAudio(byte[] fileBuffer, TimeSpan startTime, TimeSpan duration)
        {
            return DecodeAudio(fileBuffer.AsSpan(), startTime, duration);
        }

        /// <inheritdoc cref="DecodeAudio(Span{byte}, TimeSpan)"/>
        public static byte[] DecodeAudio(Span<byte> fileSpan)
        {
            return DecodeAudio(fileSpan, default);
        }

        /// <inheritdoc cref="DecodeAudio(Span{byte}, TimeSpan, TimeSpan)"/>
        public static byte[] DecodeAudio(Span<byte> fileSpan, TimeSpan startTime)
        {
            return DecodeAudio(fileSpan, startTime, TimeSpan.FromSeconds(DefaultDurationSeconds));
        }

        /// <summary>
        /// Decode audio from file path of most audio/video file.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="InvalidDataException"/>
        /// <param name="fileSpan">A <see cref="Span{T}"/> of input file</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file</param>
        /// <param name="duration">Specify <paramref name="duration"/> of audio data you need.</param>
        /// <returns>A <see cref="byte"/>[] that represents audio data(formatter:RIFF (little-endian) data, WAVE audio, Microsoft PCM, 16 bit, mono 8000 Hz)</returns>
        public static byte[] DecodeAudio(Span<byte> fileSpan, TimeSpan startTime, TimeSpan duration)
        {
            CheckBuffer(fileSpan);
            int fpBufferLength = NativeMethods.DecodeAudio(ref MemoryMarshal.GetReference(fileSpan), fileSpan.Length, (int)startTime.TotalSeconds, (int)duration.TotalSeconds, out byte* fpBufferPtr);
            return CreateBufferAndFreePtr(fpBufferPtr, fpBufferLength);
        }
    }
}
