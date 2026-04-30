using Bfar.XCutting.Abstractions.SharedKernel;
using Bfar.XCutting.SharedKernel.Contracts;

namespace Bfar.XCutting.SharedKernel.Core.Builders
{
    public sealed class UsecaseComponentBuilder<TInput, TOutput>
        : AbstractCoreComponentBuilder<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {
        public UsecaseComponentBuilder(ICoreFactory factory) : base(factory)
        {
        }

        public override AbstractCoreComponentBuilder<TInput, TOutput> ConfigureReceiver(IReceiver<TInput, TOutput>? receiver = null)
        {
            return base.ConfigureReceiver(receiver);
        }
    }
}
