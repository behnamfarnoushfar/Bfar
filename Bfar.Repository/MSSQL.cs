using Bfar.Extensions;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bfar.Repository
{
    public class MSSQL : IRepository
    {
        private DbProviderFactory _provider;
        private string connectionString;
        private string providerString;
        private IDbConnection _connection;
        private IDbTransaction _currentTransaction;
        public static MSSQL Factory { get { return new MSSQL(); } } 
        public static MSSQL Connect(string connectionString)
        {
            return new MSSQL(connectionString);
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
        public MSSQL()
        {
            MinTimeOut = 120;
            providerString = ConfigurationManager.ConnectionStrings["SourceData"].ProviderName;
            connectionString = ConfigurationManager.ConnectionStrings["SourceData"].ConnectionString;
            _provider = DbProviderFactories.GetFactory(providerString);
        }
        public MSSQL(string connectionSetting)
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
        public long Add<K>(K model) where K : class
        {
            long result = 0;
            using (IDbConnection Con = Connection)
            {
                Con.Open();
                result = Con.Insert(model);
            }
            return result;
        }


        public IRepository Build(string Command)
        {
            AdHocCommand = Command;
            return this;
        }

        public void Execute(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                Con.Open();
                var t = Con.Query(AdHocCommand, Parameters, null, false, MinTimeOut, CommandType.StoredProcedure).SingleOrDefault();
                Con.Close();
            }
        }

        public List<K> Execute<K>(object Parameters) where K : class
        {
            using (IDbConnection Con = Connection)
            {
                Con.Open();
                var t = Con.Query<K>(AdHocCommand, Parameters, null, false, 30, CommandType.StoredProcedure);
                Con.Close();
                return t?.ToList();
            }
        }

        public List<K> ExecuteAdHoc<K>(object Parameters = null) where K : class
        {
            using (IDbConnection Con = Connection)
            {
                Con.Open();
                var t = Con.Query<K>(AdHocCommand, Parameters, null, false, MinTimeOut, CommandType.Text);
                return t?.ToList();
            }
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
        public Tuple<List<A>, List<B>> ExecuteMARS<A, B>(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                var a = multi.Read<A>();
                var b = multi.Read<B>();
                return new Tuple<List<A>, List<B>>(a.ToList(), b.ToList());
            }
        }
        public Tuple<List<A>, List<B>, List<C>> ExecuteMARS<A, B, C>(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                var a = multi.Read<A>();
                var b = multi.Read<B>();
                var c = multi.Read<C>();
                return new Tuple<List<A>, List<B>, List<C>>(a.ToList(), b.ToList(), c.ToList());
            }
        }
        public Tuple<List<A>, List<B>, List<C>, List<D>> ExecuteMARS<A, B, C, D>(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                var a = multi.Read<A>();
                var b = multi.Read<B>();
                var c = multi.Read<C>();
                var d = multi.Read<D>();
                return new Tuple<List<A>, List<B>, List<C>, List<D>>(a.ToList(), b.ToList(), c.ToList(), d.ToList());
            }
        }

        public Tuple<List<A>, List<B>, List<C>, List<D>, List<E>> ExecuteMARS<A, B, C, D, E>(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                var a = multi.Read<A>();
                var b = multi.Read<B>();
                var c = multi.Read<C>();
                var d = multi.Read<D>();
                var e = multi.Read<E>();
                return new Tuple<List<A>, List<B>, List<C>, List<D>, List<E>>(a.ToList(), b.ToList(), c.ToList(), d.ToList(), e.ToList());
            }
        }

        public Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>> ExecuteMARS<A, B, C, D, E, F>(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                var a = multi.Read<A>();
                var b = multi.Read<B>();
                var c = multi.Read<C>();
                var d = multi.Read<D>();
                var e = multi.Read<E>();
                var f = multi.Read<F>();
                return new Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>>(a.ToList(), b.ToList(), c.ToList(), d.ToList(), e.ToList(), f.ToList());
            }
        }

        public Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>> ExecuteMARS<A, B, C, D, E, F, G>(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                var a = multi.Read<A>();
                var b = multi.Read<B>();
                var c = multi.Read<C>();
                var d = multi.Read<D>();
                var e = multi.Read<E>();
                var f = multi.Read<F>();
                var g = multi.Read<G>();
                return new Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>>(a.ToList(), b.ToList(), c.ToList(), d.ToList(), e.ToList(), f.ToList(), g.ToList());
            }
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
                        command.CommandText = $"UPDATE tbl SET A=@type FROM {UpdateExpression} INNER JOIN #{tbl} tnp ON tmp.id=A.id; DROP TABLE #{tbl};";
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

                        ///throw;
                    }

                    Con.Close();
                }
            }
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

        public List<K> ExecuteJson<T, K>(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
