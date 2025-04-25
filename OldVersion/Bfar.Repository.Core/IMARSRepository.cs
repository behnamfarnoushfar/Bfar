using System;
using System.Collections.Generic;

namespace Bfar.Repository.Core
{
    public interface IMARSRepository
    {
        string AdHocCommand { get; set; }
        int MinTimeOut { get; set; }
        Tuple<List<A>, List<B>> ExecuteMARS<A, B>(object Parameters);
        Tuple<List<A>, List<B>, List<C>> ExecuteMARS<A, B, C>(object Parameters);
        Tuple<List<A>, List<B>, List<C>, List<D>> ExecuteMARS<A, B, C, D>(object Parameters);
        Tuple<List<A>, List<B>, List<C>, List<D>, List<E>> ExecuteMARS<A, B, C, D, E>(object Parameters);
        Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>> ExecuteMARS<A, B, C, D, E, F>(object Parameters);
        Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>> ExecuteMARS<A, B, C, D, E, F, G>(object Parameters);
        Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>, Tuple<List<H>>> ExecuteMARS<A, B, C, D, E, F, G, H>(object Parameters);
        Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>, Tuple<List<H>, List<I>>> ExecuteMARS<A, B, C, D, E, F, G, H, I>(object Parameters);
        Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>, Tuple<List<H>, List<I>, List<J>>> ExecuteMARS<A, B, C, D, E, F, G, H, I, J>(object Parameters);
        Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>, Tuple<List<H>, List<I>, List<J>, List<K>>> ExecuteMARS<A, B, C, D, E, F, G, H, I, J, K>(object Parameters);
        Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>, Tuple<List<H>, List<I>, List<J>, List<K>, List<L>>> ExecuteMARS<A, B, C, D, E, F, G, H, I, J, K, L>(object Parameters);
        Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>, Tuple<List<H>, List<I>, List<J>, List<K>, List<L>, List<M>>> ExecuteMARS<A, B, C, D, E, F, G, H, I, J, K, L, M>(object Parameters);
        Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>, Tuple<List<H>, List<I>, List<J>, List<K>, List<L>, List<M>, List<N>>> ExecuteMARS<A, B, C, D, E, F, G, H, I, J, K, L, M, N>(object Parameters);
        Tuple<List<A>, List<B>, List<C>, List<D>, List<E>, List<F>, List<G>, Tuple<List<H>, List<I>, List<J>, List<K>, List<L>, List<M>, List<N>, Tuple<List<O>>>> ExecuteMARS<A, B, C, D, E, F, G, H, I, J, K, L, M, N, O>(object Parameters);


    }
}
