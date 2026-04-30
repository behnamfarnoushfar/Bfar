using System.Runtime.CompilerServices;

namespace Bfar.XCutting.Abstractions.SharedKernel
{
    public interface IReceiver<TInput, TOutput>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Task<TOutput?> ReceiveAsync(TInput model, CancellationToken cancellationToken = default);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Task<IEnumerable<TOutput>?> ReceiveAsync(TInput model, int receiveSize, int offset, CancellationToken cancellationToken = default);
    }
}
