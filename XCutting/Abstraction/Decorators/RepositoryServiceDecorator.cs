using Bfar.XCutting.Abstractions.SharedKernel;
using System.Runtime.CompilerServices;

namespace Bfar.XCutting.Abstractions.Decorators
{
    public sealed class RepositoryServiceDecorator<TInput, TOutput> : IServiceDecorator<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {
        private readonly IRepositoryService repository;

        public RepositoryServiceDecorator(IRepositoryService repository, string? resource, string? action)
        {
            this.repository = repository;
            ServiceResouce = resource;
            ServiceAction = action;
        }

        public string? ServiceResouce { get; }

        public string? ServiceAction { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<IEnumerable<TOutput>?> MultiResultCallAsync(TInput model, int size, int offset, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(ServiceResouce))
                throw new ArgumentNullException("query text is not initialized");
            return repository.BuildCommand(ServiceResouce!).ExecuteAsync<TInput, TOutput>(model, cancellationToken);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async Task<TOutput?> SingleResultCallAsync(TInput model, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(ServiceResouce))
                throw new ArgumentNullException("query text is not initialized");
            var result = await repository.BuildCommand(ServiceResouce!).ExecuteAsync<TInput, TOutput>(model, cancellationToken);
            return result?.FirstOrDefault();
        }

    }
}
