using Bfar.XCutting.Abstractions.Decorators;
using Bfar.XCutting.Abstractions.SharedKernel;
using Bfar.XCutting.SharedKernel.Contracts;

namespace Bfar.XCutting.SharedKernel.Core.Builders
{
    public sealed class StoreProcedureComponentBuilder<TInput, TOutput>
        : AbstractCoreComponentBuilder<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {
        private string? _procName;
        public StoreProcedureComponentBuilder(ICoreFactory factory) : base(factory)
        {
        }
        public override AbstractCoreComponentBuilder<TInput, TOutput> ConfigureService(IServiceDecorator<TInput, TOutput>? service = null)
        {
            if (service is null)
                _service = new RepositoryServiceDecorator<TInput, TOutput>(factory.BuildRepositoryService(), _procName, null);
            else
                _service = service;
            return this;
        }
        public override AbstractCoreComponentBuilder<TInput, TOutput> ConfigureParameter(string componentName, string? param = null)
        {
            if (!string.IsNullOrEmpty(param))
                _procName = param;
            if (string.IsNullOrEmpty(param) && string.IsNullOrEmpty(_procName))
                throw new Exception("parameter name required for StoreProcedureComponentBuilder");
            base.ConfigureParameter(componentName, param);
            return this;
        }

        public override ICoreComponent<TInput, TOutput> Build(string? param)
        {
            ConfigureParameter("StoreProcedureComponent", param)
                .ConfigureService()
                .ConfigureReceiver()
                .ConfigureQuery(param);
            return new CoreComponent<TInput, TOutput>(factory, _query, _command, _componenetName!);
        }
    }
}
