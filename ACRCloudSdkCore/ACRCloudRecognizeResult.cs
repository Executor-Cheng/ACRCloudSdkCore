using System;
using System.Diagnostics;
#if NETSTANDARD2_0 || NETFRAMEWORK
using Newtonsoft.Json.Linq;
#else
using System.Text.Json;
#endif

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1819 // Properties should not return arrays
namespace ACRCloudSdkCore
{
    /// <summary>
    /// Represents the result from an ACRCloud recognition.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ACRCloudRecognizeResult
    {
        /// <summary>
        /// Gets the ACRCloud unique identifier.
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// Gets the track title.
        /// </summary>
        public string Title { get; }
        /// <summary>
        /// Gets the artists.
        /// </summary>
        public string[] Artists { get; }
        /// <summary>
        /// Gets the track album.
        /// </summary>
        public string Album { get; }
        /// <summary>
        /// Gets the time position of the audio/song being played.
        /// </summary>
        public TimeSpan? PlayOffset { get; }
        /// <summary>
        /// Gets the duration of the track.
        /// </summary>
        public TimeSpan? Duration { get; }
        /// <summary>
        /// Gets the match confidence score. Range: [70, 100]
        /// </summary>
        public int Score { get; }
        /// <summary>
        /// Gets the server time of sending results.
        /// </summary>
        public DateTime Time { get; }
#if NETSTANDARD2_0 || NETFRAMEWORK
        /// <summary>
        /// Gets the server responded message, represented by <see cref="JObject"/>.
        /// </summary>
        public JObject ResponseRoot { get; }
        private string DebuggerDisplay => $"{string.Join(",", Artists)} - {Title} {(string.IsNullOrEmpty(Album) ? "" : $"{{{Album}}} ")}[{PlayOffset.GetValueOrDefault():g}/{Duration.GetValueOrDefault():g}]";
#else
        /// <summary>
        /// Gets the server responded message, represented by <see cref="JsonElement"/>.
        /// </summary>
        public JsonElement ResponseRoot { get; }
        private string DebuggerDisplay => $"{string.Join(',', Artists)} - {Title} {(string.IsNullOrEmpty(Album) ? "" : $"{{{Album}}} ")}[{PlayOffset.GetValueOrDefault():g}/{Duration.GetValueOrDefault():g}]";
#endif

#if NETSTANDARD2_0 || NETFRAMEWORK
        public ACRCloudRecognizeResult(string id, string title, string[] artists, string album, TimeSpan? playOffset, TimeSpan? duration, int score, DateTime time, JObject responseRoot)
#else
        public ACRCloudRecognizeResult(string id, string title, string[] artists, string album, TimeSpan? playOffset, TimeSpan? duration, int score, DateTime time, JsonElement responseRoot)
#endif

        {
            Id = id;
            Title = title;
            Artists = artists;
            Album = album;
            PlayOffset = playOffset;
            Duration = duration;
            Score = score;
            Time = time;
            ResponseRoot = responseRoot;
        }
    }
}
