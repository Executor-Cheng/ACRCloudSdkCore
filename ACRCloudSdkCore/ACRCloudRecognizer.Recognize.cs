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
        /// <summary>
        /// Recognizes the wav audio as an asynchronous operation.
        /// </summary>
        /// <exception cref="HttpRequestException"/>
        /// <param name="type">The audio type.</param>
        /// <param name="token">A <see cref="CancellationToken"/> which may be used to cancel the recognize operation.</param>
        /// <returns>A task that represents the asynchronous deserialize operation.</returns>
        /// <inheritdoc cref="CreateResultAsync(Task{HttpResponseMessage})"/>
        /// <inheritdoc cref="ACRCloudExtractTools.CreateHummingFingerprint(byte[])"/>
        public Task<ACRCloudRecognizeResult?> RecognizeAsync(byte[] pcmBuffer, RecognizeType type = DefaultRecognizeType, CancellationToken token = default)
        {
            IEnumerable<KeyValuePair<string?, string?>> getContents()
            {
                if ((type & RecognizeType.Audio) != 0)
                {
                    byte[] audio = ACRCloudExtractTools.CreateFingerprint(pcmBuffer);
                    yield return new KeyValuePair<string?, string?>("sample_bytes", audio.Length.ToString());
                    yield return new KeyValuePair<string?, string?>("sample", Convert.ToBase64String(audio));
                }
                if ((type & RecognizeType.Humming) != 0)
                {
                    byte[] humming = ACRCloudExtractTools.CreateHummingFingerprint(pcmBuffer);
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
