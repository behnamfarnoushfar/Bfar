using Bfar.XCutting.Abstractions.Adapters;
using Bfar.XCutting.Abstractions.Decorators;
using Bfar.XCutting.Abstractions.SharedKernel;
using Microsoft.Extensions.Logging;

namespace Bfar.XCutting.SharedKernel.Contracts
{
    public interface ICoreFactory
    {
        CoreConfig Config { get; }
        ILoggerFactory LoggerFactory { get; }
        IRestClientService Rest { get; }
        IJsonParserAdapter Json { get; }
        IDateTimeAdapter Date { get; }
        ICoreComponentDirector ComponentDirector { get; }
        ISharedKernel Kernel { get; }
        IRepositoryService BuildRepositoryService();
    }
}
