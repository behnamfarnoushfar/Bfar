using Bfar.XCutting.Abstractions.SharedKernel;
using Bfar.XCutting.SharedKernel.Core.Builders;

namespace Bfar.XCutting.SharedKernel.Contracts
{
    public interface ICoreComponentDirector
    {
        ICoreComponent<TInput, TOutput> ConstructRepositoryComponent<TInput, TOutput>
            (string text, AbstractCoreComponentBuilder<TInput, TOutput>? builder = null)
            where TInput : class
            where TOutput : class;
        ICoreComponent<TInput, TOutput> ConstructWithUsecase<TInput, TOutput>
            (IReceiver<TInput,TOutput> receiver, AbstractCoreComponentBuilder<TInput, TOutput>? builder = null)
            where TInput : class
            where TOutput : class;
    }
}