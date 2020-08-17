using System.Collections.Generic;
using System.Data;

namespace Bfar.Repository.Core
{
    public interface ITransactionalRepository
    {
        string AdHocCommand { get; set; }
        int MinTimeOut { get; set; }
        List<K> ExecuteAdHocChain<K>(object Parameters = null) where K : class;
        ITransactionalRepository Build(string Command);
        IEnumerable<K> GetAll<K>() where K : class;
        long ChainAdd<T>(T model) where T : class;
        IDbTransaction BeginTransaction();
        List<T> ExecuteChain<T>(object Parameters = null);
        bool RollbackTransaction();
        bool CommitTransaction();
    }
}
