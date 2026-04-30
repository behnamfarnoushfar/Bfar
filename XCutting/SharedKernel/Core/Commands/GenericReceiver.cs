using Bfar.XCutting.Abstractions.SharedKernel;

namespace Bfar.XCutting.SharedKernel.Core.Commands
{
    public sealed class GenericReceiver<TInput, TOutput> : IReceiver<TInput, TOutput>
    {
        private readonly IServiceDecorator<TInput, TOutput> service;

        public GenericReceiver(IServiceDecorator<TInput, TOutput> service)
        {
            this.service = service;
        }

        public Task<TOutput?> ReceiveAsync(TInput model, CancellationToken cancellationToken = default)
        {
            return service.SingleResultCallAsync(model, cancellationToken);
        }

        public Task<IEnumerable<TOutput>?> ReceiveAsync(TInput model, int receiveSize, int offset, CancellationToken cancellationToken = default)
        {
            return service.MultiResultCallAsync(model, receiveSize, offset, cancellationToken);
        }
    }
}
