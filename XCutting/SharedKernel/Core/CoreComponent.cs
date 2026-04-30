using Bfar.XCutting.Abstractions.Entities.Dtos;
using Bfar.XCutting.Abstractions.Entities.Models;
using Bfar.XCutting.Abstractions.SharedKernel;
using Bfar.XCutting.SharedKernel.Contracts;

namespace Bfar.XCutting.SharedKernel.Core
{
    public sealed class CoreComponent<TInput, TOutput> : ICoreComponent<TInput, TOutput> where TInput : class where TOutput : class
    {
        private readonly ICoreFactory factory;

        public CoreComponent(ICoreFactory factory,
            IQueryService<TInput, TOutput>? query,
            ICommandService<TInput, TOutput>? command,
            string Name)
        {
            if (string.IsNullOrEmpty(Name))
                throw new ArgumentNullException("Name cannot be null or empty");

            if (command is null && query is null)
                throw new ArgumentNullException("Either command or query cannot be null or empty");

            this.factory = factory;
            Query = query;
            Command = command;
            this.Name = Name;
        }
        public IQueryService<TInput, TOutput>? Query { get; }
        public ICommandService<TInput, TOutput>? Command { get; }
        public string Name { get; }

        public Task<AppDto<TInput, TOutput>> ProcessAsync(TInput model, PaginationModel? pagination = null, CancellationToken cancellationToken = default)
        {
            if (Command is not null)
                return Command.ExecuteAsync(model, cancellationToken);
            else
                return Query!.QueryAsync(model, pagination, cancellationToken);
        }
    }

}
