using System.Collections.Generic;

namespace Bfar.Repository.Core
{

    public interface ISimpleRepository
    {
        string AdHocCommand { get; set; }
        int MinTimeOut { get; set; }
        List<K> ExecuteJson<T, K>(T obj);
        void Execute(object Parameters = null);
        List<K> Execute<K>(object Parameters = null) where K : class;
        List<K> ExecuteAdHoc<K>(object Parameters = null) where K : class;
        ISimpleRepository Build(string Command);
        long Add<K>(K model) where K : class;
        IEnumerable<K> GetAll<K>() where K : class;
    }
}
