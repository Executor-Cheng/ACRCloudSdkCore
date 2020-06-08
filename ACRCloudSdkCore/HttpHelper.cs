using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
#if NETSTANDARD2_0
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#else
using System.Text.Json;
#endif

#pragma warning disable IDE1006 // 命名样式
namespace ACRCloudSdkCore
{
    internal static class HttpHelper
    {
        public static readonly string DefaultUserAgent = $"{nameof(ACRCloudSdkCore)}/{Assembly.GetExecutingAssembly().GetName().Version}";

        public static HttpWebRequest PrepareHttpRequest(string url, string method, int timeout, string userAgent, string cookie, IDictionary<string, string> headers, IWebProxy proxy)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Accept = "*/*";
            request.Method = method;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Proxy = proxy;
            request.Timeout = request.ReadWriteTimeout = (timeout > 0 ? timeout : 10) * 1000;
            request.UserAgent = userAgent ?? DefaultUserAgent;
            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }
            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    switch (key.ToLower())
                    {
                        case "accept":
                            {
                                request.Accept = headers[key];
                                break;
                            }
                        case "host":
                            {
                                request.Host = headers[key];
                                break;
                            }
                        case "referer":
                            {
                                request.Referer = headers[key];
                                break;
                            }
                        case "range":
                            {
                                Match m = Regex.Match(headers[key], @"(\S+)=(\d+)-(\d+)");
                                if (m.Success)
                                {
                                    request.AddRange(m.Groups[1].Value, int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value));
                                }
                                break;
                            }
                        default:
                            {
                                request.Headers.Add(key, headers[key]);
                                break;
                            }
                    }
                }
            }
            if (request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                request.ContentType ??= "application/x-www-form-urlencoded";
            }
            return request;
        }

        public static Task<WebResponse> HttpGetAsync(string url, int timeout = 0, string userAgent = null, string cookie = null, IDictionary<string, string> headers = null, IWebProxy proxy = null)
        {
            HttpWebRequest request = PrepareHttpRequest(url, "GET", timeout, userAgent, cookie, headers, proxy);
            return request.GetResponseAsync();
        }

        public static Task<WebResponse> HttpPostAsync(string url, byte[] buffer, int timeout = 0, string userAgent = null, string cookie = null, IDictionary<string, string> headers = null, IWebProxy proxy = null)
        {
            HttpWebRequest request = PrepareHttpRequest(url, "POST", timeout, userAgent, cookie, headers, proxy);
            if (buffer?.Length > 0)
            {
                using Stream stream = request.GetRequestStream();
                stream.Write(buffer, 0, buffer.Length);
            }
            return request.GetResponseAsync();
        }
        public static Task<WebResponse> HttpPostAsync(string url, string formdata, int timeout = 0, string userAgent = null, string cookie = null, IDictionary<string, string> headers = null, IWebProxy proxy = null)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(formdata);
            return HttpPostAsync(url, buffer, timeout, userAgent, cookie, headers, proxy);
        }
        public static Task<WebResponse> HttpPostAsync(string url, IDictionary<string, object> parameters = null, int timeout = 0, string userAgent = null, string cookie = null, IDictionary<string, string> headers = null, IWebProxy proxy = null)
        {
            string formdata = string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));
            return HttpPostAsync(url, formdata, timeout, userAgent, cookie, headers, proxy);
        }
        public static async Task<WebResponse> HttpPostAsync(string url, IEnumerable<HttpContent> contents, string boundary = nameof(ACRCloudSdkCore), int timeout = 10, string userAgent = null, string cookie = null, IDictionary<string, string> headers = null, IWebProxy proxy = null)
        {
            HttpWebRequest request = PrepareHttpRequest(url, "POST", timeout, userAgent, cookie, headers, proxy);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            using (MultipartFormDataContent multipart = new MultipartFormDataContent(boundary))
            {
                foreach (HttpContent content in contents)
                {
                    multipart.Add(content);
                }
                using Stream stream = request.GetRequestStream();
                using Stream multipartStream = await multipart.ReadAsStreamAsync();
                await multipartStream.CopyToAsync(stream);
            }
            return await request.GetResponseAsync();
        }

        public static async Task<string> GetStringAsync(this WebResponse response, Encoding encoding = null, bool disposeResponse = true)
        {
            try
            {
                using Stream stream = response.GetResponseStream();
                using StreamReader reader = new StreamReader(stream, encoding ?? Encoding.UTF8);
                return await reader.ReadToEndAsync();
            }
            finally
            {
                if (disposeResponse)
                {
                    response.Dispose();
                }
            }
        }
        public static async Task<string> GetStringAsync(this Task<WebResponse> responseTask, Encoding encoding = null)
        {
            using WebResponse response = await responseTask.ConfigureAwait(false);
            using Stream stream = response.GetResponseStream();
            using StreamReader reader = new StreamReader(stream, encoding ?? Encoding.UTF8);
            return await reader.ReadToEndAsync();
        }

        public static async Task<byte[]> GetBytesAsync(this WebResponse response, bool disposeResponse = true, CancellationToken token = default)
        {
            try
            {
                using Stream stream = response.GetResponseStream();
                using MemoryStream ms = new MemoryStream();
#if NETSTANDARD2_0
                await stream.CopyToAsync(ms, 81920, token);
#else
                await stream.CopyToAsync(ms, token);
#endif
                return ms.ToArray();
            }
            finally
            {
                if (disposeResponse)
                {
                    response.Dispose();
                }
            }
        }

        public static async Task<byte[]> GetBytesAsync(this Task<WebResponse> responseTask, CancellationToken token = default)
        {
            using WebResponse response = await responseTask.ConfigureAwait(false);
            using Stream stream = response.GetResponseStream();
            using MemoryStream ms = new MemoryStream();
#if NETSTANDARD2_0
            await stream.CopyToAsync(ms, 81920, token);
#else
            await stream.CopyToAsync(ms, token);
#endif
            return ms.ToArray();
        }

