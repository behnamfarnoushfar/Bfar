using Bfar.Extensions.Core;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Bfar.Repository.Core
{
    public class ChainMSSQL : ITransactionalRepository
    {
        private DbProviderFactory _provider;
        private string connectionString;
        private string providerString;
        private IDbConnection _connection;
        private IDbTransaction _currentTransaction;
        public static ChainMSSQL Factory { get { return new ChainMSSQL(); } }
        public static ChainMSSQL Connect(string connectionString)
        {
            return new ChainMSSQL(connectionString);
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
        public ChainMSSQL()
        {
            MinTimeOut = 120;
            providerString = ConfigurationManager.ConnectionStrings["SourceData"].ProviderName;
            connectionString = ConfigurationManager.ConnectionStrings["SourceData"].ConnectionString;
            _provider = DbProviderFactories.GetFactory(providerString);
        }
        public ChainMSSQL(string connectionSetting)
        {
            MinTimeOut = 120;
            providerString = ConfigurationManager.ConnectionStrings[connectionSetting].ProviderName;
            connectionString = ConfigurationManager.ConnectionStrings[connectionSetting].ConnectionString;
            _provider = DbProviderFactories.GetFactory(providerString);
        }

        public long ChainAdd<T>(T model) where T : class
        {
            long result = 0;
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            result = _connection.Insert(model, _currentTransaction);
            return result;
        }
        public bool ChainUpdate<T>(T model) where T : class
        {
            bool result;
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            result = _connection.Update(model, _currentTransaction);
            return result;
        }
        public ITransactionalRepository Build(string Command)
        {
            AdHocCommand = Command;
            return this;
        }


        public IEnumerable<K> GetAll<K>() where K : class
        {
            using (IDbConnection Con = Connection)
            {
                return Con.GetAll<K>();
            }
        }

        public List<T> ExecuteChain<T>(object Parameters = null)
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            var result = _connection.Query<T>(AdHocCommand, Parameters, _currentTransaction, false, MinTimeOut, CommandType.StoredProcedure).ToList();
            return result;
        }
        public List<K> ExecuteAdHocChain<K>(object Parameters = null) where K : class
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            var result = _connection.Query<K>(AdHocCommand, Parameters, _currentTransaction, false, MinTimeOut, CommandType.Text).ToList();
            return result;
        }


        public ITransactionalRepository SetTimeOut(int seconds)
        {
            MinTimeOut = seconds * 1000;
            return this;
        }
    }
}
