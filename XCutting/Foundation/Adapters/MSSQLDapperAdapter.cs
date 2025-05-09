using Bfar.XCutting.Abstractions.Adapters;
using Bfar.XCutting.Abstractions.Dtos;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Bfar.XCutting.Foundation.Adapters
{
    public sealed class MSSQLDapperAdapter : IDatabaseAdapter
    {
        private IDbConnection _dbConnection;
        private string? _connectionString;
        public IDatabaseAdapter SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
            _dbConnection = new SqlConnection(_connectionString);
            return this;
        }

        public async Task<TResult?> ExecuteFirstAsync<TParameters, TResult>(
            string query, TParameters parameters, bool isStoreProcedure,
            IDbTransaction? transaction = null, int timeout = 30)
        {
            var commandType = isStoreProcedure ? CommandType.StoredProcedure : CommandType.Text;
            return await _dbConnection.QueryFirstOrDefaultAsync<TResult>(
                query, parameters, transaction, timeout, commandType);
        }
        public async Task<IEnumerable<TResult>?> ExecuteAsync<TParameters, TResult>(
            string query, TParameters parameters, bool isStoreProcedure,
            IDbTransaction? transaction = null, int timeout = 30)
        {
            var commandType = isStoreProcedure ? CommandType.StoredProcedure : CommandType.Text;
            return await _dbConnection.QueryAsync<TResult>(
                query, parameters, transaction, timeout, commandType);
        }
        public async Task<DbGeneralResponse> ExecuteAsync<TParameters>(
            string query, TParameters parameters, bool isStoreProcedure,
            IDbTransaction? transaction = null, int timeout = 30)
        {
            var commandType = isStoreProcedure ? CommandType.StoredProcedure : CommandType.Text;
            return await _dbConnection.QueryFirstOrDefaultAsync<DbGeneralResponse>(
                query, parameters, transaction, timeout, commandType) ?? new DbGeneralResponse { Code = 500, Msg = "Unexpected database error" };
        }

        public async Task<DbGeneralResponse> BulkInsertAsync<TParameters>(string table, List<TParameters> parameters, IDbTransaction? transaction = null, int timeout = 30)
        {
            DataTable? dt = null;
            using (var sqlBulkCopy = new SqlBulkCopy((SqlConnection)_dbConnection, SqlBulkCopyOptions.Default, (SqlTransaction?)transaction))
            {
                sqlBulkCopy.DestinationTableName = table;
                sqlBulkCopy.BatchSize = parameters.Count;

                dt = new DataTable();
                var props = typeof(TParameters).GetProperties();
                foreach (var prop in props)
                    dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

                foreach (var item in parameters)
                {
                    var row = dt.NewRow();
                    foreach (var prop in props)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    dt.Rows.Add(row);
                }

                await sqlBulkCopy.WriteToServerAsync(dt);
                props = null;
            }
            dt?.Clear();
            GC.Collect(5, GCCollectionMode.Aggressive, true, true);
            return new DbGeneralResponse { Code = 200, Msg = "Bulk insert completed successfully" };
        }

        
    }
}
