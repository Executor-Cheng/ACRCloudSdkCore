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
        private static void CheckBuffer(Span<byte> buffer)
        {
            if (buffer.IsEmpty)
            {
                throw new ArgumentException("Invalid audio data.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CheckFilePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path must not be null or empty.", nameof(filePath));
            }
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Could not find file '{filePath}'.", filePath);
            }
        }
    }
}
