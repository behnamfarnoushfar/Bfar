using Bfar.XCutting.Abstractions.SharedKernel;
using Bfar.XCutting.SharedKernel.Contracts;
using Bfar.XCutting.SharedKernel.Core.Commands;

namespace Bfar.XCutting.SharedKernel.Core.Builders
{
    public class AbstractCoreComponentBuilder<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {
        protected string? _componenetName;
        protected IServiceDecorator<TInput, TOutput>? _service;
        protected IReceiver<TInput, TOutput>? _receiver;
        protected IQueryService<TInput, TOutput>? _query;
        protected ICommandService<TInput, TOutput>? _command;
        protected readonly ICoreFactory factory;

        public AbstractCoreComponentBuilder(ICoreFactory factory)
        {
            this.factory = factory;
        }

        public virtual AbstractCoreComponentBuilder<TInput, TOutput> ConfigureParameter(string componentName, string? param = null)
        {
            if (string.IsNullOrEmpty(componentName))
                throw new Exception("componentName cannot be null or empty");
            _componenetName = componentName;
            return this;
        }
        public virtual AbstractCoreComponentBuilder<TInput, TOutput> ConfigureService(IServiceDecorator<TInput, TOutput>? service = null)
        {
            _service = service;
            return this;
        }

        public virtual AbstractCoreComponentBuilder<TInput, TOutput> ConfigureReceiverWithService(IServiceDecorator<TInput, TOutput>? service)
        {
            if (service is null)
                _receiver = new GenericReceiver<TInput, TOutput>(_service);
            else
                _receiver = new GenericReceiver<TInput, TOutput>(service);
            return this;
        }
        public virtual AbstractCoreComponentBuilder<TInput, TOutput> ConfigureReceiver(IReceiver<TInput, TOutput>? receiver = null)
        {
            if (receiver is null && _receiver is null)
                _receiver = new GenericReceiver<TInput, TOutput>(_service);
            else if(_receiver is null)
                _receiver = receiver;
            return this;
        }
        public virtual AbstractCoreComponentBuilder<TInput, TOutput> ConfigureCommad(string commandText, IReceiver<TInput, TOutput> receiver = null)
        {
            if (receiver is null)
                _command = new GenericCommand<TInput, TOutput>(factory.LoggerFactory, commandText, _receiver);
            else
                _command = new GenericCommand<TInput, TOutput>(factory.LoggerFactory, commandText, receiver);
            return this;
        }
        public virtual AbstractCoreComponentBuilder<TInput, TOutput> ConfigureCommadWithService(string commandText, ICommandService<TInput, TOutput> command = null)
        {
            if (command is null)
                _command = new GenericCommand<TInput, TOutput>(factory.LoggerFactory, commandText, _receiver);
            else
                _command = command;
            return this;
        }
        public virtual AbstractCoreComponentBuilder<TInput, TOutput> ConfigureQuery(string queryText, IReceiver<TInput, TOutput> receiver = null)
        {
            if (receiver is null)
                _query = new GenericQuery<TInput, TOutput>(factory.LoggerFactory, queryText, _receiver);
            else
                _query = new GenericQuery<TInput, TOutput>(factory.LoggerFactory, queryText, receiver);
            return this;
        }
        public virtual AbstractCoreComponentBuilder<TInput, TOutput> ConfigureQueryWithSerivce(string queryText, IQueryService<TInput, TOutput>? query = null)
        {
            if (query is null)
                _query = new GenericQuery<TInput, TOutput>(factory.LoggerFactory, queryText, _receiver);
            else
                _query = query;
            return this;
        }


        public virtual ICoreComponent<TInput, TOutput> Build(string? param)
        {
            ConfigureParameter("StoreProcedureComponent", param)
                .ConfigureService()
                .ConfigureReceiver(null)
                .ConfigureQuery(param);
            return new CoreComponent<TInput, TOutput>(factory, _query, _command, _componenetName!);
        }
    }
}
