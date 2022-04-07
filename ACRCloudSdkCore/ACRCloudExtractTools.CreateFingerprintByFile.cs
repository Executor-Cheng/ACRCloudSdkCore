using System;
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
namespace ACRCloudSdkCore
{
    public static unsafe partial class ACRCloudExtractTools
    {
        /// <inheritdoc cref="CreateFingerprintByFile(string, TimeSpan)"/>
        public static byte[] CreateFingerprintByFile(string filePath)
        {
            return CreateFingerprintByFile(filePath, default);
        }

        /// <inheritdoc cref="CreateFingerprintByFile(string, TimeSpan, TimeSpan)"/>
        public static byte[] CreateFingerprintByFile(string filePath, TimeSpan startTime)
        {
            return CreateFingerprintByFile(filePath, startTime, TimeSpan.FromSeconds(DefaultDurationSeconds));
        }

        /// <inheritdoc cref="CreateFingerprintByFile(string, TimeSpan, TimeSpan, bool)"/>
        public static byte[] CreateFingerprintByFile(string filePath, TimeSpan startTime, TimeSpan duration)
        {
            return CreateFingerprintByFile(filePath, startTime, duration, false);
        }

        /// <inheritdoc cref="CreateFingerprintByFile(string, TimeSpan, TimeSpan, bool, int, int, float)"/>
        public static byte[] CreateFingerprintByFile(string filePath, TimeSpan startTime, TimeSpan duration, bool isDB)
        {
            return CreateFingerprintByFile(filePath, startTime, duration, isDB, DefaultMinFilterEnergy, DefaultSilenceEnergyThreshold, DefaultSilenceRateThreshold);
        }

        /// <summary>
        /// Create ACRCloud Fingerprint by file path of most audio/video file.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="InvalidDataException"/>
        /// <param name="filePath">The file path.</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file.</param>
        /// <param name="duration">Specify <paramref name="duration"/> of audio data you need. If you create recogize frigerprint, defaults to 12 seconds, if you create db frigerprint, it is ignored.</param>
        /// <param name="isDB"><see langword="true"/> to create db frigerprint.</param>
        /// <param name="minFilterEnergy">The minimum of filter energy.</param>
        /// <param name="silenceEnergyThreshold">The silence energy threshold.</param>
        /// <param name="silenceRateThreshold">The audio type.</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Fingerprint.</returns>
        public static byte[] CreateFingerprintByFile(string filePath, TimeSpan startTime, TimeSpan duration, bool isDB,
                                                     int minFilterEnergy = DefaultMinFilterEnergy,
                                                     int silenceEnergyThreshold = DefaultSilenceEnergyThreshold,
                                                     float silenceRateThreshold = DefaultSilenceRateThreshold)
        {
            CheckFilePath(filePath);
            int fpBufferLength = NativeMethods.CreateFingerprint(filePath, (int)startTime.TotalSeconds, (int)duration.TotalSeconds, isDB, minFilterEnergy, silenceEnergyThreshold, silenceRateThreshold, out byte* fpBufferPtr);
            return CreateBufferAndFreePtr(fpBufferPtr, fpBufferLength);
        }

        /// <inheritdoc cref="CreateFingerprintByFile(byte[], TimeSpan)"/>
        public static byte[] CreateFingerprintByFile(byte[] fileBuffer)
        {
            return CreateFingerprintByFile(fileBuffer, default);
        }

        /// <inheritdoc cref="CreateFingerprintByFile(byte[], TimeSpan, TimeSpan)"/>
        public static byte[] CreateFingerprintByFile(byte[] fileBuffer, TimeSpan startTime)
        {
            return CreateFingerprintByFile(fileBuffer, startTime, TimeSpan.FromSeconds(DefaultDurationSeconds));
        }

        /// <inheritdoc cref="CreateFingerprintByFile(byte[], TimeSpan, TimeSpan, bool)"/>
        public static byte[] CreateFingerprintByFile(byte[] fileBuffer, TimeSpan startTime, TimeSpan duration)
        {
            return CreateFingerprintByFile(fileBuffer, startTime, duration, false);
        }

