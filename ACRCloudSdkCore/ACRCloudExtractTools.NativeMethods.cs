using System.Runtime.InteropServices;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1401 // P/Invokes should not be visible
#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments
namespace ACRCloudSdkCore
{
    public static unsafe partial class ACRCloudExtractTools
    {
        public static unsafe class NativeMethods
        {
            public const string ExtractToolName = "libacrcloud_extr_tool";

            [DllImport(ExtractToolName, EntryPoint = "create_fingerprint")]
            public static extern int CreateFingerprint(ref byte pcm_buffer, int pcm_buffer_len, [MarshalAs(UnmanagedType.I1)] bool is_db_fingerprint, int filter_energy_min, int silence_energy_threshold, float silence_rate_threshold, out byte* fps_buffer);

            [DllImport(ExtractToolName, EntryPoint = "create_fingerprint_by_file")]
            public static extern int CreateFingerprint(string file_path, int start_time_seconds, int audio_len_seconds, [MarshalAs(UnmanagedType.I1)] bool is_db_fingerprint, int filter_energy_min, int silence_energy_threshold, float silence_rate_threshold, out byte* fps_buffer);

            [DllImport(ExtractToolName, EntryPoint = "create_fingerprint_by_filebuffer")]
            public static extern int CreateFingerprint(ref byte file_buffer, int file_buffer_len, int start_time_seconds, int audio_len_seconds, [MarshalAs(UnmanagedType.I1)] bool is_db_fingerprint, int filter_energy_min, int silence_energy_threshold, float silence_rate_threshold, out byte* fps_buffer);

            [DllImport(ExtractToolName, EntryPoint = "create_humming_fingerprint")]
            public static extern int CreateHummingFingerprint(ref byte pcm_buffer, int pcm_buffer_len, out byte* fps_buffer);

            [DllImport(ExtractToolName, EntryPoint = "create_humming_fingerprint_by_file", CharSet = CharSet.Ansi)]
            public static extern int CreateHummingFingerprint(string file_path, int start_time_seconds, int audio_len_seconds, out byte* fps_buffer);

            [DllImport(ExtractToolName, EntryPoint = "create_humming_fingerprint_by_filebuffer")]
            public static extern int CreateHummingFingerprint(ref byte file_buffer, int file_buffer_len, int start_time_seconds, int audio_len_seconds, out byte* fps_buffer);

            [DllImport(ExtractToolName, EntryPoint = "decode_audio_by_file", CharSet = CharSet.Ansi)]
            public static extern int DecodeAudio(string file_path, int start_time_seconds, int audio_len_seconds, out byte* audio_buffer);

            [DllImport(ExtractToolName, EntryPoint = "decode_audio_by_filebuffer")]
            public static extern int DecodeAudio(ref byte file_buffer, int file_buffer_len, int start_time_seconds, int audio_len_seconds, out byte* audio_buffer);

            [DllImport(ExtractToolName, EntryPoint = "acr_free")]
            public static extern void Free(byte* buffer);

            [DllImport(ExtractToolName, EntryPoint = "get_duration_ms_by_file", CharSet = CharSet.Ansi)]
            public static extern int GetDuration(string filePath);

            [DllImport(ExtractToolName, EntryPoint = "acr_set_debug")]
            public static extern void SetDebug([MarshalAs(UnmanagedType.I1)] bool isDebug);

            [DllImport(ExtractToolName, EntryPoint = "acr_init")]
            public static extern void Init();

#if NETFRAMEWORK
            public static class Win32
            {
                [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
                public static extern void* LoadLibrary(string lpFileName);
            }
#endif
        }
    }
}
