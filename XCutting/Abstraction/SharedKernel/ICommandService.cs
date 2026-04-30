using Bfar.XCutting.Abstractions.Entities.Dtos;
using System.Runtime.CompilerServices;

namespace Bfar.XCutting.Abstractions.SharedKernel
{
    public interface ICommandService<TInput, TOutput>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<AppDto<TInput, TOutput>> ExecuteAsync(TInput model, CancellationToken cancellationToken = default);
    }
}
