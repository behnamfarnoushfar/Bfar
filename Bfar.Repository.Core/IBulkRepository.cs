using System.Collections.Generic;

namespace Bfar.Repository.Core
{
    public interface IBulkRepository
    {
        string AdHocCommand { get; set; }
        int MinTimeOut { get; set; }
        void BulkInsert<T>(IList<T> Data, string Name) where T : class;
    }
}
