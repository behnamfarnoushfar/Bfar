using Bfar.XCutting.Abstractions.Entities.Dtos;
using Bfar.XCutting.Abstractions.Entities.Models;
using System.Runtime.CompilerServices;

namespace Bfar.XCutting.Abstractions.SharedKernel
{
    public interface IQueryService<TInput, TOutput> where TInput : class where TOutput : class
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<AppDto<TInput, TOutput>> QueryAsync(TInput model, PaginationModel? pagination = null, CancellationToken cancellationToken = default);
    }
}
