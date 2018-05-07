using System;
using System.Collections.Generic;
using System.Data;

namespace Bfar.Repository
{
    public interface IRepository
    {
        string AdHocCommand { get; set; }
        int MinTimeOut { get; set; }
        List<K> ExecuteJson<T, K>(T obj);
        void Execute(object Parameters = null);
        List<K> Execute<K>(object Parameters = null) where K : class;
        List<K> ExecuteAdHoc<K>(object Parameters = null) where K : class;
        List<K> ExecuteAdHocChain<K>(object Parameters = null) where K : class;
        IRepository Build(string Command);
        long Add<K>(K model) where K : class;
        IEnumerable<K> GetAll<K>() where K : class;
        long ChainAdd<T>(T model) where T : class;
        IDbTransaction BeginTransaction();
        List<T> ExecuteChain<T>(object Parameters = null);
        Tuple<List<A>, List<B>, List<C>, List<D>> ExecuteMARS<A, B, C, D>(object Parameters);
        Tuple<List<A>, List<B>, List<C>, List<D>, List<E>> ExecuteMARS<A, B, C, D, E>(object Parameters);
        Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>> ExecuteMARS<A, B, C, D, E, F>(object Parameters);
        Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>> ExecuteMARS<A, B, C, D, E, F, G>(object Parameters);
        Tuple<List<A>, List<B>, List<C>> ExecuteMARS<A, B, C>(object Parameters);
        Tuple<List<A>, List<B>> ExecuteMARS<A, B>(object Parameters);
        void BulkInsert<T>(IList<T> Data, string Name) where T : class;
        void BulkInsertChain<T>(IList<T> Data, string Name) where T : class;
    }
}
