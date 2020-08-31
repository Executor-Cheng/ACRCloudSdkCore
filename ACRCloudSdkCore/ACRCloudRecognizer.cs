﻿using ACRCloudSdkCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ACRCloudSdkCore.Extensions;
#if NETSTANDARD2_0
using Newtonsoft.Json.Linq;
#else
using System.Text.Json;
#endif

namespace ACRCloudSdkCore
{
    public class ACRCloudRecognizer : IDisposable
    {
        private const RecognizeType DefaultRecognizeType = RecognizeType.Audio;

        private const int DefaultMinFilterEnergy = ACRCloudExtractTools.DefaultMinFilterEnergy;

        private const int DefaultSilenceEnergyThreshold = ACRCloudExtractTools.DefaultSilenceEnergyThreshold;

        private const float DefaultSilenceRateThreshold = ACRCloudExtractTools.DefaultSilenceRateThreshold;

        private const string DefaultDataType = "fingerprint";

        private const string DefaultSignVersion = "1";

        private static StringContent DefaultDataTypeContent => CreateStringContent("data_type", DefaultDataType);

        private static StringContent DefaultSignVersionContent => CreateStringContent("signature_version", DefaultSignVersion);

        private ACRCloudOptions Options { get; }

        private HttpClient Client { get; } = new HttpClient();

        public ACRCloudRecognizer(ACRCloudOptions options)
            => Options = options;

        public Task<ACRCloudRecognizeResult?> RecognizeAsync(byte[] pcmBuffer, RecognizeType type = DefaultRecognizeType)
        {
            IEnumerable<HttpContent> getContents()
            {
                if ((type & RecognizeType.Audio) != 0)
                {
                    byte[] audio = ACRCloudExtractTools.CreateFingerprint(pcmBuffer);
                    yield return CreateStringContent("sample_bytes", audio.Length.ToString());
                    yield return CreateByteArrayContent("sample", audio);
                }
                if ((type & RecognizeType.Humming) != 0)
                {
                    byte[] humming = ACRCloudExtractTools.CreateHummingFingerprint(pcmBuffer);
                    yield return CreateStringContent("sample_hum_bytes", humming.Length.ToString());
                    yield return CreateByteArrayContent("sample_hum", humming);
                }
            }
            Task<HttpResponseMessage> response = Client.PostAsync($"http://{Options.Host}/v1/identify", getContents().Concat(GetCommonContents()));
            return CreateResultAsync(response);
        }

        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(string filePath, RecognizeType type = DefaultRecognizeType)
        {
            return RecognizeByFileAsync(filePath, default, type);
        }

        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(string filePath, TimeSpan startTime, RecognizeType type = DefaultRecognizeType)
        {
            return RecognizeByFileAsync(filePath, startTime, TimeSpan.FromSeconds(ACRCloudExtractTools.DefaultDurationSeconds), type);
        }

        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(string filePath, TimeSpan startTime, TimeSpan duration, RecognizeType type = DefaultRecognizeType)
        {
            return RecognizeByFileAsync(filePath, startTime, duration, false, type);
        }

        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(string filePath, TimeSpan startTime, TimeSpan duration, bool isDB, RecognizeType type = DefaultRecognizeType)
        {
            return RecognizeByFileAsync(filePath, startTime, duration, isDB, DefaultMinFilterEnergy, DefaultSilenceEnergyThreshold, DefaultSilenceRateThreshold, type);
        }

        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(string filePath, TimeSpan startTime, TimeSpan duration, bool isDB,
                                                                  int minFilterEnergy = DefaultMinFilterEnergy,
                                                                  int silenceEnergyThreshold = DefaultSilenceEnergyThreshold,
                                                                  float silenceRateThreshold = DefaultSilenceRateThreshold,
                                                                  RecognizeType type = DefaultRecognizeType)
        {
            IEnumerable<HttpContent> getContents()
            {
                if ((type & RecognizeType.Audio) != 0)
                {
                    byte[] audio = ACRCloudExtractTools.CreateFingerprintByFile(filePath, startTime, duration, isDB, minFilterEnergy, silenceEnergyThreshold, silenceRateThreshold);
                    yield return CreateStringContent("sample_bytes", audio.Length.ToString());
                    yield return CreateByteArrayContent("sample", audio);
                }
                if ((type & RecognizeType.Humming) != 0)
                {
                    byte[] humming = ACRCloudExtractTools.CreateHummingFingerprintByFile(filePath, startTime, duration);
                    yield return CreateStringContent("sample_hum_bytes", humming.Length.ToString());
                    yield return CreateByteArrayContent("sample_hum", humming);
                }
            }
            Task<HttpResponseMessage> response = Client.PostAsync($"http://{Options.Host}/v1/identify", getContents().Concat(GetCommonContents()));
            return CreateResultAsync(response);
        }

        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(byte[] fileBuffer, RecognizeType type = DefaultRecognizeType)
        {
            return RecognizeByFileAsync(fileBuffer, default, type);
        }

        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(byte[] fileBuffer, TimeSpan startTime, RecognizeType type = DefaultRecognizeType)
        {
            return RecognizeByFileAsync(fileBuffer, startTime, TimeSpan.FromSeconds(ACRCloudExtractTools.DefaultDurationSeconds), type);
        }

        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(byte[] fileBuffer, TimeSpan startTime, TimeSpan duration, RecognizeType type = DefaultRecognizeType)
        {
            return RecognizeByFileAsync(fileBuffer, startTime, duration, false, type);
        }

        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(byte[] fileBuffer, TimeSpan startTime, TimeSpan duration, bool isDB, RecognizeType type = DefaultRecognizeType)
        {
            return RecognizeByFileAsync(fileBuffer, startTime, duration, isDB, DefaultMinFilterEnergy, DefaultSilenceEnergyThreshold, DefaultSilenceRateThreshold, type);
        }

        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(byte[] fileBuffer, TimeSpan startTime, TimeSpan duration, bool isDB,
                                                                  int minFilterEnergy = DefaultMinFilterEnergy,
                                                                  int silenceEnergyThreshold = DefaultSilenceEnergyThreshold,
                                                                  float silenceRateThreshold = DefaultSilenceRateThreshold,
                                                                  RecognizeType type = DefaultRecognizeType)
        {
            IEnumerable<HttpContent> getContents()
            {
                if ((type & RecognizeType.Audio) != 0)
                {
                    byte[] audio = ACRCloudExtractTools.CreateFingerprintByFile(fileBuffer, startTime, duration, isDB, minFilterEnergy, silenceEnergyThreshold, silenceRateThreshold);
                    yield return CreateStringContent("sample_bytes", audio.Length.ToString());
                    yield return CreateByteArrayContent("sample", audio);
                }
                if ((type & RecognizeType.Humming) != 0)
                {
                    byte[] humming = ACRCloudExtractTools.CreateHummingFingerprintByFile(fileBuffer, startTime, duration);
                    yield return CreateStringContent("sample_hum_bytes", humming.Length.ToString());
                    yield return CreateByteArrayContent("sample_hum", humming);
                }
            }
            Task<HttpResponseMessage> response = Client.PostAsync($"http://{Options.Host}/v1/identify", getContents().Concat(GetCommonContents()));
            return CreateResultAsync(response);
        }

