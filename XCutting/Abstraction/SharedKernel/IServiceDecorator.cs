using System.Runtime.CompilerServices;

namespace Bfar.XCutting.Abstractions.SharedKernel
{
    public interface IServiceDecorator<TInput, TOutput>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Task<TOutput?> SingleResultCallAsync(TInput model, CancellationToken cancellationToken = default);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Task<IEnumerable<TOutput>?> MultiResultCallAsync(TInput model, int size, int offset, CancellationToken cancellationToken = default);
        public string? ServiceResouce { get; }
        public string? ServiceAction { get; }
    }
}
