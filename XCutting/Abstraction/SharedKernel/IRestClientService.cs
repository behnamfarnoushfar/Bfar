using Bfar.XCutting.Abstractions.Entities.Dtos;
using Bfar.XCutting.Abstractions.Entities.Models;

namespace Bfar.XCutting.Abstractions.SharedKernel
{
    public interface IRestClientService
    {
        public Task<GeneralResponse<TResponse?>> ExecuteJsonAsync<TResponse, TBody>(string host,
            string resource = "/",
            string method = "GET",
            TBody? body = null,
            List<NameValueModel> headers = null,
            List<NameValueModel> files = null,
            List<NameValueModel> formParameters = null,
            string? proxyAddress = null,
            string? proxyUsername = null,
            string? proxyPassword = null,
            bool allowInvalidSSL = false,
            CancellationToken cancellationToken = default) where TBody : class;
    }
}
