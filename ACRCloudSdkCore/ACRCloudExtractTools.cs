using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace ACRCloudSdkCore
{
    /// <summary>
    /// This class contains methods that interop with libacrcloud_extr_tool.
    /// </summary>
    public static unsafe partial class ACRCloudExtractTools
    {
        public const int DefaultMinFilterEnergy = 50;

        public const int DefaultSilenceEnergyThreshold = 50;

        public const float DefaultSilenceRateThreshold = 0.99f;

        public const int DefaultDurationSeconds = 12;

        static ACRCloudExtractTools()
        {
#if NETFRAMEWORK
            string path = IntPtr.Size == 4 ? $@"x86\{NativeMethods.ExtractToolName}.dll" : $@"x64\{NativeMethods.ExtractToolName}.dll";
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Unable to find native asset.", path);
            }
            if (NativeMethods.Win32.LoadLibrary(path) == null)
            {
                throw new BadImageFormatException("Format of the executable (.exe) or library (.dll) is invalid.");
            }
#endif
            NativeMethods.Init();
        }

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

        /// <summary>
        /// Create ACRCloud Fingerprint by wav audio buffer(RIFF (little-endian) data, WAVE audio, Microsoft PCM, 16 bit, mono 8000 Hz)
        /// </summary>
        /// <param name="pcmBuffer">A <see cref="byte"/>[] of wav audio</param>
        /// <param name="isDB"><see langword="true"/> to create db frigerprint</param>
        /// <param name="minFilterEnergy"></param>
        /// <param name="silenceEnergyThreshold"></param>
        /// <param name="silenceRateThreshold"></param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Fingerprint</returns>
        public static byte[] CreateFingerprint(byte[] pcmBuffer, bool isDB,
                                               int minFilterEnergy = DefaultMinFilterEnergy,
                                               int silenceEnergyThreshold = DefaultSilenceEnergyThreshold,
                                               float silenceRateThreshold = DefaultSilenceRateThreshold)
        {
            CheckBuffer(pcmBuffer);
            int fpBufferLength = NativeMethods.CreateFingerprint(pcmBuffer, pcmBuffer.Length, isDB, minFilterEnergy, silenceEnergyThreshold, silenceRateThreshold, out byte* fpBufferPtr);
            return CreateBufferAndFreePtr(fpBufferPtr, fpBufferLength);
        }

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

        /// <summary>
        /// Create ACRCloud Fingerprint by file buffer of most audio/video file.
        /// </summary>
        /// <param name="fileBuffer">A <see cref="byte"/>[] of input file</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file</param>
        /// <param name="duration">Specify <paramref name="duration"/> of audio data you need. If you create recogize frigerprint, defaults to 12 seconds, if you create db frigerprint, it is ignored</param>
        /// <param name="isDB"><see langword="true"/> to create db frigerprint</param>
        /// <param name="minFilterEnergy">The minimum of filter energy.</param>
        /// <param name="silenceEnergyThreshold">The silence energy threshold.</param>
        /// <param name="silenceRateThreshold">The audio type.</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Fingerprint</returns>
        public static byte[] CreateFingerprintByFile(byte[] fileBuffer, TimeSpan startTime, TimeSpan duration, bool isDB = false,
                                                     int minFilterEnergy = DefaultMinFilterEnergy,
                                                     int silenceEnergyThreshold = DefaultSilenceEnergyThreshold,
                                                     float silenceRateThreshold = DefaultSilenceRateThreshold)
        {
            CheckBuffer(fileBuffer);
            int fpBufferLength = NativeMethods.CreateFingerprint(fileBuffer, fileBuffer.Length, (int)startTime.TotalSeconds, (int)duration.TotalSeconds, isDB, minFilterEnergy, silenceEnergyThreshold, silenceRateThreshold, out byte* fpBufferPtr);
            return CreateBufferAndFreePtr(fpBufferPtr, fpBufferLength);
        }

        /// <summary>
        /// Create ACRCloud Humming Fingerprint by wav audio buffer(RIFF (little-endian) data, WAVE audio, Microsoft PCM, 16 bit, mono 8000 Hz)
        /// </summary>
        /// <param name="pcmBuffer">A <see cref="byte"/>[] of wav audio</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Humming Fingerprint</returns>
        public static byte[] CreateHummingFingerprint(byte[] pcmBuffer)
        {
            CheckBuffer(pcmBuffer);
            int fpBufferLength = NativeMethods.CreateHummingFingerprint(pcmBuffer, pcmBuffer.Length, out byte* fpBufferPtr);
            return CreateBufferAndFreePtr(fpBufferPtr, fpBufferLength);
        }

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

        /// <summary>
        /// Create ACRCloud Humming Fingerprint by file buffer of most audio/video file.
        /// </summary>
        /// <param name="fileBuffer">A <see cref="byte"/>[] of input file</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file</param>
        /// <param name="duration">Specify <paramref name="duration"/> of audio data you need.</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Humming Fingerprint</returns>
        public static byte[] CreateHummingFingerprintByFile(byte[] fileBuffer, TimeSpan startTime, TimeSpan duration)
        {
            CheckBuffer(fileBuffer);
            int fpBufferLength = NativeMethods.CreateHummingFingerprint(fileBuffer, fileBuffer.Length, (int)startTime.TotalSeconds, (int)duration.TotalSeconds, out byte* fpBufferPtr);
            return CreateBufferAndFreePtr(fpBufferPtr, fpBufferLength);
        }

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

        /// <summary>
        /// Decode audio from file path of most audio/video file.
        /// </summary>
        /// <param name="fileBuffer">A <see cref="byte"/>[] of input file</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file</param>
        /// <param name="duration">Specify <paramref name="duration"/> of audio data you need.</param>
        /// <returns>A <see cref="byte"/>[] that represents audio data(formatter:RIFF (little-endian) data, WAVE audio, Microsoft PCM, 16 bit, mono 8000 Hz)</returns>
        public static byte[] DecodeAudio(byte[] fileBuffer, TimeSpan startTime, TimeSpan duration)
        {
            CheckBuffer(fileBuffer);
            int fpBufferLength = NativeMethods.DecodeAudio(fileBuffer, fileBuffer.Length, (int)startTime.TotalSeconds, (int)duration.TotalSeconds, out byte* fpBufferPtr);
            return CreateBufferAndFreePtr(fpBufferPtr, fpBufferLength);
        }

        /// <summary>
        /// Get duration from file buffer of most audio/video file.
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <returns>A <see cref="TimeSpan"/> that represents duration</returns>
        public static TimeSpan GetDuration(string filePath)
        {
            CheckFilePath(filePath);
            int ms = NativeMethods.GetDuration(filePath);
            if (ms <= 0)
            {
                throw new NotImplementedException();
            }
            return TimeSpan.FromMilliseconds(ms);
        }

        /// <summary>
        /// Set whether the debug output is printed.
        /// </summary>
        /// <param name="isDebug"><see langword="true"/> to display debug output</param>
        public static void SetDebug(bool isDebug)
        {
            NativeMethods.SetDebug(isDebug);
        }

        private static byte[] CreateBufferAndFreePtr(byte* fpBufferPtr, int fpBufferLength)
        {
            if (fpBufferLength <= 0)
            {
                throw new InvalidDataException();
            }
            byte[] fpBuffer = new byte[fpBufferLength];
#if NETSTANDARD2_0 || NETFRAMEWORK
            fixed (byte* localFpBufferPtr = fpBuffer)
            {
                Buffer.MemoryCopy(fpBufferPtr, localFpBufferPtr, fpBufferLength, fpBufferLength);
            }
#else
            Unsafe.CopyBlock(ref fpBuffer[0], ref *fpBufferPtr, (uint)fpBufferLength);
#endif
            NativeMethods.Free(fpBufferPtr);
            return fpBuffer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CheckBuffer(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                throw new ArgumentException("Invalid audio data.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CheckFilePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path must not be null or empty.");
            }
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Could not find file '{filePath}'.", filePath);
            }
        }
    }
}
