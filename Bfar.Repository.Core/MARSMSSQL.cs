using Bfar.Extensions.Core;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Bfar.Repository.Core
{
    public class MARSMSSQL : IMARSRepository
    {
        private DbProviderFactory _provider;
        private string connectionString;
        private string providerString;
        private IDbConnection _connection;
        private IDbTransaction _currentTransaction;
        public static MARSMSSQL Factory { get { return new MARSMSSQL(); } }
        public static MARSMSSQL Connect(string connectionString)
        {
            return new MARSMSSQL(connectionString);
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
        public MARSMSSQL()
        {
            MinTimeOut = 120;
            providerString = ConfigurationManager.ConnectionStrings["SourceData"].ProviderName;
            connectionString = ConfigurationManager.ConnectionStrings["SourceData"].ConnectionString;
            _provider = DbProviderFactories.GetFactory(providerString);
        }
        public MARSMSSQL(string connectionSetting)
        {
            MinTimeOut = 120;
            providerString = ConfigurationManager.ConnectionStrings[connectionSetting].ProviderName;
            connectionString = ConfigurationManager.ConnectionStrings[connectionSetting].ConnectionString;
            _provider = DbProviderFactories.GetFactory(providerString);
        }
        public IMARSRepository Build(string Command)
        {
            AdHocCommand = Command;
            return this;
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
        public IMARSRepository SetTimeOut(int seconds)
        {
            MinTimeOut = seconds * 1000;
            return this;
        }
    }
}
