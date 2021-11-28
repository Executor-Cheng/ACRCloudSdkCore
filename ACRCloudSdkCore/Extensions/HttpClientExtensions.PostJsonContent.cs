#if !NET5_0
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
#if NETSTANDARD2_0 || NETFRAMEWORK
using System.Text;
using Newtonsoft.Json;
#else
using System.Text.Json;
#endif

#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
namespace ACRCloudSdkCore.Extensions
{
    public static partial class HttpClientExtensions
    {
#if NETSTANDARD2_0 || NETFRAMEWORK
        /// <param name="value">The value to be serialized to <see cref="HttpContent"/>.</param>
        /// <param name="options">A <see cref="JsonSerializerSettings"/> to be used while serializing the <paramref name="value"/> to <see cref="HttpContent"/>.</param>
        /// <inheritdoc cref="PostAsync(HttpClient, Uri, byte[], CancellationToken)"/>
        public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient client, Uri uri, TValue value, JsonSerializerSettings? options, CancellationToken token = default)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(value, options!), Encoding.UTF8);
            content.Headers.ContentType = DefaultJsonMediaType;
            return client.PostAsync(uri, content, token);
        }

        /// <param name="url">The url the request is sent to.</param>
        /// <inheritdoc cref="PostAsJsonAsync{TValue}(HttpClient, Uri, TValue, JsonSerializerSettings?, CancellationToken)"/>
        public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient client, string url, TValue value, JsonSerializerSettings? options, CancellationToken token = default)
            => client.PostAsJsonAsync(new Uri(url), value, options, token);
#else
        /// <param name="value">The value to be serialized to <see cref="HttpContent"/>.</param>
        /// <param name="options">A <see cref="JsonSerializerOptions"/> to be used while serializing the <paramref name="value"/> to <see cref="HttpContent"/>.</param>
        /// <inheritdoc cref="PostAsync(HttpClient, Uri, byte[], CancellationToken)"/>
        public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient client, Uri uri, TValue value, JsonSerializerOptions? options, CancellationToken token = default)
        {
            ByteArrayContent content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(value, options));
            content.Headers.ContentType = DefaultJsonMediaType;
            return client.PostAsync(uri, content, token);
        }

        /// <param name="url">The url the request is sent to.</param>
        /// <inheritdoc cref="PostAsJsonAsync{TValue}(HttpClient, Uri, TValue, JsonSerializerOptions?, CancellationToken)"/>
        public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this HttpClient client, string url, TValue value, JsonSerializerOptions? options, CancellationToken token = default)
            => client.PostAsJsonAsync(new Uri(url), value, options, token);
#endif
    }
}
#endif