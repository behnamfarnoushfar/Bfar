using Bfar.XCutting.Abstractions.Adapters;
using Bfar.XCutting.Abstractions.Dtos;
using Bfar.XCutting.Abstractions.Models;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Serializers.Json;
using System.Diagnostics;
using System.Text.Json;

namespace Bfar.XCutting.Foundation.Adapters
{
    public sealed class RestSharpClientAdapter(ILogger logger, IJsonParserAdapter json) : IRestClientAdapter
    {
        private static string line = $"---------------------------------------------------{Environment.NewLine}";
        public async Task<GeneralResponse<TResponse?>> ExecuteJsonAsync<TResponse, TBody>(string host,
            string resource = "/",
            string method = "GET",
            TBody? body = null,
            List<NameValueModel> headers = null,
            List<NameValueModel> files = null,
            List<NameValueModel> formParameters = null,
            CancellationToken cancellationToken = default) where TBody : class
        {
            var options = new RestClientOptions(host)
            {
                MaxTimeout = -1
            };
            var client = new RestClient(options, configureSerialization: s => s.UseSystemTextJson(new JsonSerializerOptions
            {
                PropertyNamingPolicy = null
            }));
            var request = new RestRequest(resource, BuildMethod(method));
          
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
                Duration = (int)sw.Elapsed.TotalSeconds,
                Content = response.Data,
                UID = response.Headers?.Where(x => x.Name?.ToLower() == "uid")?.FirstOrDefault()?.Value
            };
            result.Raw = result.Content is not null ? response.Content : null;
            if (!response.IsSuccessful)
                logger?.LogError(response.ErrorException,
                        $"{line}{result.Duration} {method} {resource} {Environment.NewLine}{(body is not null ? json.Serialize(body) : null)}{Environment.NewLine}" +
                        $"{result.UID}{response.StatusCode}{result.Raw}{line}");
            else
                logger?.LogTrace(
                        $"{line}{result.Duration} {method} {resource} {Environment.NewLine}{(body is not null ? json.Serialize(body) : null)}{Environment.NewLine}" +
                        $"{result.UID}{response.StatusCode}{result.Raw}{line}");
            return result;
        }

        private Method BuildMethod(string method)
        {
            return method.ToUpper() switch
            {
                "GET" => Method.Get,
                "POST" => Method.Post,
                "PATCH" => Method.Patch,
                "DELETE" => Method.Delete,
                "PUT" => Method.Put,
                "MERGE" => Method.Merge,
                "SEARCH" => Method.Search,
                "COPY" => Method.Copy,
                "HEAD" => Method.Head,
                _ => Method.Get
            };
        }
    }
}
