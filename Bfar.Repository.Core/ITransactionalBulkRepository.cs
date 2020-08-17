using System.Collections.Generic;
using System.Data;

namespace Bfar.Repository.Core
{
    public interface ITransactionalBulkRepository
    {
        string AdHocCommand { get; set; }
        int MinTimeOut { get; set; }
        IDbTransaction BeginTransaction();
        bool RollbackTransaction();
        bool CommitTransaction();
        void BulkInsertChain<T>(IList<T> Data, string Name) where T : class;
    }
}
