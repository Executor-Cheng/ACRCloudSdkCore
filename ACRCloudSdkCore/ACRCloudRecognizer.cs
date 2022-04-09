using ACRCloudSdkCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ACRCloudSdkCore.Extensions;
#if NETSTANDARD2_0 || NETFRAMEWORK
using Newtonsoft.Json.Linq;
#else
using System.Text.Json;
#endif

namespace ACRCloudSdkCore
{
    /// <summary>
    /// Provides ACRCloud audio identification implementation.
    /// </summary>
    public partial class ACRCloudRecognizer : IDisposable
    {
        private const RecognizeType DefaultRecognizeType = RecognizeType.Audio;

        private const int DefaultMinFilterEnergy = ACRCloudExtractTools.DefaultMinFilterEnergy;

        private const int DefaultSilenceEnergyThreshold = ACRCloudExtractTools.DefaultSilenceEnergyThreshold;

        private const float DefaultSilenceRateThreshold = ACRCloudExtractTools.DefaultSilenceRateThreshold;

        private const string DefaultDataType = "fingerprint";

        private const string DefaultSignVersion = "1";

        private static readonly KeyValuePair<string?, string?> DefaultDataTypeContent = new KeyValuePair<string?, string?>("data_type", DefaultDataType);

        private static readonly KeyValuePair<string?, string?> DefaultSignVersionContent = new KeyValuePair<string?, string?>("signature_version", DefaultSignVersion);

        private ACRCloudOptions Options { get; }

        private HttpClient Client { get; } = new HttpClient();

        public ACRCloudRecognizer(ACRCloudOptions options)
            => Options = options;

        private IEnumerable<KeyValuePair<string?, string?>> GetCommonContents()
        {
            string timeStamp = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000).ToString();
            using HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(Options.AccessSecret));
            string signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes($"POST\n/v2/identify\n{Options.AccessKey}\n{DefaultDataType}\n{DefaultSignVersion}\n{timeStamp}")));
            yield return new KeyValuePair<string?, string?>("access_key", Options.AccessKey);
            yield return DefaultDataTypeContent;
            yield return DefaultSignVersionContent;
            yield return new KeyValuePair<string?, string?>("signature", signature);
            yield return new KeyValuePair<string?, string?>("timestamp", timeStamp);
        }

        /// <exception cref="InvalidAccessKeyException"/>
        /// <exception cref="InvalidAccessSecretException"/>
        /// <exception cref="LimitExceededException"/>
        /// <exception cref="UnknownResponseException"/>
        private async Task<ACRCloudRecognizeResult?> CreateResultAsync(Task<HttpResponseMessage> response) // Suppress all CS8602/CS8604 in JToken[string] / JToken.ToObject<T>
        {
#if NETSTANDARD2_0 || NETFRAMEWORK
            JObject root = (JObject)await response.GetJsonAsync();
            JToken status = root["status"]!;
            switch (status["code"]!.ToObject<int>())
            {
                case 0:
                    {
                        JToken metadata = root["metadata"]!,
                               music = metadata["music"]![0]!;
                        int? playOffset = music["play_offset_ms"]?.ToObject<int>(),
                             duration = music["duration_ms"]?.ToObject<int>();
                        return new ACRCloudRecognizeResult(
                            music["acrid"]!.ToObject<string>()!,
                            music["title"]!.ToObject<string>()!,
                            music["artists"]!.Select(p => p["name"]!.ToObject<string>()!).ToArray(),
                            music["album"]!["name"]!.ToObject<string>()!,
                            playOffset.HasValue ? TimeSpan.FromMilliseconds(playOffset.Value) : default(TimeSpan?),
                            duration.HasValue ? TimeSpan.FromMilliseconds(duration.Value) : default(TimeSpan?),
                            music["score"]!.ToObject<int>(),
                            metadata["timestamp_utc"]!.ToObject<DateTime>().ToLocalTime(),
                            root
                            );
                    }
#else
            JsonElement root = await response.GetObjectAsync<JsonElement>(),
                        status = root.GetProperty("status");
            switch (status.GetProperty("code").GetInt32())
            {
                case 0:
                    {
                        JsonElement metadata = root.GetProperty("metadata"),
                                    music = metadata.GetProperty("music")[0];
                        return new ACRCloudRecognizeResult(
                            music.GetProperty("acrid").GetString()!,
                            music.GetProperty("title").GetString()!,
                            music.GetProperty("artists").EnumerateArray().Select(p => p.GetProperty("name").GetString()!).ToArray(),
                            music.GetProperty("album").GetProperty("name").GetString()!,
                            music.TryGetProperty("play_offset_ms", out JsonElement playOffset) ? TimeSpan.FromMilliseconds(playOffset.GetInt32()) : default(TimeSpan?),
                            music.TryGetProperty("duration_ms", out JsonElement duration) ? TimeSpan.FromMilliseconds(duration.GetInt32()) : default(TimeSpan?),
                            music.GetProperty("score").GetInt32(),
                            DateTime.Parse(metadata.GetProperty("timestamp_utc").GetString()!).ToLocalTime(),
                            root
                            );
                    }
#endif
                case 1001:
                    {
                        return null;
                    }
                case 3001:
                    {
                        throw new InvalidAccessKeyException(Options.AccessKey);
                    }
                case 3003:
                    {
                        throw new LimitExceededException();
                    }
                case 3014:
                    {
                        throw new InvalidAccessSecretException(Options.AccessSecret);
                    }
                default:
                    {
#if NETSTANDARD2_0 || NETFRAMEWORK
                        throw new UnknownResponseException(root.ToString(0));
#else
                        throw new UnknownResponseException(root.GetRawText());
#endif
                    }
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