        /// <inheritdoc cref="CreateFingerprintByFile(byte[], TimeSpan, TimeSpan, bool, int, int, float)"/>
        public static byte[] CreateFingerprintByFile(byte[] fileBuffer, TimeSpan startTime, TimeSpan duration, bool isDB)
        {
            return CreateFingerprintByFile(fileBuffer, startTime, duration, isDB, DefaultMinFilterEnergy, DefaultSilenceEnergyThreshold, DefaultSilenceRateThreshold);
        }

        /// <param name="fileBuffer">A <see cref="byte"/>[] of input file</param>
        /// <inheritdoc cref="CreateFingerprintByFile(Span{byte}, TimeSpan, TimeSpan, bool, int, int, float)"/>
        public static byte[] CreateFingerprintByFile(byte[] fileBuffer, TimeSpan startTime, TimeSpan duration, bool isDB = false,
                                                     int minFilterEnergy = DefaultMinFilterEnergy,
                                                     int silenceEnergyThreshold = DefaultSilenceEnergyThreshold,
                                                     float silenceRateThreshold = DefaultSilenceRateThreshold)
        {
            return CreateFingerprintByFile(fileBuffer.AsSpan(), startTime, duration, isDB, minFilterEnergy, silenceEnergyThreshold, silenceRateThreshold);
        }

        /// <inheritdoc cref="CreateFingerprintByFile(Span{byte}, TimeSpan)"/>
        public static byte[] CreateFingerprintByFile(Span<byte> fileSpan)
        {
            return CreateFingerprintByFile(fileSpan, default);
        }

        /// <inheritdoc cref="CreateFingerprintByFile(Span{byte}, TimeSpan, TimeSpan)"/>
        public static byte[] CreateFingerprintByFile(Span<byte> fileSpan, TimeSpan startTime)
        {
            return CreateFingerprintByFile(fileSpan, startTime, TimeSpan.FromSeconds(DefaultDurationSeconds));
        }

        /// <inheritdoc cref="CreateFingerprintByFile(Span{byte}, TimeSpan, TimeSpan, bool)"/>
        public static byte[] CreateFingerprintByFile(Span<byte> fileSpan, TimeSpan startTime, TimeSpan duration)
        {
            return CreateFingerprintByFile(fileSpan, startTime, duration, false);
        }

        /// <inheritdoc cref="CreateFingerprintByFile(Span{byte}, TimeSpan, TimeSpan, bool, int, int, float)"/>
        public static byte[] CreateFingerprintByFile(Span<byte> fileSpan, TimeSpan startTime, TimeSpan duration, bool isDB)
        {
            return CreateFingerprintByFile(fileSpan, startTime, duration, isDB, DefaultMinFilterEnergy, DefaultSilenceEnergyThreshold, DefaultSilenceRateThreshold);
        }

        /// <summary>
        /// Create ACRCloud Fingerprint by file buffer of most audio/video file.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="InvalidDataException"/>
        /// <param name="fileSpan">A <see cref="Span{T}"/> of input file</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file</param>
        /// <param name="duration">Specify <paramref name="duration"/> of audio data you need. If you create recogize frigerprint, defaults to 12 seconds, if you create db frigerprint, it is ignored</param>
        /// <param name="isDB"><see langword="true"/> to create db frigerprint</param>
        /// <param name="minFilterEnergy">The minimum of filter energy.</param>
        /// <param name="silenceEnergyThreshold">The silence energy threshold.</param>
        /// <param name="silenceRateThreshold">The audio type.</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Fingerprint</returns>
        public static byte[] CreateFingerprintByFile(Span<byte> fileSpan, TimeSpan startTime, TimeSpan duration, bool isDB = false,
                                                     int minFilterEnergy = DefaultMinFilterEnergy,
                                                     int silenceEnergyThreshold = DefaultSilenceEnergyThreshold,
                                                     float silenceRateThreshold = DefaultSilenceRateThreshold)
        {
            CheckBuffer(fileSpan);
            int fpBufferLength = NativeMethods.CreateFingerprint(ref MemoryMarshal.GetReference(fileSpan), fileSpan.Length, (int)startTime.TotalSeconds, (int)duration.TotalSeconds, isDB, minFilterEnergy, silenceEnergyThreshold, silenceRateThreshold, out byte* fpBufferPtr);
            return CreateBufferAndFreePtr(fpBufferPtr, fpBufferLength);
        }
    }
}
