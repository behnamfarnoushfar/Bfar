using Bfar.XCutting.Abstractions.Adapters;
using Bfar.XCutting.Abstractions.Decorators;
using Bfar.XCutting.Abstractions.SharedKernel;
using Bfar.XCutting.Foundation.Adapters;
using Bfar.XCutting.SharedKernel.Contracts;
using Bfar.XCutting.SharedKernel.Core.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bfar.XCutting.SharedKernel.Core
{
    public static class SharedKernelExtensions
    {
        public static void AddSharedKernel(this IServiceCollection services, IConfiguration configuration, ILoggingBuilder logging, string ServiceName)
        {
            //services.AddOpenTelemetry()
            //    .WithLogging(b => b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(ServiceName))
            //      .AddOtlpExporter(options =>
            //      {
            //          //OpenTelemetry:Type:HTTP|HTTPS|GRPC
            //          options.Endpoint = new Uri(configuration.GetSection("OpenTelemetry:Log:EndPoint").Value ?? "https://alloy.xxxxxx.net/v1/logs");
            //          options.Protocol = OtlpExportProtocol.HttpProtobuf;
            //          options.ExportProcessorType = OpenTelemetry.ExportProcessorType.Simple;
            //          options.BatchExportProcessorOptions.MaxExportBatchSize = 1;
            //          options.BatchExportProcessorOptions.MaxQueueSize = 1;
            //          options.BatchExportProcessorOptions.ScheduledDelayMilliseconds = 1;
            //      }));

            //services.AddSingleton<IMessageTranslator<HttpRequestMessage, HttpResponseMessage>, HttpTranslator>();

            services.AddSingleton<IJsonParserAdapter, SystemTextJsonParserAdapter>();
            services.AddSingleton<IDateTimeAdapter, DateTimeAdapter>();
            services.AddSingleton<IRestClientService, RestSharpChannelAdapter>();
            services.AddSingleton<ICoreFactory, CoreFactory>();
            services.AddSingleton<ICircuitBreakerAdapter>(x=>null);

            //if (configuration.GetSection("EventSourcingConfiguration").Exists())
            //{
            //    services.AddSingleton<IEventSourcingFactory, EventSourcingFactory>();
            //}
            if (configuration.GetSection("CoreConfig").Exists())
            {
                services.AddSingleton<ICoreFactory, CoreFactory>();
                services.Configure<CoreConfig>(configuration.GetSection("CoreConfig"));
            }

            if (configuration.GetSection("Storage").Exists())
            {
                //services.AddSingleton<IStorageService, S3StorageService>();
                //var storagecfg = new StorageConfig()
                //{
                //    Host = configuration.GetSection("Storage:Host").Value!,
                //    AccessKey = configuration.GetSection("Storage:Username").Value!,
                //    DefaultBucket = configuration.GetSection("Storage:DefaultBucket").Value!,
                //    SecretKey = configuration.GetSection("Storage:Password").Value!,
                //    DefaultPath = configuration.GetSection("Storage:DefaultPath").Value!,
                //};
                //services = services.AddSingleton(storagecfg);
            }
        }
    }
}
