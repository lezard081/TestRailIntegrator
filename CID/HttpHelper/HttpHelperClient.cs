using CID.HttpHelper.Model;
using CID.HttpHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Microsoft.Extensions.DependencyInjection;
using CID.TestRail.Utils;

namespace CID.HttpHelper
{
    //Update this class to be a more optimized httpclient
    public class HttpHelperClient : IHttpHelperClient
    {
        private static IHttpClientFactory _httpClientFactory;
        private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy =
            Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(x => x.StatusCode >= HttpStatusCode.InternalServerError || x.StatusCode == HttpStatusCode.RequestTimeout)
            .WaitAndRetryAsync(5, retryAttempts => TimeSpan.FromSeconds(Math.Pow(2, retryAttempts)));

        private Uri _baseAddress { get; set; }


        #region Constructor

        public HttpHelperClient()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            IServiceProvider serviceProvider = ServicesConfig.GetServiceProvider();

            _httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        }

        #endregion

        #region public methods
        async public Task<HttpResponse> MakeHttpGetRequest(string requestUri, Dictionary<string, string> headers = null, string clientName = "")
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(clientName);

            _baseAddress = httpClient.BaseAddress;

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            httpResponseMessage = await _retryPolicy.ExecuteAsync(() => httpClient.GetWithHeaderAsync(requestUri, headers));

            return await ProcessHttpResponse(httpResponseMessage);
        }

        async public Task<HttpResponse> MakeHttpPostRequest(string requestUri, Dictionary<string, string> headers, string content, string clientName = "")
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(clientName);

            _baseAddress = httpClient.BaseAddress;

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            httpResponseMessage = await _retryPolicy.ExecuteAsync(() => httpClient.PostJsonWithHeaderAsync(requestUri, headers, content));

            return await ProcessHttpResponse(httpResponseMessage);
        }

        public string GetBaseAddress()
        {
            return _baseAddress.ToString();
        }

        #endregion

        #region privateMethods


        private static async Task<HttpResponse> ProcessHttpResponse(HttpResponseMessage httpResponseMessage)
        {
            HttpResponse response = new HttpResponse();

            response.StatusCode = httpResponseMessage.StatusCode;
            response.ReasonPhrase = httpResponseMessage.ReasonPhrase;
            response.Content = await httpResponseMessage.Content.ReadAsStringAsync();

            return response;
        }

        #endregion
    }
}