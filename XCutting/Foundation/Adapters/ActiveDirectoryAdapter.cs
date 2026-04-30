using Bfar.XCutting.Abstractions.Adapters;
using Bfar.XCutting.Abstractions.SharedKernel;
using System.DirectoryServices.Protocols;
using System.Net;

namespace Bfar.XCutting.Foundation.Adapters
{
    public sealed class ActiveDirectoryAdapter : IRepositoryService
    {

        private LdapConnection connection;
        public int TimeOut => circuitBreaker?.GetCurrentTimeout() ?? 60;
        private string? command;
        private string? username;
        private string? password;
        private ICircuitBreakerAdapter? circuitBreaker;
        private string? host;
        private int auth = 1;
        private int sess = 3;
        private int port = 389;
        private string? filter;

        public string ConnectionString => throw new NotImplementedException();

        public ActiveDirectoryAdapter(ICircuitBreakerAdapter? circuitBreaker = null)
        {
            this.circuitBreaker = circuitBreaker;
            host = "dc.pishgaman.local";// circuitBreaker?.GetCurrentConnection();
            //connection = new DirectoryEntry(ConnectionString, "pass", "Pp123456");
            //connection = new DirectoryEntry(ConnectionString, "apigateway", "2j6YjLCg^SYxBB.sn9zVG:pk");

        }
        public IRepositoryService BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public IRepositoryService BuildCommand(string command)
        {
            this.command = command;
            return this;
        }

        public void BulkInsert<T>(in IEnumerable<T> model, int timeout = 30)
        {
            throw new NotImplementedException();
        }

        public IRepositoryService CommitTransaction()
        {
            throw new NotImplementedException();
        }

        public IRepositoryService Connect()
        {
            try
            {

                if (circuitBreaker?.ShouldAnswer() ?? true)
                {
                    NetworkCredential credential = new NetworkCredential(username, password);
                    connection = new LdapConnection(new LdapDirectoryIdentifier(host, port), credential, (AuthType)auth);
                    connection.SessionOptions.ProtocolVersion = sess;
                    connection.Bind();
                }
            }
            catch (Exception ex)
            {
                circuitBreaker?.Trip(ex, circuitBreaker.GetCurrentConnection());
                throw;
            }
            return this;
        }

        public IRepositoryService Disconnect()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public IEnumerable<TOutput>? Execute<TInput, TOutput>(in TInput? model) where TInput : class
            where TOutput : class
        {
            if (model == null)
                return null;
            Connect();
            var request = new SearchRequest(command, filter, SearchScope.Subtree);
            var result = connection.SendRequest(request) as SearchResponse;
            return result?.Entries?.Cast<SearchResultEntry>() as IEnumerable<TOutput>;
        }



        public void Execute<TInput>(in TInput? model) where TInput : class
        {
            throw new NotImplementedException();
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public IRepositoryService RollbackTransaction()
        {
            throw new NotImplementedException();
        }

        public IRepositoryService SetUsername(string username)
        {
            this.username = username;
            return this;
        }

        public IRepositoryService SetPassword(string password)
        {
            this.password = password;
            return this;
        }

        public IRepositoryService SetAddress(string address)
        {
            this.host = address;
            return this;
        }
        public IRepositoryService SetFilters(string filter)
        {
            this.filter = filter;
            return this;
        }
        public IRepositoryService SetPort(int port)
        {
            this.port = port;
            return this;
        }

        public IRepositoryService SetSessionLevel(int level)
        {
            sess = level;
            return this;
        }

        public IRepositoryService SetAuthenticationLevel(int level)
        {
            auth = level;
            return this;
        }

        public Task<IRepositoryService> ConnectAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IRepositoryService> DisconnectAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public Task ExecuteAsync<TInput>(TInput? model, CancellationToken cancellationToken = default) where TInput : class
        {
            throw new NotImplementedException();
        }

        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task BulkInsertAsync<T>(IEnumerable<T> model, int size = 50000, int timeout = 0, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IRepositoryService SetConnectionString(string str)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TOutput>?> ExecuteAsync<TInput, TOutput>(TInput? model, CancellationToken cancellationToken = default)
            where TInput : class
            where TOutput : class
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<IEnumerable<TOutput>>? ExecuteStream<TInput, TOutput>(TInput? model, int bufferSize, CancellationToken cancellationToken = default)
            where TInput : class
            where TOutput : class
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TOutput>?> ExecuteAsync<TOutput>(CancellationToken cancellationToken = default) where TOutput : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TOutput>? Execute<TOutput>() where TOutput : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TOutput>? ExecuteAdHoc<TInput, TOutput>(in TInput? model)
            where TInput : class
            where TOutput : class
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TOutput>?> ExecuteAdHocAsync<TInput, TOutput>(TInput? model, CancellationToken cancellationToken = default)
            where TInput : class
            where TOutput : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TOutput>? ExecuteAdHoc<TOutput>() where TOutput : class
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TOutput>?> ExecuteAdHocAsync<TOutput>(CancellationToken cancellationToken = default) where TOutput : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TOutput>? ExecuteJson<TInput, TOutput>(in TInput? model)
            where TInput : class
            where TOutput : class
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TOutput>?> ExecuteJsonAsync<TInput, TOutput>(TInput? model, CancellationToken cancellationToken = default)
            where TInput : class
            where TOutput : class
        {
            throw new NotImplementedException();
        }
    }
}