#if NETSTANDARD2_0
        public static Task<JObject> GetJsonAsync(this Task<WebResponse> responseTask, CancellationToken token = default)
#else
        public static Task<JsonDocument> GetJsonAsync(this Task<WebResponse> responseTask, CancellationToken token = default)
#endif
        {
            return responseTask.GetJsonAsync(default, token);
        }

#if NETSTANDARD2_0
        public static async Task<JObject> GetJsonAsync(this Task<WebResponse> responseTask, JsonLoadSettings options, CancellationToken token = default)
#else
        public static async Task<JsonDocument> GetJsonAsync(this Task<WebResponse> responseTask, JsonDocumentOptions options, CancellationToken token = default)
#endif
        {
            using WebResponse response = await responseTask.ConfigureAwait(false);
            using Stream stream = response.GetResponseStream();
#if NETSTANDARD2_0
            using TextReader reader = new StreamReader(stream, Encoding.UTF8);
            JsonReader jReader = new JsonTextReader(reader);
            return await JObject.LoadAsync(jReader, options, token);
#else
            return await JsonDocument.ParseAsync(stream, options, token);
#endif
        }

#if !NETSTANDARD2_0
        public static Task<JsonElement> GetPersistentJsonAsync(this Task<WebResponse> responseTask, CancellationToken token = default)
        {
            return responseTask.GetPersistentJsonAsync(null, token);
        }
        public static async Task<JsonElement> GetPersistentJsonAsync(this Task<WebResponse> responseTask, JsonSerializerOptions options, CancellationToken token = default)
        {
            using WebResponse response = await responseTask.ConfigureAwait(false);
            using Stream stream = response.GetResponseStream();
            return await JsonSerializer.DeserializeAsync<JsonElement>(stream, options, token);
        }
#endif

        public static Task<string> IgnoreNonSuccess(this Task<string> task)
        {
            return task.ContinueWith(t =>
            {
                if (t.IsFaulted && t.Exception.InnerException is WebException e && e.Response is HttpWebResponse resp)
                {
                    return resp.GetStringAsync();
                }
                return t;
            }, TaskContinuationOptions.ExecuteSynchronously).Unwrap();
        }

        public static Task<WebResponse> IgnoreNonSuccess(this Task<WebResponse> task)
        {
            return task.ContinueWith(t =>
            {
                if (t.IsFaulted && t.Exception.InnerException is WebException e)
                {
                    return Task.FromResult(e.Response);
                }
                return t;
            }, TaskContinuationOptions.ExecuteSynchronously).Unwrap();
        }
    }
}
