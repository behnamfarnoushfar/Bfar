using Bfar.XCutting.Abstractions.Dtos;

namespace Bfar.XCutting.Abstractions.Infrastructures
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<DbGeneralResponse?> AddAsync(T entity);
        Task<DbGeneralResponse?> UpdateAsync(T entity);
        Task<DbGeneralResponse?> DeleteAsync(int id);
        Task<DbGeneralResponse?> ExecuteAdHocAsync(T? model);
        Task<TResult?> ExecuteAdHocAsync<TResult>(T? model);
        Task<TResult?> ExecuteProcedureAsync<TResult>(T? model);
    }
}
