using Bfar.XCutting.Abstractions.Adapters;
using Bfar.XCutting.Abstractions.Entities.Dtos;
using Bfar.XCutting.Abstractions.Entities.Models;
using Bfar.XCutting.Abstractions.SharedKernel;
using Bfar.XCutting.Foundation.Constants;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Serializers.Json;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace Bfar.XCutting.Foundation.Adapters
{
    public sealed class RestSharpChannelAdapter : IRestClientService
    {
        private RestClientOptions options;
        private readonly string? authorization;
        private readonly string? appid;
        private readonly ILogger? logger;
        private readonly IJsonParserAdapter json;
        private RestClient client;
        public RestSharpChannelAdapter(ILoggerFactory loggerFactory, IJsonParserAdapter json, string host = "", int timeout = -1, string? authorization = null, string? appid = null)
        {

            this.authorization = authorization;
            this.appid = appid;
            this.logger = loggerFactory.CreateLogger<RestSharpChannelAdapter>();
            this.json = json;

        }

        public async Task<GeneralResponse<TResponse?>> ExecuteJsonAsync<TResponse, TBody>(string host,
            string resource = StringConstants.Slash,
            string method = StringConstants.GET,
            TBody? body = null,
            List<NameValueModel> headers = null,
            List<NameValueModel> files = null,
            List<NameValueModel> formParameters = null,
            string? proxyAddress = null,
            string? proxyUsername = null,
            string? proxyPassword = null,
            bool alloInvalidSSL = false,
            CancellationToken cancellationToken = default) where TBody : class
        {
            WebProxy? proxy = null;
            if (!string.IsNullOrEmpty(proxyAddress))
            {
                proxy = new WebProxy(proxyAddress)
                {
                    Credentials = new NetworkCredential(proxyUsername, proxyPassword)
                };
            }

            options = new RestClientOptions(host)
            {
                Proxy = proxy
            };
            if (alloInvalidSSL)
            {
                options.RemoteCertificateValidationCallback = (_, _, _, _) => true;
            }
            client = new RestClient(options, configureSerialization: s => s.UseSystemTextJson(new JsonSerializerOptions
            {
                PropertyNamingPolicy = null
            }));
            var request = new RestRequest(resource, BuildMethod(method));
            if (!string.IsNullOrEmpty(authorization))
                request.AddHeader(StringConstants.Authorization, authorization);
            if (!string.IsNullOrEmpty(appid))
                request.AddHeader(StringConstants.AppId, appid);
            if (headers is not null && headers.Count > 0)
                for (int i = 0; i < headers.Count; i++)
                {
                    request.AddHeader(headers[i].Name, headers[i].Value!);
                }
            if (body is not null)
                request.AddJsonBody(body);
            Stopwatch sw = Stopwatch.StartNew();
            var response = await client.ExecuteAsync<TResponse>(request);
            var result = new GeneralResponse<TResponse?>()
            {
                ResultCode = (int)response.StatusCode,
                Duration = (int)sw.Elapsed.TotalMilliseconds,
                Content = response.Data,
                UID = response.Headers?.Where(x => x.Name?.ToLower() == StringConstants.RequestId.ToLower())?.FirstOrDefault()?.Value,
                Raw = response?.Content,
                IsSuccessFull = response?.IsSuccessful ?? false
            };
            result.Raw = response.Content;
            if (!string.IsNullOrEmpty(response.ErrorMessage) || !response.IsSuccessful || (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created) || response.ContentLength == 0)
                logger?.LogError(response.ErrorException,
                        $"{result.Duration}ms {method} {host} {resource} {(!string.IsNullOrEmpty(proxyAddress) ? $"using proxy :{proxyAddress}" : string.Empty)} \n{(body is not null ? json.Serialize(body) : null)}\n" +
                        $"\n{result.UID}{response.StatusCode}{result.Raw}\n{response.ErrorException}");
            else
                logger?.LogInformation($"{result.Duration} {method} {host} {resource} {(!string.IsNullOrEmpty(proxyAddress) ? $"using proxy :{proxyAddress}" : string.Empty)} \n{(body is not null ? json.Serialize(body) : null)}\n" +
                        $"\n{result.UID}{response.StatusCode}{result.Raw}{response.ErrorException}");

            if (logger is null)
            {
                if (!string.IsNullOrEmpty(response.ErrorMessage) || !response.IsSuccessful || (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created) || response.ContentLength == 0)
                    Console.WriteLine($"{result.Duration} {method} {host} {resource} {(!string.IsNullOrEmpty(proxyAddress) ? $"using proxy :{proxyAddress}" : string.Empty)} \n{(body is not null ? json.Serialize(body) : null)}\n" +
                        $"\n{result.UID}{response.StatusCode} {host} {response.Content}\n{response.ErrorException}");
                else Console.WriteLine($"{result.Duration} {method} {host} {resource} {(!string.IsNullOrEmpty(proxyAddress) ? $"using proxy :{proxyAddress}" : string.Empty)} \n{(body is not null ? json.Serialize(body) : null)}\n" +
                        $"\n{result.UID}{response.StatusCode} {host} {response.Content}{response.ErrorException}");

            }
            return result;
        }

        private Method BuildMethod(string method)
        {
            return method.ToUpper() switch
            {
                StringConstants.GET => Method.Get,
                StringConstants.POST => Method.Post,
                StringConstants.PATCH => Method.Patch,
                StringConstants.DELETE => Method.Delete,
                StringConstants.PUT => Method.Put,
                StringConstants.MERGE => Method.Merge,
                StringConstants.SEARCH => Method.Search,
                StringConstants.COPY => Method.Copy,
                StringConstants.HEAD => Method.Head,
                _ => Method.Get
            };
        }
    }
}
