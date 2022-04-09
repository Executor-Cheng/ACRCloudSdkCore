using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable CS1573 // Parameter <parameter> has no matching param tag in the XML comment for <method> (but other parameters do)
namespace ACRCloudSdkCore
{
    public partial class ACRCloudRecognizer
    {
        /// <inheritdoc cref="RecognizeByFileAsync(string, TimeSpan, RecognizeType, CancellationToken)"/>
        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(string filePath, RecognizeType type = DefaultRecognizeType, CancellationToken token = default)
        {
            return RecognizeByFileAsync(filePath, default, type, token);
        }

        /// <inheritdoc cref="RecognizeByFileAsync(string, TimeSpan, TimeSpan, RecognizeType, CancellationToken)"/>
        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(string filePath, TimeSpan startTime, RecognizeType type = DefaultRecognizeType, CancellationToken token = default)
        {
            return RecognizeByFileAsync(filePath, startTime, TimeSpan.FromSeconds(ACRCloudExtractTools.DefaultDurationSeconds), type, token);
        }

        /// <inheritdoc cref="RecognizeByFileAsync(string, TimeSpan, TimeSpan, bool, RecognizeType, CancellationToken)"/>
        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(string filePath, TimeSpan startTime, TimeSpan duration, RecognizeType type = DefaultRecognizeType, CancellationToken token = default)
        {
            return RecognizeByFileAsync(filePath, startTime, duration, false, type, token);
        }

        /// <inheritdoc cref="RecognizeByFileAsync(string, TimeSpan, TimeSpan, bool, int, int, float, RecognizeType, CancellationToken)"/>
        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(string filePath, TimeSpan startTime, TimeSpan duration, bool isDB, RecognizeType type = DefaultRecognizeType, CancellationToken token = default)
        {
            return RecognizeByFileAsync(filePath, startTime, duration, isDB, DefaultMinFilterEnergy, DefaultSilenceEnergyThreshold, DefaultSilenceRateThreshold, type, token);
        }

        /// <summary>
        /// Recognizes the audio file as an asynchronous operation.
        /// </summary>
        /// <exception cref="HttpRequestException"/>
        /// <param name="type">The audio type.</param>
        /// <param name="token">A <see cref="CancellationToken"/> which may be used to cancel the recognize operation.</param>
        /// <returns>A task that represents the asynchronous deserialize operation.</returns>
        /// <inheritdoc cref="ACRCloudExtractTools.CreateFingerprintByFile(string, TimeSpan, TimeSpan, bool, int, int, float)"/>
        /// <inheritdoc cref="CreateResultAsync(Task{HttpResponseMessage})"/>
        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(string filePath, TimeSpan startTime, TimeSpan duration, bool isDB,
                                                                  int minFilterEnergy = DefaultMinFilterEnergy,
                                                                  int silenceEnergyThreshold = DefaultSilenceEnergyThreshold,
                                                                  float silenceRateThreshold = DefaultSilenceRateThreshold,
                                                                  RecognizeType type = DefaultRecognizeType,
                                                                  CancellationToken token = default)
        {
            IEnumerable<KeyValuePair<string?, string?>> getContents()
            {
                if ((type & RecognizeType.Audio) != 0)
                {
                    byte[] audio = ACRCloudExtractTools.CreateFingerprintByFile(filePath, startTime, duration, isDB, minFilterEnergy, silenceEnergyThreshold, silenceRateThreshold);
                    yield return new KeyValuePair<string?, string?>("sample_bytes", audio.Length.ToString());
                    yield return new KeyValuePair<string?, string?>("sample", Convert.ToBase64String(audio));
                }
                if ((type & RecognizeType.Humming) != 0)
                {
                    byte[] humming = ACRCloudExtractTools.CreateHummingFingerprintByFile(filePath, startTime, duration);
                    yield return new KeyValuePair<string?, string?>("sample_hum_bytes", humming.Length.ToString());
                    yield return new KeyValuePair<string?, string?>("sample_hum", Convert.ToBase64String(humming));
                }
            }
            FormUrlEncodedContent content = new FormUrlEncodedContent(getContents().Concat(GetCommonContents()));
            Task<HttpResponseMessage> response = Client.PostAsync($"http://{Options.Host}/v2/identify", content, token);
            return CreateResultAsync(response);
        }
    }
}
