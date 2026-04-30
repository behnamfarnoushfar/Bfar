using Bfar.XCutting.Abstractions.Adapters;
using Bfar.XCutting.Abstractions.Decorators;
using Bfar.XCutting.Abstractions.SharedKernel;
using Bfar.XCutting.Foundation.Adapters;
using Bfar.XCutting.SharedKernel.Contracts;
using Bfar.XCutting.SharedKernel.Core.Builders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bfar.XCutting.SharedKernel.Core.Factories
{
    public sealed class CoreFactory : ICoreFactory
    {
        public CoreFactory(
            IOptions<CoreConfig> config,
            ILoggerFactory loggerFactory,
            IRestClientService rest,
            IJsonParserAdapter json,
            IDateTimeAdapter date)
        {
            Config = config.Value;
            LoggerFactory = loggerFactory;
            Rest = rest;
            Json = json;
            Date = date;
            Kernel = new SharedAppAPIService(Rest, Config.SharedKernelPortConfig, LoggerFactory);
            ComponentDirector = new CoreComponentDirector(this);
        }

        public CoreConfig Config { get; }
        public ILoggerFactory LoggerFactory { get; }
        public IRestClientService Rest { get; }
        public IJsonParserAdapter Json { get; }
        public IDateTimeAdapter Date { get; }
        public ICoreComponentDirector ComponentDirector { get; }

        public ISharedKernel Kernel { get; }
        public IRepositoryService BuildRepositoryService()
        {
            return new MSSQLDapperAdapter(Config.Domain.ConnectionString);
        }
    }
}
