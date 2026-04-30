using System.Data;

namespace Bfar.XCutting.Abstractions.SharedKernel
{
    public interface IRepositoryService
    {
        string? ConnectionString { get; }
        int TimeOut { get; }
        IRepositoryService Connect();
        Task<IRepositoryService> ConnectAsync(CancellationToken cancellationToken = default);
        IRepositoryService SetUsername(string username);
        IRepositoryService SetPassword(string password);
        IRepositoryService SetAddress(string address);
        IRepositoryService SetPort(int port);
        IRepositoryService SetSessionLevel(int level);
        IRepositoryService SetAuthenticationLevel(int level);
        IRepositoryService SetConnectionString(string str);
        IRepositoryService Disconnect();
        Task<IRepositoryService> DisconnectAsync(CancellationToken cancellationToken = default);
        IRepositoryService BuildCommand(string command);
        IEnumerable<TOutput>? Execute<TInput, TOutput>(in TInput? model) where TInput : class where TOutput : class;
        Task<IEnumerable<TOutput>?> ExecuteAsync<TInput, TOutput>(TInput? model, CancellationToken cancellationToken = default) where TInput : class where TOutput : class;
        IAsyncEnumerable<IEnumerable<TOutput>>? ExecuteStream<TInput, TOutput>(TInput? model, int bufferSize, CancellationToken cancellationToken = default) where TInput : class where TOutput : class;
        IEnumerable<TOutput>? Execute<TOutput>() where TOutput : class;
        Task<IEnumerable<TOutput>?> ExecuteAsync<TOutput>(CancellationToken cancellationToken = default) where TOutput : class;
        void Execute<TInput>(in TInput? model) where TInput : class;
        Task ExecuteAsync<TInput>(TInput? model, CancellationToken cancellationToken = default) where TInput : class;
        void Execute();
        Task ExecuteAsync(CancellationToken cancellationToken = default);
        IEnumerable<TOutput>? ExecuteAdHoc<TInput, TOutput>(in TInput? model) where TInput : class where TOutput : class;
        Task<IEnumerable<TOutput>?> ExecuteAdHocAsync<TInput, TOutput>(TInput? model, CancellationToken cancellationToken = default) where TInput : class where TOutput : class;
        IEnumerable<TOutput>? ExecuteAdHoc<TOutput>() where TOutput : class;
        Task<IEnumerable<TOutput>?> ExecuteAdHocAsync<TOutput>(CancellationToken cancellationToken = default) where TOutput : class;
        IEnumerable<TOutput>? ExecuteJson<TInput, TOutput>(in TInput? model) where TInput : class where TOutput : class;
        Task<IEnumerable<TOutput>?> ExecuteJsonAsync<TInput, TOutput>(TInput? model, CancellationToken cancellationToken = default) where TInput : class where TOutput : class;
        IRepositoryService BeginTransaction();
        IRepositoryService CommitTransaction();
        IRepositoryService RollbackTransaction();
        void BulkInsert<T>(in IEnumerable<T> model, int timeout = 30);
        Task BulkInsertAsync<T>(IEnumerable<T> model, int size = 50000, int timeout = 0, CancellationToken cancellationToken = default);
        IRepositoryService SetFilters(string filter);
    }
}
