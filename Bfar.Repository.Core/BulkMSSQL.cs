using Bfar.Extensions.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Bfar.Repository.Core
{
    public class BulkMSSQL : IBulkRepository
    {
        private DbProviderFactory _provider;
        private string connectionString;
        private string providerString;
        private IDbConnection _connection;
        private IDbTransaction _currentTransaction;
        public static BulkMSSQL Factory { get { return new BulkMSSQL(); } }
        public static BulkMSSQL Connect(string connectionString)
        {
            return new BulkMSSQL(connectionString);
        }
        public string AdHocCommand { get; set; }
        public int MinTimeOut { get; set; }

        private string ConnectionString { get { return connectionString; } set { connectionString = value; } }
        private string ProviderString { get { return providerString; } set { providerString = value; } }
        private DbProviderFactory DBFactory
        {
            get
            {
                return _provider;
            }
        }

        public IDbConnection Connection
        {
            get
            {
                var connection = DBFactory.CreateConnection();
                connection.ConnectionString = connectionString;
                //connection.ConnectionString = string.IsNullOrEmpty(connectionString) ? ConfigurationManager.ConnectionStrings["SourceData"].ConnectionString : connectionString;
                return connection;
            }
        }
        private IDbTransaction Transaction { get { return _currentTransaction; } }

        public IDbTransaction BeginTransaction()
        {
            try
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Rollback();
                }
                if (_connection != null && _connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                    _connection = null;
                }
                _connection = Connection;
                _connection.Open();
                _currentTransaction = _connection.BeginTransaction();
                return _currentTransaction;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public bool RollbackTransaction()
        {
            try
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Rollback();
                }
                if (_connection != null && _connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                    _connection = null;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (_connection != null && _connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                    _connection = null;
                }
            }
        }
        public bool CommitTransaction()
        {
            try
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Commit();
                }
                if (_connection != null && _connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                    _connection = null;
                }
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (_connection != null && _connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                    _connection = null;
                }
            }
        }
        public BulkMSSQL()
        {
            MinTimeOut = 120;
            providerString = ConfigurationManager.ConnectionStrings["SourceData"].ProviderName;
            connectionString = ConfigurationManager.ConnectionStrings["SourceData"].ConnectionString;
            _provider = DbProviderFactories.GetFactory(providerString);
        }
        public BulkMSSQL(string connectionSetting)
        {
            MinTimeOut = 120;
            providerString = ConfigurationManager.ConnectionStrings[connectionSetting].ProviderName;
            connectionString = ConfigurationManager.ConnectionStrings[connectionSetting].ConnectionString;
            _provider = DbProviderFactories.GetFactory(providerString);
        }
        public IBulkRepository Build(string Command)
        {
            AdHocCommand = Command;
            return this;
        }

        public void BulkUpdate(string[] Columns, object Values, string UpdateExpression)
        {
            try
            {
                string tbl = $"TmpTableBulkUpdate{Guid.NewGuid().ToString()}";
                DataTable dt = new DataTable(tbl);
                dt = ConvertToTransportDataTable(Columns, Values);
                BeginTransaction();
                using (var command = _connection.CreateCommand())
                {
                    try
                    {

                        _connection.Open();
                        //Creating temp table on database
                        command.CommandText = $"CREATE TABLE #{tbl}()";
                        command.ExecuteNonQuery();

                        //Bulk insert into temp table
                        using (SqlBulkCopy bulkcopy = new SqlBulkCopy((SqlConnection)_connection, SqlBulkCopyOptions.Default, (SqlTransaction)_currentTransaction))
                        {
                            bulkcopy.BulkCopyTimeout = 660;
                            bulkcopy.DestinationTableName = $"#{tbl}";
                            bulkcopy.WriteToServer(dt);
                            bulkcopy.Close();
                        }

                        // Updating destination table, and dropping temp table
                        command.CommandTimeout = 300;
                        command.CommandText = $"UPDATE ReceiptDetails SET TransportType=@type FROM {UpdateExpression} INNER JOIN #{tbl} tnp ON tmp.consid=receipts.ConsignmentNumber; DROP TABLE #{tbl};";
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                        CommitTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                throw;
            }
        }
        private DataTable ConvertToTransportDataTable(string[] Columns, object Values)
        {
            throw new NotImplementedException();
        }
        public void BulkInsert<T>(IList<T> Data, string Name) where T : class
        {
            var tbl = Data.ToDataTable(Name);
            using (SqlConnection Con = (SqlConnection)Connection)
            {
                using (SqlBulkCopy bulk = new SqlBulkCopy(Con))
                {
                    Con.Open();
                    bulk.DestinationTableName = Name;
                    try
                    {
                        bulk.WriteToServer(tbl);
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                    Con.Close();
                }
            }
        }
        public IBulkRepository SetTimeOut(int seconds)
        {
            MinTimeOut = seconds * 1000;
            return this;
        }
    }
}
