using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ACRCloudSdkCore
{
    public static unsafe partial class ACRCloudExtractTools
    {
        /// <param name="pcmBuffer">A <see cref="byte"/>[] of wav audio</param>
        /// <inheritdoc cref="CreateHummingFingerprint(Span{byte})"/>
        public static byte[] CreateHummingFingerprint(byte[] pcmBuffer)
        {
            return CreateHummingFingerprint(pcmBuffer.AsSpan());
        }

        /// <summary>
        /// Create ACRCloud Humming Fingerprint by wav audio buffer(RIFF (little-endian) data, WAVE audio, Microsoft PCM, 16 bit, mono 8000 Hz)
        /// </summary>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="InvalidDataException"/>
        /// <param name="pcmSpan">A <see cref="Span{T}"/> of wav audio</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Humming Fingerprint</returns>
        public static byte[] CreateHummingFingerprint(Span<byte> pcmSpan)
        {
            CheckBuffer(pcmSpan);
            int fpBufferLength = NativeMethods.CreateHummingFingerprint(ref MemoryMarshal.GetReference(pcmSpan), pcmSpan.Length, out byte* fpBufferPtr);
            return CreateBufferAndFreePtr(fpBufferPtr, fpBufferLength);
        }
    }
}
