using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1401 // P/Invokes should not be visible
#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments
namespace ACRCloudSdkCore
{
    /// <summary>
    /// This class contains methods that interop with libacrcloud_extr_tool.
    /// </summary>
    public static unsafe class ACRCloudExtractTools
    {
        public const int DefaultMinFilterEnergy = 50;

        public const int DefaultSilenceEnergyThreshold = 100;

        public const float DefaultSilenceRateThreshold = 0.99f;

        public const int DefaultDurationSeconds = 12;

        static ACRCloudExtractTools()
        {
            NativeMethods.Init();
        }

        /// <summary>
        /// Create ACRCloud Fingerprint by wav audio buffer(RIFF (little-endian) data, WAVE audio, Microsoft PCM, 16 bit, mono 8000 Hz)
        /// </summary>
        /// <param name="pcmBuffer">A <see cref="byte"/>[] of wav audio</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Fingerprint</returns>
        public static byte[] CreateFingerprint(byte[] pcmBuffer)
        {
            return CreateFingerprint(pcmBuffer, false);
        }
        /// <summary>
        /// Create ACRCloud Fingerprint by wav audio buffer(RIFF (little-endian) data, WAVE audio, Microsoft PCM, 16 bit, mono 8000 Hz)
        /// </summary>
        /// <param name="pcmBuffer">A <see cref="byte"/>[] of wav audio</param>
        /// <param name="isDB"><see langword="true"/> to create db frigerprint</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Fingerprint</returns>
        public static byte[] CreateFingerprint(byte[] pcmBuffer, bool isDB)
        {
            return CreateFingerprint(pcmBuffer, isDB);
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
        /// <summary>
        /// Create ACRCloud Fingerprint by file path of most audio/video file.
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Fingerprint</returns>
        public static byte[] CreateFingerprintByFile(string filePath)
        {
            return CreateFingerprintByFile(filePath, default);
        }
        /// <summary>
        /// Create ACRCloud Fingerprint by file path of most audio/video file.
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Fingerprint</returns>
        public static byte[] CreateFingerprintByFile(string filePath, TimeSpan startTime)
        {
            return CreateFingerprintByFile(filePath, startTime, TimeSpan.FromSeconds(DefaultDurationSeconds));
        }
        /// <summary>
        /// Create ACRCloud Fingerprint by file path of most audio/video file.
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file</param>
        /// <param name="duration">Specify <paramref name="duration"/> of audio data you need. If you create recogize frigerprint, defaults to 12 seconds, if you create db frigerprint, it is ignored</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Fingerprint</returns>
        public static byte[] CreateFingerprintByFile(string filePath, TimeSpan startTime, TimeSpan duration)
        {
            return CreateFingerprintByFile(filePath, startTime, duration, false);
        }
        /// <summary>
        /// Create ACRCloud Fingerprint by file path of most audio/video file.
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file</param>
        /// <param name="duration">Specify <paramref name="duration"/> of audio data you need. If you create recogize frigerprint, defaults to 12 seconds, if you create db frigerprint, it is ignored</param>
        /// <param name="isDB"><see langword="true"/> to create db frigerprint</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Fingerprint</returns>
        public static byte[] CreateFingerprintByFile(string filePath, TimeSpan startTime, TimeSpan duration, bool isDB)
        {
            return CreateFingerprintByFile(filePath, startTime, duration, isDB, DefaultMinFilterEnergy, DefaultSilenceEnergyThreshold, DefaultSilenceRateThreshold);
        }
        /// <summary>
        /// Create ACRCloud Fingerprint by file path of most audio/video file.
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file</param>
        /// <param name="duration">Specify <paramref name="duration"/> of audio data you need. If you create recogize frigerprint, defaults to 12 seconds, if you create db frigerprint, it is ignored</param>
        /// <param name="isDB"><see langword="true"/> to create db frigerprint</param>
        /// <param name="minFilterEnergy"></param>
        /// <param name="silenceEnergyThreshold"></param>
        /// <param name="silenceRateThreshold"></param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Fingerprint</returns>
        public static byte[] CreateFingerprintByFile(string filePath, TimeSpan startTime, TimeSpan duration, bool isDB,
                                                     int minFilterEnergy = DefaultMinFilterEnergy,
                                                     int silenceEnergyThreshold = DefaultSilenceEnergyThreshold,
                                                     float silenceRateThreshold = DefaultSilenceRateThreshold)
        {
            CheckFilePath(filePath);
            int fpBufferLength = NativeMethods.CreateFingerprint(filePath, (int)startTime.TotalSeconds, (int)duration.TotalSeconds, isDB, minFilterEnergy, silenceEnergyThreshold, silenceRateThreshold, out byte* fpBufferPtr);
            return CreateBufferAndFreePtr(fpBufferPtr, fpBufferLength);
        }
        /// <summary>
        /// Create ACRCloud Fingerprint by file buffer of most audio/video file.
        /// </summary>
        /// <param name="fileBuffer">A <see cref="byte"/>[] of input file</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Fingerprint</returns>
        public static byte[] CreateFingerprintByFile(byte[] fileBuffer)
        {
            return CreateFingerprintByFile(fileBuffer, default);
        }
        /// <summary>
        /// Create ACRCloud Fingerprint by file buffer of most audio/video file.
        /// </summary>
        /// <param name="fileBuffer">A <see cref="byte"/>[] of input file</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Fingerprint</returns>
        public static byte[] CreateFingerprintByFile(byte[] fileBuffer, TimeSpan startTime)
        {
            return CreateFingerprintByFile(fileBuffer, startTime, TimeSpan.FromSeconds(DefaultDurationSeconds));
        }
        /// <summary>
        /// Create ACRCloud Fingerprint by file buffer of most audio/video file.
        /// </summary>
        /// <param name="fileBuffer">A <see cref="byte"/>[] of input file</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file</param>
        /// <param name="duration">Specify <paramref name="duration"/> of audio data you need. If you create recogize frigerprint, defaults to 12 seconds, if you create db frigerprint, it is ignored</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Fingerprint</returns>
        public static byte[] CreateFingerprintByFile(byte[] fileBuffer, TimeSpan startTime, TimeSpan duration)
        {
            return CreateFingerprintByFile(fileBuffer, startTime, duration, false);
        }
        /// <summary>
        /// Create ACRCloud Fingerprint by file buffer of most audio/video file.
        /// </summary>
        /// <param name="fileBuffer">A <see cref="byte"/>[] of input file</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file</param>
        /// <param name="duration">Specify <paramref name="duration"/> of audio data you need. If you create recogize frigerprint, defaults to 12 seconds, if you create db frigerprint, it is ignored</param>
        /// <param name="isDB"><see langword="true"/> to create db frigerprint</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Fingerprint</returns>
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
        /// <param name="minFilterEnergy"></param>
        /// <param name="silenceEnergyThreshold"></param>
        /// <param name="silenceRateThreshold"></param>
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
        /// <summary>
        /// Create ACRCloud Humming Fingerprint by file path of most audio/video file.
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Humming Fingerprint</returns>
        public static byte[] CreateHummingFingerprintByFile(string filePath)
        {
            return CreateHummingFingerprintByFile(filePath, default);
        }
        /// <summary>
        /// Create ACRCloud Humming Fingerprint by file path of most audio/video file.
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Humming Fingerprint</returns>
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
        /// <summary>
        /// Create ACRCloud Humming Fingerprint by file buffer of most audio/video file.
        /// </summary>
        /// <param name="fileBuffer">A <see cref="byte"/>[] of input file</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Humming Fingerprint</returns>
        public static byte[] CreateHummingFingerprintByFile(byte[] fileBuffer)
        {
            return CreateHummingFingerprintByFile(fileBuffer, default);
        }
        /// <summary>
        /// Create ACRCloud Humming Fingerprint by file buffer of most audio/video file.
        /// </summary>
        /// <param name="fileBuffer">A <see cref="byte"/>[] of input file</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file</param>
        /// <returns>A <see cref="byte"/>[] that represents ACRCloud Humming Fingerprint</returns>
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
        /// <summary>
        /// Decode audio from file path of most audio/video file.
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <returns>A <see cref="byte"/>[] that represents audio data(formatter:RIFF (little-endian) data, WAVE audio, Microsoft PCM, 16 bit, mono 8000 Hz)</returns>
        public static byte[] DecodeAudio(string filePath)
        {
            return DecodeAudio(filePath, default);
        }
        /// <summary>
        /// Decode audio from file path of most audio/video file.
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file</param>
        /// <returns>A <see cref="byte"/>[] that represents audio data(formatter:RIFF (little-endian) data, WAVE audio, Microsoft PCM, 16 bit, mono 8000 Hz)</returns>
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
        /// <summary>
        /// Decode audio from file path of most audio/video file.
        /// </summary>
        /// <param name="fileBuffer">A <see cref="byte"/>[] of input file</param>
        /// <returns>A <see cref="byte"/>[] that represents audio data(formatter:RIFF (little-endian) data, WAVE audio, Microsoft PCM, 16 bit, mono 8000 Hz)</returns>
        public static byte[] DecodeAudio(byte[] fileBuffer)
        {
            return DecodeAudio(fileBuffer, default);
        }
        /// <summary>
        /// Decode audio from file path of most audio/video file.
        /// </summary>
        /// <param name="fileBuffer">A <see cref="byte"/>[] of input file</param>
        /// <param name="startTime">Skip <paramref name="startTime"/> from the beginning of the file</param>
        /// <returns>A <see cref="byte"/>[] that represents audio data(formatter:RIFF (little-endian) data, WAVE audio, Microsoft PCM, 16 bit, mono 8000 Hz)</returns>
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
#if NETSTANDARD2_0
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

        private static unsafe class NativeMethods
        {
            [DllImport("libacrcloud_extr_tool.dll", EntryPoint = "create_fingerprint")]
            public static extern int CreateFingerprint(byte[] pcm_buffer, int pcm_buffer_len, [MarshalAs(UnmanagedType.I1)]bool is_db_fingerprint, int filter_energy_min, int silence_energy_threshold, float silence_rate_threshold, out byte* fps_buffer);

            [DllImport("libacrcloud_extr_tool.dll", EntryPoint = "create_fingerprint_by_file")]
            public static extern int CreateFingerprint(string file_path, int start_time_seconds, int audio_len_seconds, [MarshalAs(UnmanagedType.I1)]bool is_db_fingerprint, int filter_energy_min, int silence_energy_threshold, float silence_rate_threshold, out byte* fps_buffer);

            [DllImport("libacrcloud_extr_tool.dll", EntryPoint = "create_fingerprint_by_filebuffer")]
            public static extern int CreateFingerprint(byte[] file_buffer, int file_buffer_len, int start_time_seconds, int audio_len_seconds, [MarshalAs(UnmanagedType.I1)]bool is_db_fingerprint, int filter_energy_min, int silence_energy_threshold, float silence_rate_threshold, out byte* fps_buffer);

            [DllImport("libacrcloud_extr_tool.dll", EntryPoint = "create_humming_fingerprint")]
            public static extern int CreateHummingFingerprint(byte[] pcm_buffer, int pcm_buffer_len, out byte* fps_buffer);

            [DllImport("libacrcloud_extr_tool.dll", EntryPoint = "create_humming_fingerprint_by_file", CharSet = CharSet.Ansi)]
            public static extern int CreateHummingFingerprint(string file_path, int start_time_seconds, int audio_len_seconds, out byte* fps_buffer);

            [DllImport("libacrcloud_extr_tool.dll", EntryPoint = "create_humming_fingerprint_by_filebuffer")]
            public static extern int CreateHummingFingerprint(byte[] file_buffer, int file_buffer_len, int start_time_seconds, int audio_len_seconds, out byte* fps_buffer);

            [DllImport("libacrcloud_extr_tool.dll", EntryPoint = "decode_audio_by_file", CharSet = CharSet.Ansi)]
            public static extern int DecodeAudio(string file_path, int start_time_seconds, int audio_len_seconds, out byte* audio_buffer);

            [DllImport("libacrcloud_extr_tool.dll", EntryPoint = "decode_audio_by_filebuffer")]
            public static extern int DecodeAudio(byte[] file_buffer, int file_buffer_len, int start_time_seconds, int audio_len_seconds, out byte* audio_buffer);

            [DllImport("libacrcloud_extr_tool.dll", EntryPoint = "acr_free")]
            public static extern void Free(byte* buffer);

            [DllImport("libacrcloud_extr_tool.dll", EntryPoint = "get_duration_ms_by_file", CharSet = CharSet.Ansi)]
            public static extern int GetDuration(string filePath);

            [DllImport("libacrcloud_extr_tool.dll", EntryPoint = "acr_set_debug")]
            public static extern void SetDebug([MarshalAs(UnmanagedType.I1)]bool isDebug);

            [DllImport("libacrcloud_extr_tool.dll", EntryPoint = "acr_init")]
            public static extern void Init();
        }
    }
}
