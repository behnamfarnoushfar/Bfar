﻿using Bfar.Extensions.Core;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Bfar.Repository.Core
{
    /// <summary>
    /// Obsolete
    /// </summary>
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
                var t = Con.Query<K>(AdHocCommand, Parameters, null, false, MinTimeOut, CommandType.StoredProcedure);
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
                IEnumerable<A> a = null;
                IEnumerable<B> b = null;
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                a = multi.Read<A>();
                try
                {
                    b = multi.Read<B>();
                }
                catch { }

                return new Tuple<List<A>, List<B>>(a?.ToList(), b?.ToList());
            }
        }
        public Tuple<List<A>, List<B>, List<C>> ExecuteMARS<A, B, C>(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                IEnumerable<A> a = null;
                IEnumerable<B> b = null;
                IEnumerable<C> c = null;
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                a = multi.Read<A>();
                try
                {
                    b = multi.Read<B>();
                }
                catch { }
                try
                {
                    c = multi.Read<C>();
                }
                catch { }
                return new Tuple<List<A>, List<B>, List<C>>(a?.ToList(), b?.ToList(), c?.ToList());
            }
        }
        public Tuple<List<A>, List<B>, List<C>, List<D>> ExecuteMARS<A, B, C, D>(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                IEnumerable<A> a = null;
                IEnumerable<B> b = null;
                IEnumerable<C> c = null;
                IEnumerable<D> d = null;
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                a = multi.Read<A>();
                try
                {
                    b = multi.Read<B>();
                }
                catch (Exception ex)
                { }
                try
                {
                    c = multi.Read<C>();
                }
                catch { }
                try
                {
                    d = multi.Read<D>();
                }
                catch { }
                return new Tuple<List<A>, List<B>, List<C>, List<D>>(a?.ToList(), b?.ToList(), c?.ToList(), d?.ToList());
            }
        }
        public Tuple<List<A>, List<B>, List<C>, List<D>, List<E>> ExecuteMARS<A, B, C, D, E>(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                IEnumerable<A> a = null;
                IEnumerable<B> b = null;
                IEnumerable<C> c = null;
                IEnumerable<D> d = null;
                IEnumerable<E> e = null;
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                a = multi.Read<A>();
                try
                {
                    b = multi.Read<B>();
                }
                catch { }
                try
                {
                    c = multi.Read<C>();
                }
                catch { }
                try
                {
                    d = multi.Read<D>();
                }
                catch { }
                try
                {
                    e = multi.Read<E>();
                }
                catch { }
                return new Tuple<List<A>, List<B>, List<C>, List<D>, List<E>>(a?.ToList(), b?.ToList(), c?.ToList(), d?.ToList(), e?.ToList());
            }
        }
        public Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>> ExecuteMARS<A, B, C, D, E, F>(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                IEnumerable<A> a = null;
                IEnumerable<B> b = null;
                IEnumerable<C> c = null;
                IEnumerable<D> d = null;
                IEnumerable<E> e = null;
                IEnumerable<F> f = null;
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                a = multi.Read<A>();
                try
                {
                    b = multi.Read<B>();
                }
                catch { }
                try
                {
                    c = multi.Read<C>();
                }
                catch { }
                try
                {
                    d = multi.Read<D>();
                }
                catch { }
                try
                {
                    e = multi.Read<E>();
                }
                catch { }
                try
                {
                    f = multi.Read<F>();
                }
                catch { }
                return new Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>>(a?.ToList(), b?.ToList(), c?.ToList(), d?.ToList(), e?.ToList(), f?.ToList());
            }
        }
        public Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>> ExecuteMARS<A, B, C, D, E, F, G>(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                IEnumerable<A> a = null;
                IEnumerable<B> b = null;
                IEnumerable<C> c = null;
                IEnumerable<D> d = null;
                IEnumerable<E> e = null;
                IEnumerable<F> f = null;
                IEnumerable<G> g = null;
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                a = multi.Read<A>();
                try
                {
                    b = multi.Read<B>();
                }
                catch { }
                try
                {
                    c = multi.Read<C>();
                }
                catch { }
                try
                {
                    d = multi.Read<D>();
                }
                catch { }
                try
                {
                    e = multi.Read<E>();
                }
                catch { }
                try
                {
                    f = multi.Read<F>();
                }
                catch { }
                try
                {
                    g = multi.Read<G>();
                }
                catch { }
                return new Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>>(a?.ToList(), b?.ToList(), c?.ToList(), d?.ToList(), e?.ToList(), f?.ToList(), g?.ToList());
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


        public Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>, Tuple<List<H>>> ExecuteMARS<A, B, C, D, E, F, G, H>(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                IEnumerable<A> a = null;
                IEnumerable<B> b = null;
                IEnumerable<C> c = null;
                IEnumerable<D> d = null;
                IEnumerable<E> e = null;
                IEnumerable<F> f = null;
                IEnumerable<G> g = null;
                IEnumerable<H> h = null;
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                a = multi.Read<A>();
                try
                {
                    b = multi.Read<B>();
                }
                catch { }
                try
                {
                    c = multi.Read<C>();
                }
                catch { }
                try
                {
                    d = multi.Read<D>();
                }
                catch { }
                try
                {
                    e = multi.Read<E>();
                }
                catch { }
                try
                {
                    f = multi.Read<F>();
                }
                catch { }
                try
                {
                    g = multi.Read<G>();
                }
                catch { }
                try
                {
                    h = multi.Read<H>();
                }
                catch { }
                return new Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>, Tuple<List<H>>>(a?.ToList(), b?.ToList(), c?.ToList(), d?.ToList(), e?.ToList(), f?.ToList(), g?.ToList(), Tuple.Create(h?.ToList()));
            }
        }
        public Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>, Tuple<List<H>, List<I>>> ExecuteMARS
        <A, B, C, D, E, F, G, H, I>(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                IEnumerable<A> a = null;
                IEnumerable<B> b = null;
                IEnumerable<C> c = null;
                IEnumerable<D> d = null;
                IEnumerable<E> e = null;
                IEnumerable<F> f = null;
                IEnumerable<G> g = null;
                IEnumerable<H> h = null;
                IEnumerable<I> i = null;
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                a = multi.Read<A>();
                try
                {
                    b = multi.Read<B>();
                }
                catch { }
                try
                {
                    c = multi.Read<C>();
                }
                catch { }
                try
                {
                    d = multi.Read<D>();
                }
                catch { }
                try
                {
                    e = multi.Read<E>();
                }
                catch { }
                try
                {
                    f = multi.Read<F>();
                }
                catch { }
                try
                {
                    g = multi.Read<G>();
                }
                catch { }
                try
                {
                    h = multi.Read<H>();
                }
                catch { }
                try
                {
                    i = multi.Read<I>();
                }
                catch { }
                return new Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>,
                    Tuple<List<H>, List<I>>>(
                    a?.ToList(), b?.ToList(), c?.ToList(), d?.ToList(), e?.ToList(), f?.ToList(), g?.ToList(),
                    Tuple.Create(h?.ToList(), i?.ToList()));
            }
        }
        public Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>, Tuple<List<H>, List<I>, List<J>>> ExecuteMARS
        <A, B, C, D, E, F, G, H, I, J>(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                IEnumerable<A> a = null;
                IEnumerable<B> b = null;
                IEnumerable<C> c = null;
                IEnumerable<D> d = null;
                IEnumerable<E> e = null;
                IEnumerable<F> f = null;
                IEnumerable<G> g = null;
                IEnumerable<H> h = null;
                IEnumerable<I> i = null;
                IEnumerable<J> j = null;
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                a = multi.Read<A>();
                try
                {
                    b = multi.Read<B>();
                }
                catch { }
                try
                {
                    c = multi.Read<C>();
                }
                catch { }
                try
                {
                    d = multi.Read<D>();
                }
                catch { }
                try
                {
                    e = multi.Read<E>();
                }
                catch { }
                try
                {
                    f = multi.Read<F>();
                }
                catch { }
                try
                {
                    g = multi.Read<G>();
                }
                catch { }
                try
                {
                    h = multi.Read<H>();
                }
                catch { }
                try
                {
                    i = multi.Read<I>();
                }
                catch { }
                try
                {
                    j = multi.Read<J>();
                }
                catch { }
                return new Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>,
                    Tuple<List<H>, List<I>, List<J>>>(
                    a?.ToList(), b?.ToList(), c?.ToList(), d?.ToList(), e?.ToList(), f?.ToList(), g?.ToList(),
                    Tuple.Create(h?.ToList(), i?.ToList(), j?.ToList()));
            }
        }
        public Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>, Tuple<List<H>, List<I>, List<J>, List<K>>> ExecuteMARS
        <A, B, C, D, E, F, G, H, I, J, K>(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                IEnumerable<A> a = null;
                IEnumerable<B> b = null;
                IEnumerable<C> c = null;
                IEnumerable<D> d = null;
                IEnumerable<E> e = null;
                IEnumerable<F> f = null;
                IEnumerable<G> g = null;
                IEnumerable<H> h = null;
                IEnumerable<I> i = null;
                IEnumerable<J> j = null;
                IEnumerable<K> k = null;
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                a = multi.Read<A>();
                try
                {
                    b = multi.Read<B>();
                }
                catch { }
                try
                {
                    c = multi.Read<C>();
                }
                catch { }
                try
                {
                    d = multi.Read<D>();
                }
                catch { }
                try
                {
                    e = multi.Read<E>();
                }
                catch { }
                try
                {
                    f = multi.Read<F>();
                }
                catch { }
                try
                {
                    g = multi.Read<G>();
                }
                catch { }
                try
                {
                    h = multi.Read<H>();
                }
                catch { }
                try
                {
                    i = multi.Read<I>();
                }
                catch { }
                try
                {
                    j = multi.Read<J>();
                }
                catch { }
                try
                {
                    k = multi.Read<K>();
                }
                catch { }
                return new Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>,
                    Tuple<List<H>, List<I>, List<J>, List<K>>>(
                    a?.ToList(), b?.ToList(), c?.ToList(), d?.ToList(), e?.ToList(), f?.ToList(), g?.ToList(),
                    Tuple.Create(h?.ToList(), i?.ToList(), j?.ToList(), k?.ToList()));
            }
        }
        public Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>, Tuple<List<H>, List<I>, List<J>, List<K>, List<L>>> ExecuteMARS
        <A, B, C, D, E, F, G, H, I, J, K, L>(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                IEnumerable<A> a = null;
                IEnumerable<B> b = null;
                IEnumerable<C> c = null;
                IEnumerable<D> d = null;
                IEnumerable<E> e = null;
                IEnumerable<F> f = null;
                IEnumerable<G> g = null;
                IEnumerable<H> h = null;
                IEnumerable<I> i = null;
                IEnumerable<J> j = null;
                IEnumerable<K> k = null;
                IEnumerable<L> l = null;
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                a = multi.Read<A>();
                try
                {
                    b = multi.Read<B>();
                }
                catch { }
                try
                {
                    c = multi.Read<C>();
                }
                catch { }
                try
                {
                    d = multi.Read<D>();
                }
                catch { }
                try
                {
                    e = multi.Read<E>();
                }
                catch { }
                try
                {
                    f = multi.Read<F>();
                }
                catch { }
                try
                {
                    g = multi.Read<G>();
                }
                catch { }
                try
                {
                    h = multi.Read<H>();
                }
                catch { }
                try
                {
                    i = multi.Read<I>();
                }
                catch { }
                try
                {
                    j = multi.Read<J>();
                }
                catch { }
                try
                {
                    k = multi.Read<K>();
                }
                catch { }
                try
                {
                    l = multi.Read<L>();
                }
                catch { }
                return new Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>,
                    Tuple<List<H>, List<I>, List<J>, List<K>, List<L>>>(
                    a?.ToList(), b?.ToList(), c?.ToList(), d?.ToList(), e?.ToList(), f?.ToList(), g?.ToList(),
                    Tuple.Create(h?.ToList(), i?.ToList(), j?.ToList(), k?.ToList(), l?.ToList()));
            }
        }
        public Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>, Tuple<List<H>, List<I>, List<J>, List<K>, List<L>, List<M>>> ExecuteMARS
        <A, B, C, D, E, F, G, H, I, J, K, L, M>(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                IEnumerable<A> a = null;
                IEnumerable<B> b = null;
                IEnumerable<C> c = null;
                IEnumerable<D> d = null;
                IEnumerable<E> e = null;
                IEnumerable<F> f = null;
                IEnumerable<G> g = null;
                IEnumerable<H> h = null;
                IEnumerable<I> i = null;
                IEnumerable<J> j = null;
                IEnumerable<K> k = null;
                IEnumerable<L> l = null;
                IEnumerable<M> m = null;
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                a = multi.Read<A>();
                try
                {
                    b = multi.Read<B>();
                }
                catch { }
                try
                {
                    c = multi.Read<C>();
                }
                catch { }
                try
                {
                    d = multi.Read<D>();
                }
                catch { }
                try
                {
                    e = multi.Read<E>();
                }
                catch { }
                try
                {
                    f = multi.Read<F>();
                }
                catch { }
                try
                {
                    g = multi.Read<G>();
                }
                catch { }
                try
                {
                    h = multi.Read<H>();
                }
                catch { }
                try
                {
                    i = multi.Read<I>();
                }
                catch { }
                try
                {
                    j = multi.Read<J>();
                }
                catch { }
                try
                {
                    k = multi.Read<K>();
                }
                catch { }
                try
                {
                    l = multi.Read<L>();
                }
                catch { }
                try
                {
                    m = multi.Read<M>();
                }
                catch { }
                return new Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>,
                    Tuple<List<H>, List<I>, List<J>, List<K>, List<L>, List<M>>>(
                    a?.ToList(), b?.ToList(), c?.ToList(), d?.ToList(), e?.ToList(), f?.ToList(), g?.ToList(),
                    Tuple.Create(h?.ToList(), i?.ToList(), j?.ToList(), k?.ToList(), l?.ToList(), m?.ToList()));
            }
        }
        public Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>, Tuple<List<H>, List<I>, List<J>, List<K>, List<L>, List<M>, List<N>>> ExecuteMARS
        <A, B, C, D, E, F, G, H, I, J, K, L, M, N>(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                IEnumerable<A> a = null;
                IEnumerable<B> b = null;
                IEnumerable<C> c = null;
                IEnumerable<D> d = null;
                IEnumerable<E> e = null;
                IEnumerable<F> f = null;
                IEnumerable<G> g = null;
                IEnumerable<H> h = null;
                IEnumerable<I> i = null;
                IEnumerable<J> j = null;
                IEnumerable<K> k = null;
                IEnumerable<L> l = null;
                IEnumerable<M> m = null;
                IEnumerable<N> n = null;
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                a = multi.Read<A>();
                try
                {
                    b = multi.Read<B>();
                }
                catch { }
                try
                {
                    c = multi.Read<C>();
                }
                catch { }
                try
                {
                    d = multi.Read<D>();
                }
                catch { }
                try
                {
                    e = multi.Read<E>();
                }
                catch { }
                try
                {
                    f = multi.Read<F>();
                }
                catch { }
                try
                {
                    g = multi.Read<G>();
                }
                catch { }
                try
                {
                    h = multi.Read<H>();
                }
                catch { }
                try
                {
                    i = multi.Read<I>();
                }
                catch { }
                try
                {
                    j = multi.Read<J>();
                }
                catch { }
                try
                {
                    k = multi.Read<K>();
                }
                catch { }
                try
                {
                    l = multi.Read<L>();
                }
                catch { }
                try
                {
                    m = multi.Read<M>();
                }
                catch { }
                try
                {
                    n = multi.Read<N>();
                }
                catch { }
                return new Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>,
                    Tuple<List<H>, List<I>, List<J>, List<K>, List<L>, List<M>, List<N>>>(
                    a?.ToList(), b?.ToList(), c?.ToList(), d?.ToList(), e?.ToList(), f?.ToList(), g?.ToList(),
                    Tuple.Create(h?.ToList(), i?.ToList(), j?.ToList(), k?.ToList(), l?.ToList(), m?.ToList(), n?.ToList()));
            }
        }
        public Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>, Tuple<List<H>, List<I>, List<J>, List<K>, List<L>, List<M>, List<N>, Tuple<List<O>>>> ExecuteMARS
        <A, B, C, D, E, F, G, H, I, J, K, L, M, N, O>(object Parameters)
        {
            using (IDbConnection Con = Connection)
            {
                #region results
                IEnumerable<A> a = null;
                IEnumerable<B> b = null;
                IEnumerable<C> c = null;
                IEnumerable<D> d = null;
                IEnumerable<E> e = null;
                IEnumerable<F> f = null;
                IEnumerable<G> g = null;
                IEnumerable<H> h = null;
                IEnumerable<I> i = null;
                IEnumerable<J> j = null;
                IEnumerable<K> k = null;
                IEnumerable<L> l = null;
                IEnumerable<M> m = null;
                IEnumerable<N> n = null;
                IEnumerable<O> o = null;
                #endregion
                Con.Open();
                var multi = Con.QueryMultiple(AdHocCommand, Parameters, null, MinTimeOut, CommandType.StoredProcedure);
                a = multi.Read<A>();
                try
                {
                    b = multi.Read<B>();
                }
                catch { }
                try
                {
                    c = multi.Read<C>();
                }
                catch { }
                try
                {
                    d = multi.Read<D>();
                }
                catch { }
                try
                {
                    e = multi.Read<E>();
                }
                catch { }
                try
                {
                    f = multi.Read<F>();
                }
                catch { }
                try
                {
                    g = multi.Read<G>();
                }
                catch { }
                try
                {
                    h = multi.Read<H>();
                }
                catch { }
                try
                {
                    i = multi.Read<I>();
                }
                catch { }
                try
                {
                    j = multi.Read<J>();
                }
                catch { }
                try
                {
                    k = multi.Read<K>();
                }
                catch { }
                try
                {
                    l = multi.Read<L>();
                }
                catch { }
                try
                {
                    m = multi.Read<M>();
                }
                catch { }
                try
                {
                    n = multi.Read<N>();
                }
                catch { }
                try
                {
                    o = multi.Read<O>();
                }
                catch { }
                return new Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>,
                    Tuple<List<H>, List<I>, List<J>, List<K>, List<L>, List<M>, List<N>, Tuple<List<O>>>>(
                    a?.ToList(), b?.ToList(), c?.ToList(), d?.ToList(), e?.ToList(), f?.ToList(), g?.ToList(),
                    Tuple.Create(h?.ToList(), i?.ToList(), j?.ToList(), k?.ToList(), l?.ToList(), m?.ToList(), n?.ToList(), o?.ToList()));
            }
        }
        public List<K> ExecuteJson<T, K>(T obj)
        {
            throw new NotImplementedException();
        }

        public IRepository SetTimeOut(int seconds)
        {
            MinTimeOut = seconds * 1000;
            return this;
        }
    }
}
