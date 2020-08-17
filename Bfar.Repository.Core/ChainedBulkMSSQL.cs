using Bfar.Extensions.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Bfar.Repository.Core
{
    public class ChainedBulkMSSQL : ITransactionalBulkRepository
    {
        private const string providerString = "System.Data.SqlClient";
        private readonly string connectionString;
        public static SimpleMSSQL Factory(string server, string databaseName, string userName, string password) { return new SimpleMSSQL(server, databaseName, userName, password); }

        private SqlConnection _connection;
        private IDbTransaction _currentTransaction;
        public static ChainedBulkMSSQL Factory { get { return new ChainedBulkMSSQL(); } }
        public static ChainedBulkMSSQL Connect(string connectionString)
        {
            return new ChainedBulkMSSQL(connectionString);
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
        public ChainedBulkMSSQL()
        {
            MinTimeOut = 120;
            providerString = ConfigurationManager.ConnectionStrings["SourceData"].ProviderName;
            connectionString = ConfigurationManager.ConnectionStrings["SourceData"].ConnectionString;
            _provider = DbProviderFactories.GetFactory(providerString);
        }
        public ChainedBulkMSSQL(string connectionSetting)
        {
            MinTimeOut = 120;
            providerString = ConfigurationManager.ConnectionStrings[connectionSetting].ProviderName;
            connectionString = ConfigurationManager.ConnectionStrings[connectionSetting].ConnectionString;
            _provider = DbProviderFactories.GetFactory(providerString);
        }
        public ITransactionalBulkRepository Build(string Command)
        {
            AdHocCommand = Command;
            return this;
        }
        public void BulkInsertChain<T>(IList<T> Data, string Name) where T : class
        {
            var tbl = Data.ToDataTable(Name);
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            using (SqlBulkCopy bulk = new SqlBulkCopy((SqlConnection)_connection, SqlBulkCopyOptions.Default, (SqlTransaction)Transaction))
            {
                bulk.DestinationTableName = Name;
                bulk.WriteToServer(tbl);
            }
        }
        public ITransactionalBulkRepository SetTimeOut(int seconds)
        {
            MinTimeOut = seconds * 1000;
            return this;
        }
    }
}
