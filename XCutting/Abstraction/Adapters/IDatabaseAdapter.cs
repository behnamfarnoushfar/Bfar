namespace Bfar.XCutting.Abstractions.Adapters
{
    public interface IDatabaseAdapter
    {
        string? ConnectionString { get; }
        int TimeOut { get; }

        IDatabaseAdapter BeginTransaction();
        IDatabaseAdapter BuildCommand(string command);
        void BulkInsert<T>(in IEnumerable<T> model, int timeout = 30);
        Task BulkInsertAsync<T>(IEnumerable<T> model, int size = 50000, int timeout = 0, CancellationToken cancellationToken = default);
        IDatabaseAdapter CommitTransaction();
        IDatabaseAdapter Connect();
        Task<IDatabaseAdapter> ConnectAsync(CancellationToken cancellationToken = default);
        IDatabaseAdapter Disconnect();
        Task<IDatabaseAdapter> DisconnectAsync(CancellationToken cancellationToken = default);
        void Dispose();
        void Execute();
        IEnumerable<TOutput>? Execute<TInput, TOutput>(in TInput? model)
            where TInput : class
            where TOutput : class;
        void Execute<TInput>(in TInput? model) where TInput : class;
        IEnumerable<TOutput>? Execute<TOutput>() where TOutput : class;
        IEnumerable<TOutput>? ExecuteAdHoc<TInput, TOutput>(in TInput? model)
            where TInput : class
            where TOutput : class;
        IEnumerable<TOutput>? ExecuteAdHoc<TOutput>() where TOutput : class;
        Task<IEnumerable<TOutput>?> ExecuteAdHocAsync<TInput, TOutput>(TInput? model, CancellationToken cancellationToken = default)
            where TInput : class
            where TOutput : class;
        Task<IEnumerable<TOutput>?> ExecuteAdHocAsync<TOutput>(CancellationToken cancellationToken = default) where TOutput : class;
        Task ExecuteAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TOutput>?> ExecuteAsync<TInput, TOutput>(TInput? model, CancellationToken cancellationToken = default)
            where TInput : class
            where TOutput : class;
        Task ExecuteAsync<TInput>(TInput? model, CancellationToken cancellationToken = default) where TInput : class;
        Task<IEnumerable<TOutput>?> ExecuteAsync<TOutput>(CancellationToken cancellationToken = default) where TOutput : class;
        IEnumerable<TOutput>? ExecuteJson<TInput, TOutput>(in TInput? model)
            where TInput : class
            where TOutput : class;
        Task<IEnumerable<TOutput>?> ExecuteJsonAsync<TInput, TOutput>(TInput? model, CancellationToken cancellationToken = default)
            where TInput : class
            where TOutput : class;
        IAsyncEnumerable<IEnumerable<TOutput>>? ExecuteStream<TInput, TOutput>(TInput? model, int bufferSize, CancellationToken cancellationToken = default)
            where TInput : class
            where TOutput : class;
        IDatabaseAdapter RollbackTransaction();
        IDatabaseAdapter SetAddress(string address);
        IDatabaseAdapter SetAuthenticationLevel(int level);
        IDatabaseAdapter SetConnectionString(string str);
        IDatabaseAdapter SetFilters(string filter);
        IDatabaseAdapter SetPassword(string password);
        IDatabaseAdapter SetPort(int port);
        IDatabaseAdapter SetSessionLevel(int level);
        IDatabaseAdapter SetUsername(string username);
    }
}
