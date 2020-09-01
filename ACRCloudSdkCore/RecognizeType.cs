using System;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1401 // P/Invokes should not be visible
#pragma warning disable CA2101 // Specify marshaling for P/Invoke string arguments
namespace ACRCloudSdkCore
{
    /// <summary>
    /// Represents the audio type.
    /// </summary>
    [Flags]
    public enum RecognizeType
    {
        Audio = 1 << 0,
        Humming = 1 << 1,
        Both = Audio | Humming
    };
}
