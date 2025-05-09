using Bfar.XCutting.Abstractions.Dtos;
using System.Data;

namespace Bfar.XCutting.Abstractions.Adapters
{
    public interface IDatabaseAdapter
    {
        IDatabaseAdapter SetConnectionString(string connectionString);
        Task<TResult?> ExecuteFirstAsync<TParameters,TResult>(string query, TParameters parameters,bool isStoreProcedure,IDbTransaction? transaction=null,int timeout=0);
        Task<IEnumerable<TResult>?> ExecuteAsync<TParameters,TResult>(string query, TParameters parameters,bool isStoreProcedure,IDbTransaction? transaction=null,int timeout=0);
        Task<DbGeneralResponse> ExecuteAsync<TParameters>(string query, TParameters parameters,bool isStoreProcedure, IDbTransaction? transaction = null, int timeout = 0);
        Task<DbGeneralResponse> BulkInsertAsync<TParameters>(string table, List<TParameters> parameters, IDbTransaction? transaction = null, int timeout = 0);
    }

}
