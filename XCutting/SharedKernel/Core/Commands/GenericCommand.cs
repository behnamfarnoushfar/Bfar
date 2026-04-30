using Bfar.XCutting.Abstractions.Entities.Dtos;
using Bfar.XCutting.Abstractions.SharedKernel;
using Microsoft.Extensions.Logging;

namespace Bfar.XCutting.SharedKernel.Core.Commands
{
    public sealed class GenericCommand<TInput, TOutput> : ICommandService<TInput, TOutput>
    {
        private readonly string commandName;
        private readonly IReceiver<TInput, TOutput> receiver;
        private readonly ILogger logger;

        public GenericCommand(ILoggerFactory loggerFactory, string commandName, IReceiver<TInput, TOutput> receiver)
        {
            this.commandName = commandName;
            this.receiver = receiver;
            logger = loggerFactory.CreateLogger(commandName);
        }
        public async Task<AppDto<TInput, TOutput>> ExecuteAsync(TInput model, CancellationToken cancellationToken = default)
        {
            AppDto<TInput, TOutput> result = new();
            try
            {
                if (model is null)
                {
                    result.Successed = false;
                    result.ResultDescription = "اطلاعات وارد شده نامعتبر است";
                }
                else
                {
                    result.Result = await receiver.ReceiveAsync(model, cancellationToken);
                    if (result.Result is null)
                    {
                        result.Successed = false;
                        result.ResultDescription = "خطا در اجرای سرویس";
                        logger.LogError(result.ResultDescription, commandName);
                    }
                    else
                    {
                        result.Successed = true;
                    }
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, commandName);
                result.Successed = false;
            }
            return result;
        }
    }
}
