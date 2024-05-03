using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CID.HttpHelper.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> GetWithHeaderAsync(this HttpClient httpClient, string requestUri, Dictionary<string, string> headers)
        {
            RemoveInvalidContentTypeHeader();
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }

                return await httpClient.SendAsync(request);
            }
        }

        public static async Task<HttpResponseMessage> PostJsonWithHeaderAsync(this HttpClient httpClient, string requestUri, Dictionary<string, string> headers, string contentBody)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            StringContent content = new StringContent(contentBody, Encoding.UTF8, "application/json");

            return await httpClient.PostAsync(requestUri, content);
        }

        private static void RemoveInvalidContentTypeHeader()
        {
            //a very hacky way of adding Content-Type to Headers in HttpClient when using .net Framework for the GET request. If we are upgrading to .NetCore, need to rewrite GET functions.
            var field = typeof(System.Net.Http.Headers.HttpRequestHeaders)
                        .GetField("invalidHeaders", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                         ?? typeof(System.Net.Http.Headers.HttpRequestHeaders)
                        .GetField("s_invalidHeaders", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            if (field != null)
            {
                var invalidFields = (HashSet<string>)field.GetValue(null);
                invalidFields.Remove("Content-Type");
            }
        }
    }
}
