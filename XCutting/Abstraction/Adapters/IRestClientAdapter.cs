using Bfar.XCutting.Abstractions.Dtos;
using Bfar.XCutting.Abstractions.Models;

namespace Bfar.XCutting.Abstractions.Adapters
{
    public interface IRestClientAdapter
    {
        Task<GeneralResponse<TResponse?>> ExecuteJsonAsync<TResponse, TBody>(string host, string resource = "/", string method = "GET", TBody? body = null, List<NameValueModel> headers = null, List<NameValueModel> files = null, List<NameValueModel> formParameters = null, CancellationToken cancellationToken = default) where TBody : class;
    }
}
