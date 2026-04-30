using Bfar.XCutting.Abstractions.SharedKernel;
using Bfar.XCutting.SharedKernel.Contracts;

namespace Bfar.XCutting.SharedKernel.Core.Builders
{
    public sealed class CoreComponentDirector : ICoreComponentDirector
    {
        public CoreComponentDirector(ICoreFactory factory)
        {
            this.factory = factory;
        }
        private readonly ICoreFactory factory;

        public ICoreComponent<TInput, TOutput> ConstructRepositoryComponent<TInput, TOutput>
            (string text, AbstractCoreComponentBuilder<TInput, TOutput>? builder = null)
            where TInput : class
            where TOutput : class
        {
            if (builder is null)
                return new StoreProcedureComponentBuilder<TInput, TOutput>(factory).Build(text);
            return builder.Build(text);
        }

        public ICoreComponent<TInput, TOutput> ConstructWithUsecase<TInput, TOutput>(IReceiver<TInput, TOutput> receiver, AbstractCoreComponentBuilder<TInput, TOutput>? builder = null)
            where TInput : class
            where TOutput : class
        {
            if (builder is null)
                return new UsecaseComponentBuilder<TInput, TOutput>(factory).ConfigureReceiver(receiver).Build("");
            return builder.Build("");
        }
    }
}
