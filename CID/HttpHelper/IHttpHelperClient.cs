using CID.HttpHelper.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CID.HttpHelper
{
    public interface IHttpHelperClient
    {
        Task<HttpResponse> MakeHttpGetRequest(string requestUri, Dictionary<string, string> headers, string clientName);
        Task<HttpResponse> MakeHttpPostRequest(string requestUri, Dictionary<string, string> headers, string content, string clientName);

        string GetBaseAddress();
    }
}
