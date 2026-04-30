using Bfar.XCutting.Abstractions.Entities.Dtos;
using Bfar.XCutting.Abstractions.Entities.Models;
using Bfar.XCutting.Abstractions.SharedKernel;

namespace Bfar.XCutting.SharedKernel.Contracts
{
    public interface ICoreComponent<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {
        ICommandService<TInput, TOutput>? Command { get; }
        string Name { get; }
        IQueryService<TInput, TOutput>? Query { get; }

        Task<AppDto<TInput, TOutput>> ProcessAsync(TInput model, PaginationModel? pagination = null, CancellationToken cancellationToken = default);
    }
}