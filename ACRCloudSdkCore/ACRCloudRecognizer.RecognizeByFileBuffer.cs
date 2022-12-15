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
        /// <inheritdoc cref="RecognizeByFileAsync(byte[], TimeSpan, RecognizeType, CancellationToken)"/>
        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(byte[] fileBuffer, RecognizeType type = DefaultRecognizeType, CancellationToken token = default)
        {
            return RecognizeByFileAsync(fileBuffer, default, type, token);
        }

        /// <inheritdoc cref="RecognizeByFileAsync(byte[], TimeSpan, TimeSpan, RecognizeType, CancellationToken)"/>
        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(byte[] fileBuffer, TimeSpan startTime, RecognizeType type = DefaultRecognizeType, CancellationToken token = default)
        {
            return RecognizeByFileAsync(fileBuffer, startTime, TimeSpan.FromSeconds(ACRCloudExtractTools.DefaultDurationSeconds), type, token);
        }

        /// <inheritdoc cref="RecognizeByFileAsync(byte[], TimeSpan, TimeSpan, bool, RecognizeType, CancellationToken)"/>
        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(byte[] fileBuffer, TimeSpan startTime, TimeSpan duration, RecognizeType type = DefaultRecognizeType, CancellationToken token = default)
        {
            return RecognizeByFileAsync(fileBuffer, startTime, duration, false, type, token);
        }

        /// <inheritdoc cref="RecognizeByFileAsync(byte[], TimeSpan, TimeSpan, bool, int, int, float, RecognizeType, CancellationToken)"/>
        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(byte[] fileBuffer, TimeSpan startTime, TimeSpan duration, bool isDB, RecognizeType type = DefaultRecognizeType, CancellationToken token = default)
        {
            return RecognizeByFileAsync(fileBuffer, startTime, duration, isDB, DefaultMinFilterEnergy, DefaultSilenceEnergyThreshold, DefaultSilenceRateThreshold, type, token);
        }

        /// <param name="fileBuffer">A <see cref="byte"/>[] of input file</param>
        /// <inheritdoc cref="RecognizeByFileAsync(Memory{byte}, TimeSpan, TimeSpan, bool, int, int, float, RecognizeType, CancellationToken)"/>
        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(byte[] fileBuffer, TimeSpan startTime, TimeSpan duration, bool isDB,
                                                                  int minFilterEnergy = DefaultMinFilterEnergy,
                                                                  int silenceEnergyThreshold = DefaultSilenceEnergyThreshold,
                                                                  float silenceRateThreshold = DefaultSilenceRateThreshold,
                                                                  RecognizeType type = DefaultRecognizeType,
                                                                  CancellationToken token = default)
        {
            return RecognizeByFileAsync(fileBuffer.AsMemory(), startTime, duration, isDB, minFilterEnergy, silenceEnergyThreshold, silenceRateThreshold, type, token);
        }

        /// <inheritdoc cref="RecognizeByFileAsync(Memory{byte}, TimeSpan, RecognizeType, CancellationToken)"/>
        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(Memory<byte> fileBuffer, RecognizeType type = DefaultRecognizeType, CancellationToken token = default)
        {
            return RecognizeByFileAsync(fileBuffer, default, type, token);
        }

        /// <inheritdoc cref="RecognizeByFileAsync(Memory{byte}, TimeSpan, TimeSpan, RecognizeType, CancellationToken)"/>
        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(Memory<byte> fileBuffer, TimeSpan startTime, RecognizeType type = DefaultRecognizeType, CancellationToken token = default)
        {
            return RecognizeByFileAsync(fileBuffer, startTime, TimeSpan.FromSeconds(ACRCloudExtractTools.DefaultDurationSeconds), type, token);
        }

        /// <inheritdoc cref="RecognizeByFileAsync(Memory{byte}, TimeSpan, TimeSpan, bool, RecognizeType, CancellationToken)"/>
        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(Memory<byte> fileBuffer, TimeSpan startTime, TimeSpan duration, RecognizeType type = DefaultRecognizeType, CancellationToken token = default)
        {
            return RecognizeByFileAsync(fileBuffer, startTime, duration, false, type, token);
        }

        /// <inheritdoc cref="RecognizeByFileAsync(Memory{byte}, TimeSpan, TimeSpan, bool, int, int, float, RecognizeType, CancellationToken)"/>
        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(Memory<byte> fileBuffer, TimeSpan startTime, TimeSpan duration, bool isDB, RecognizeType type = DefaultRecognizeType, CancellationToken token = default)
        {
            return RecognizeByFileAsync(fileBuffer, startTime, duration, isDB, DefaultMinFilterEnergy, DefaultSilenceEnergyThreshold, DefaultSilenceRateThreshold, type, token);
        }

        /// <summary>
        /// Recognizes the audio as an asynchronous operation.
        /// </summary>
        /// <exception cref="HttpRequestException"/>
        /// <param name="fileBuffer">A <see cref="Memory{T}"/> of input file</param>
        /// <param name="type">The audio type.</param>
        /// <param name="token">A <see cref="CancellationToken"/> which may be used to cancel the recognize operation.</param>
        /// <returns>A task that represents the asynchronous deserialize operation.</returns>
        /// <inheritdoc cref="ACRCloudExtractTools.CreateFingerprintByFile(byte[], TimeSpan, TimeSpan, bool, int, int, float)"/>
        /// <inheritdoc cref="CreateResultAsync(Task{HttpResponseMessage})"/>
        public Task<ACRCloudRecognizeResult?> RecognizeByFileAsync(Memory<byte> fileBuffer, TimeSpan startTime, TimeSpan duration, bool isDB,
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
                    byte[] audio = ACRCloudExtractTools.CreateFingerprintByFile(fileBuffer.Span, startTime, duration, isDB, minFilterEnergy, silenceEnergyThreshold, silenceRateThreshold);
                    yield return new KeyValuePair<string?, string?>("sample_bytes", audio.Length.ToString());
                    yield return new KeyValuePair<string?, string?>("sample", Convert.ToBase64String(audio));
                }
                if ((type & RecognizeType.Humming) != 0)
                {
                    byte[] humming = ACRCloudExtractTools.CreateHummingFingerprintByFile(fileBuffer.Span, startTime, duration);
                    yield return new KeyValuePair<string?, string?>("sample_hum_bytes", humming.Length.ToString());
                    yield return new KeyValuePair<string?, string?>("sample_hum", Convert.ToBase64String(humming));
                }
            }
            FormUrlEncodedContent content = new FormUrlEncodedContent(getContents().Concat(GetCommonContents()));
            Task<HttpResponseMessage> response = Client.PostAsync($"http://{Options.Host}/v1/identify", content, token);
            return CreateResultAsync(response);
        }
    }
}
