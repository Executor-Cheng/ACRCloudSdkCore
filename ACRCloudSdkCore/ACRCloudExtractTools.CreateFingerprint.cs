using System;
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
namespace ACRCloudSdkCore
{
    public static unsafe partial class ACRCloudExtractTools
    {
        /// <inheritdoc cref="CreateFingerprint(byte[], bool)"/>
        public static byte[] CreateFingerprint(byte[] pcmBuffer)
        {
            return CreateFingerprint(pcmBuffer, false);
        }

        /// <inheritdoc cref="CreateFingerprint(byte[], bool, int, int, float)"/>
        public static byte[] CreateFingerprint(byte[] pcmBuffer, bool isDB)
        {
            return CreateFingerprint(pcmBuffer, isDB, DefaultMinFilterEnergy, DefaultSilenceEnergyThreshold, DefaultSilenceRateThreshold);
        }

        /// <param name="pcmBuffer">A <see cref="byte"/>[] of wav audio</param>
        /// <inheritdoc cref="CreateFingerprint(Span{byte}, bool, int, int, float)"/>
        public static byte[] CreateFingerprint(byte[] pcmBuffer, bool isDB,
                                               int minFilterEnergy = DefaultMinFilterEnergy,
                                               int silenceEnergyThreshold = DefaultSilenceEnergyThreshold,
                                               float silenceRateThreshold = DefaultSilenceRateThreshold)
        {
            return CreateFingerprint(pcmBuffer.AsSpan(), isDB, minFilterEnergy, silenceEnergyThreshold, silenceRateThreshold);
        }

        /// <inheritdoc cref="CreateFingerprint(Span{byte}, bool)"/>
        public static byte[] CreateFingerprint(Span<byte> pcmSpan)
        {
            return CreateFingerprint(pcmSpan, false);
        }

        /// <inheritdoc cref="CreateFingerprint(Span{byte}, bool, int, int, float)"/>
        public static byte[] CreateFingerprint(Span<byte> pcmSpan, bool isDB)
        {
            return CreateFingerprint(pcmSpan, isDB, DefaultMinFilterEnergy, DefaultSilenceEnergyThreshold, DefaultSilenceRateThreshold);
        }

        /// <summary>
        /// Create ACRCloud Fingerprint by wav audio buffer(RIFF (little-endian) data, WAVE audio, Microsoft PCM, 16 bit, mono 8000 Hz)
        /// </summary>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="InvalidDataException"/>
        /// <param name="pcmSpan">A <see cref="Span{T}"/> of wav audio</param>
        /// <param name="isDB"><see langword="true"/> to create db frigerprint</param>
        /// <param name="minFilterEnergy"></param>
        /// <param name="silenceEnergyThreshold"></param>
        /// <param name="silenceRateThreshold"></param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Fingerprint</returns>
        public static byte[] CreateFingerprint(Span<byte> pcmSpan, bool isDB,
                                               int minFilterEnergy = DefaultMinFilterEnergy,
                                               int silenceEnergyThreshold = DefaultSilenceEnergyThreshold,
                                               float silenceRateThreshold = DefaultSilenceRateThreshold)
        {
            CheckBuffer(pcmSpan);
            int fpBufferLength = NativeMethods.CreateFingerprint(ref MemoryMarshal.GetReference(pcmSpan), pcmSpan.Length, isDB, minFilterEnergy, silenceEnergyThreshold, silenceRateThreshold, out byte* fpBufferPtr);
            return CreateBufferAndFreePtr(fpBufferPtr, fpBufferLength);
        }
    }
}
