using Bfar.XCutting.Abstractions.Entities.Dtos;
using Bfar.XCutting.Abstractions.Entities.Models;
using Bfar.XCutting.Abstractions.SharedKernel;
using Microsoft.Extensions.Logging;

namespace Bfar.XCutting.SharedKernel.Core.Commands
{
    public sealed class GenericQuery<TInput, TOutput> : IQueryService<TInput, TOutput> where TInput : class where TOutput : class
    {
        private readonly string commandName;
        private readonly IReceiver<TInput, TOutput> receiver;
        private readonly ILogger logger;

        public GenericQuery(ILoggerFactory loggerFactory, string commandName, IReceiver<TInput, TOutput> receiver)
        {
            this.commandName = commandName;
            this.receiver = receiver;
            logger = loggerFactory.CreateLogger(commandName);
        }
        public async Task<AppDto<TInput, TOutput>> QueryAsync(TInput model, PaginationModel? pagination = null, CancellationToken cancellationToken = default)
        {
            AppDto<TInput, TOutput> dto = new();
            try
            {
                if (model is null)
                {
                    dto.Successed = true;
                    dto.ResultDescription = "اطلاعات وارد شده نامعتبر است";
                }
                else
                {
                    IEnumerable<TOutput>? enums = null;
                    if (pagination is null)
                        dto.Result = await receiver.ReceiveAsync(model, cancellationToken);
                    else
                        enums = await receiver.ReceiveAsync(model, pagination.PageSize, pagination.OffSet, cancellationToken) as IEnumerable<TOutput>;
                    if (pagination is null && dto.Result is null)
                    {
                        dto.Successed = false;
                        dto.ResultDescription = "خطا در اجرای سرویس";
                        logger.LogError(dto.ResultDescription, commandName);
                    }
                    else if (pagination is null && dto.Result is not null)
                    {
                        dto.Successed = true;
                    }
                    else if (pagination is not null && enums?.Count() == 0)
                    {
                        dto.Successed = true;
                        dto.ResultDescription = "موردی یافت نشد";
                    }
                    else
                    {
                        dto!.Successed = true;
                        dto!.ResultDescription = $"{enums?.Count()} مورد یافت شد";
                    }
                    //else
                    //{
                    //    dto.TotalCount = dto.Result is Enumerable ? dto.Result;
                    //}
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, commandName);
                dto.Successed = false;
            }
            return dto;
        }
    }
}
