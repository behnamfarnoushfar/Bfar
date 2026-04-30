using Bfar.XCutting.Abstractions.Adapters;
using Bfar.XCutting.Abstractions.SharedKernel;
using Bfar.XCutting.Foundation.Diagnostics;
using Bfar.XCutting.Foundation.Extensions;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Bfar.XCutting.Foundation.Adapters
{
    public sealed class MSSQLDapperAdapter : IRepositoryService
    {
        private const string commanetprefix = "MSSQLDapperConnectionAdapter=>";
        private readonly ICircuitBreakerAdapter? circuitBreaker;
        private readonly IJsonParserAdapter json;
        private SqlConnection? connection;
        private string? command;
        private SqlTransaction? transaction;

        public MSSQLDapperAdapter(ICircuitBreakerAdapter? circuitBreaker, IJsonParserAdapter json)
        {
            this.circuitBreaker = circuitBreaker;
            this.json = json;
            connection = new SqlConnection(circuitBreaker?.GetCurrentConnection());
        }
        public MSSQLDapperAdapter(string connection)
        {
            this.connection = new SqlConnection(connection);
        }
        public string? ConnectionString => circuitBreaker?.GetCurrentConnection();
        public int TimeOut => circuitBreaker?.GetCurrentTimeout() ?? 30;
        public IRepositoryService BuildCommand(string command)
        {
            this.command = command;
            return this;
        }

        public IRepositoryService Connect()
        {
            try
            {

                if (circuitBreaker?.ShouldAnswer() ?? true)
                    connection!.Open();
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, connection?.ConnectionString!);
                throw;
            }
            return this;
        }

        public IRepositoryService Disconnect()
        {
            try
            {

                if (circuitBreaker?.ShouldAnswer() ?? true)
                    connection!.Close();
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, connection!);
                throw;
            }

            return this;
        }

        public void Dispose()
        {
            if (connection!.State != ConnectionState.Closed)
            {
                connection.Close();
            }
            connection.Dispose();
        }
        public void BulkInsert<T>(in IEnumerable<T> model, int timeout = 30)
        {
            if (model == null || model.Count() == 0)
                return;
            try
            {
                if (circuitBreaker?.ShouldAnswer() ?? true)
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        sqlBulkCopy.BatchSize = model.Count();
                        sqlBulkCopy.BulkCopyTimeout = timeout;
                        if (connection!.State != ConnectionState.Open)
                            connection.Open();
                        DataTable dt = model.ToDataTable();
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (dt.Columns[i].ColumnName[0] == '_')
                                continue;
                            sqlBulkCopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                        }
                        sqlBulkCopy.BulkCopyTimeout = TimeOut;
                        sqlBulkCopy.DestinationTableName = command;
                        sqlBulkCopy.WriteToServer(dt);
                        sqlBulkCopy.ColumnMappings.Clear();
                        GC.Collect(2, GCCollectionMode.Aggressive, true, true);
                    }
                }
                else
                    ThrowHelper.CircuitIsOpen(connection?.ConnectionString, commanetprefix + command);
                return;
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, model, connection?.ConnectionString!);
                throw;
            }
            finally
            {
                if (transaction is null)
                    connection!.Close();
            }
        }

        public IEnumerable<TOutput>? Execute<TInput, TOutput>(in TInput? model) where TInput : class where TOutput : class
        {
            try
            {
                if (circuitBreaker?.ShouldAnswer() ?? true)
                {
                    if (connection!.State != ConnectionState.Open)
                        connection.Open();
                    var result = connection.Query<TOutput>(command!, model, transaction, true, TimeOut, CommandType.StoredProcedure)?.ToList();
                    return result;
                }
                else
                    ThrowHelper.CircuitIsOpen(connection?.ConnectionString, commanetprefix + command);
                return null;
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, model!, connection?.ConnectionString!);
                throw;
            }
            finally
            {
                connection!.Close();
            }

        }
        public IEnumerable<TOutput>? Execute<TOutput>() where TOutput : class
        {
            try
            {
                if (circuitBreaker?.ShouldAnswer() ?? true)
                    return connection!.Query<TOutput>(command!, null, transaction, true, TimeOut, CommandType.StoredProcedure)?.ToList();
                ThrowHelper.CircuitIsOpen(connection?.ConnectionString, commanetprefix + command);
                return null;
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, connection?.ConnectionString!);
                throw;
            }
        }
        public void Execute<TInput>(in TInput? model) where TInput : class
        {
            try
            {
                if (circuitBreaker?.ShouldAnswer() ?? true)
                {
                    connection!.Query(command!, model, transaction, true, TimeOut, CommandType.StoredProcedure)?.ToList();
                }
                else
                    ThrowHelper.CircuitIsOpen(connection?.ConnectionString, commanetprefix + command);
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, model!, connection?.ConnectionString!);
                throw;
            }
        }
        public IEnumerable<TOutput>? ExecuteAdHoc<TInput, TOutput>(in TInput? model) where TInput : class where TOutput : class
        {
            try
            {
                if (circuitBreaker?.ShouldAnswer() ?? true)
                    return connection!.Query<TOutput>(command!, model, transaction, true, TimeOut, CommandType.Text)?.ToList();
                else
                    ThrowHelper.CircuitIsOpen(connection?.ConnectionString, commanetprefix + command);
                return null;
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, model!, connection?.ConnectionString!);
                throw;
            }

        }
        public IEnumerable<TOutput>? ExecuteAdHoc<TOutput>() where TOutput : class
        {
            try
            {
                if (circuitBreaker?.ShouldAnswer() ?? true)
                    return connection!.Query<TOutput>(command!, null, transaction, true, TimeOut, CommandType.Text)?.ToList();
                ThrowHelper.CircuitIsOpen(connection?.ConnectionString, commanetprefix + command);
                return null;
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, connection?.ConnectionString!);
                throw;
            }
        }
        public void Execute()
        {
            try
            {
                if (circuitBreaker?.ShouldAnswer() ?? true)
                    connection!.Query(command!, null, transaction, true, TimeOut, CommandType.Text)?.ToList();
                else
                    ThrowHelper.CircuitIsOpen(connection?.ConnectionString, commanetprefix + command);
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, connection?.ConnectionString!);
                throw;
            }
        }
        public IEnumerable<TOutput>? ExecuteJson<TInput, TOutput>(in TInput? model) where TInput : class where TOutput : class
        {
            try
            {
                if (circuitBreaker?.ShouldAnswer() ?? true)
                    return connection!.Query<TOutput>(command!, new { model = json.Serialize(model) }, transaction, true, TimeOut, CommandType.StoredProcedure)?.ToList();
                ThrowHelper.CircuitIsOpen(connection?.ConnectionString, commanetprefix + command);
                return null;
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, model!, connection?.ConnectionString!);
                throw;
            }
        }

        public IRepositoryService BeginTransaction()
        {
            try
            {
                if (circuitBreaker?.ShouldAnswer() ?? true)
                {
                    if (connection!.State != ConnectionState.Open)
                        connection.Open();
                    transaction = connection.BeginTransaction();
                }
                else
                    ThrowHelper.CircuitIsOpen(connection?.ConnectionString, commanetprefix + command);
                return this;
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, connection?.ConnectionString!);
                throw;
            }

        }

        public IRepositoryService CommitTransaction()
        {
            transaction?.Commit();
            return this;
        }

        public IRepositoryService RollbackTransaction()
        {
            transaction?.Rollback();
            return this;
        }

        public IRepositoryService SetUsername(string username)
        {
            throw new NotImplementedException();
        }

        public IRepositoryService SetPassword(string password)
        {
            throw new NotImplementedException();
        }

        public IRepositoryService SetAddress(string address)
        {
            throw new NotImplementedException();
        }

        public IRepositoryService SetPort(int port)
        {
            throw new NotImplementedException();
        }

        public IRepositoryService SetSessionLevel(int level)
        {
            throw new NotImplementedException();
        }

        public IRepositoryService SetAuthenticationLevel(int level)
        {
            throw new NotImplementedException();
        }

        public IRepositoryService SetFilters(string filter)
        {
            throw new NotImplementedException();
        }

        public async Task<IRepositoryService> ConnectAsync(CancellationToken cancellationToken = default)
        {
            try
            {

                if (circuitBreaker?.ShouldAnswer() ?? true)
                    await connection!.OpenAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, connection?.ConnectionString!);
                throw;
            }
            return this;
        }

        public async Task<IRepositoryService> DisconnectAsync(CancellationToken cancellationToken = default)
        {
            try
            {

                if (circuitBreaker?.ShouldAnswer() ?? true)
                    await connection!.CloseAsync();
                return this;
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, connection!);
                throw;
            }
        }

        public async Task<IEnumerable<TOutput>?> ExecuteAsync<TInput, TOutput>(TInput? model, CancellationToken cancellationToken = default)
            where TInput : class
            where TOutput : class
        {
            try
            {
                if (circuitBreaker?.ShouldAnswer() ?? true)
                {
                    if (connection!.State != ConnectionState.Open)
                        connection.Open();
                    var result = await connection.QueryAsync<TOutput>(command!, model, transaction, TimeOut, CommandType.StoredProcedure);
                    return result;
                }
                else
                    ThrowHelper.CircuitIsOpen(connection?.ConnectionString, commanetprefix + command);
                return null;
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, model!, connection?.ConnectionString!);
                throw;
            }
            finally
            {
                await connection!.CloseAsync();
            }
        }

        public async Task<IEnumerable<TOutput>?> ExecuteAsync<TOutput>(CancellationToken cancellationToken = default) where TOutput : class
        {
            try
            {
                if (circuitBreaker?.ShouldAnswer() ?? true)
                    return await connection!.QueryAsync<TOutput>(command!, null, transaction, TimeOut, CommandType.StoredProcedure);
                ThrowHelper.CircuitIsOpen(connection?.ConnectionString, commanetprefix + command);
                return null;
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, connection?.ConnectionString!);
                throw;
            }
        }

        public async Task ExecuteAsync<TInput>(TInput? model, CancellationToken cancellationToken = default) where TInput : class
        {
            try
            {
                if (circuitBreaker?.ShouldAnswer() ?? true)
                {
                    await connection!.QueryAsync(command!, model, transaction, TimeOut, CommandType.StoredProcedure);
                }
                else
                    ThrowHelper.CircuitIsOpen(connection?.ConnectionString, commanetprefix + command);
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, model!, connection?.ConnectionString!);
                throw;
            }
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (circuitBreaker?.ShouldAnswer() ?? true)
                    await connection!.QueryAsync(command!, null, transaction, TimeOut, CommandType.Text);
                else
                    ThrowHelper.CircuitIsOpen(connection?.ConnectionString, commanetprefix + command);
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, connection?.ConnectionString!);
                throw;
            }
        }


        public async Task<IEnumerable<TOutput>?> ExecuteAdHocAsync<TInput, TOutput>(TInput? model, CancellationToken cancellationToken = default) where TInput : class where TOutput : class
        {
            try
            {
                if (circuitBreaker?.ShouldAnswer() ?? true)
                    return await connection!.QueryAsync<TOutput>(command!, model, transaction, TimeOut, CommandType.Text);
                else
                    ThrowHelper.CircuitIsOpen(connection?.ConnectionString, commanetprefix + command);
                return null;
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, model!, connection?.ConnectionString!);
                throw;
            }
        }

        public async Task<IEnumerable<TOutput>?> ExecuteAdHocAsync<TOutput>(CancellationToken cancellationToken = default) where TOutput : class
        {
            try
            {
                if (circuitBreaker?.ShouldAnswer() ?? true)
                    return await connection!.QueryAsync<TOutput>(command!, null, transaction, TimeOut, CommandType.Text);
                ThrowHelper.CircuitIsOpen(connection?.ConnectionString, commanetprefix + command);
                return null;
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, connection?.ConnectionString!);
                throw;
            }
        }

        public async Task<IEnumerable<TOutput>?> ExecuteJsonAsync<TInput, TOutput>(TInput? model, CancellationToken cancellationToken = default) where TInput : class where TOutput : class
        {
            try
            {
                if (circuitBreaker?.ShouldAnswer() ?? true)
                    return await connection!.QueryAsync<TOutput>(command!, new { model = json.Serialize(model) }, transaction, TimeOut, CommandType.StoredProcedure);
                ThrowHelper.CircuitIsOpen(connection?.ConnectionString, commanetprefix + command);
                return null;
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, model!, connection?.ConnectionString!);
                throw;
            }
        }

        public async Task BulkInsertAsync<T>(IEnumerable<T> model, int size = 50000, int timeout = 0, CancellationToken cancellationToken = default)
        {
            if (model == null || model.Count() == 0)
                return;
            try
            {
                if (circuitBreaker?.ShouldAnswer() ?? true)
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        sqlBulkCopy.BatchSize = size;
                        sqlBulkCopy.BulkCopyTimeout = timeout;
                        if (connection!.State != ConnectionState.Open)
                            await connection.OpenAsync();
                        DataTable dt = model.ToDataTable();
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (dt.Columns[i].ColumnName[0] == '_')
                                continue;
                            sqlBulkCopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                        }
                        sqlBulkCopy.BulkCopyTimeout = TimeOut;
                        sqlBulkCopy.DestinationTableName = command;
                        await sqlBulkCopy.WriteToServerAsync(dt);
                        sqlBulkCopy.ColumnMappings.Clear();
                        GC.Collect(2, GCCollectionMode.Aggressive, true, true);
                    }
                }
                else
                    ThrowHelper.CircuitIsOpen(connection?.ConnectionString, commanetprefix + command);
                return;
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, model, connection?.ConnectionString!);
                throw;
            }
            finally
            {
                if (transaction is null)
                    await connection!.CloseAsync();
            }
        }

        public IRepositoryService SetConnectionString(string str)
        {
            connection?.Close();
            connection?.Dispose();
            this.connection = new SqlConnection(str);
            return this;
        }

        public IAsyncEnumerable<IEnumerable<TOutput>>? ExecuteStream<TInput, TOutput>(TInput? model, int bufferSize, CancellationToken cancellationToken = default)
            where TInput : class
            where TOutput : class
        {
            throw new NotImplementedException();
        }
    }
}