        private IEnumerable<HttpContent> GetCommonContents()
        {
            string timeStamp = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000).ToString();
            using HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(Options.AccessSecret));
            string signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes($"POST\n/v1/identify\n{Options.AccessKey}\n{DefaultDataType}\n{DefaultSignVersion}\n{timeStamp}")));
            yield return CreateStringContent("access_key", Options.AccessKey);
            yield return DefaultDataTypeContent;
            yield return DefaultSignVersionContent;
            yield return CreateStringContent("signature", signature);
            yield return CreateStringContent("timestamp", timeStamp);
        }

        private async Task<ACRCloudRecognizeResult?> CreateResultAsync(Task<HttpResponseMessage> response) // Suppress all CS8602/CS8604 in JToken[string] / JToken.ToObject<T>
        {
#if NETSTANDARD2_0
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
#if NET5_0
            JsonElement root = await response.ForceJson().GetObjectAsync<JsonElement>(),
#else
            JsonElement root = await response.GetObjectAsync<JsonElement>(),
#endif
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
#if NETSTANDARD2_0
                        throw new UnknownResponseException(root.ToString(0));
#else
                        throw new UnknownResponseException(root.GetRawText());
#endif
                    }
            }
        }

        private static ByteArrayContent CreateByteArrayContent(string name, byte[] buffer)
        {
            ByteArrayContent content = new ByteArrayContent(buffer);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = $"\"{name}\""
            };
            return content;
        }

        private static StringContent CreateStringContent(string name, string content)
        {
            StringContent sContent = new StringContent(content);
            sContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = $"\"{name}\""
            };
            return sContent;
        }

        public void Dispose()
        {
            Client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
