using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace CID.TestRail.Utils
{
    /// <summary>
    /// This class handles the HttpResult with a generic deserializer for the Json from TestRail
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RequestResult<T>
    {
        public HttpStatusCode StatusCode { get; }

        public T Content { get; }

        public Exception ThrownException { get; }

        public string RawJson { get; }

        /// <summary>
        /// DTO for request result returned by TestRail. TestRailJson is loaded on T Content.
        /// </summary>
        /// <param name="statusCode">HTTP Status code of request.</param>
        /// <param name="rawJson">Raw Json string returned from request.</param>
        /// <param name="thrownException">Exception thrown by the called class.</param>
        /// <param name="content">Generic Type that holds the Data Transfer Object of a request.</param>
        public RequestResult(HttpStatusCode statusCode, string rawJson = null, Exception thrownException = null, T content = default(T))
        {
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy { ProcessDictionaryKeys = true }
                },
                Formatting = Formatting.Indented
            };

            if (rawJson != null)
            {
                RawJson = rawJson;

                Content = JsonConvert.DeserializeObject<T>(rawJson);
            }
            else
            {
                Content = content;
            }

            StatusCode = statusCode;
            ThrownException = thrownException;
        }
    }
}
