using Bfar.XCutting.Abstractions.Models;

namespace Bfar.XCutting.Abstractions.ApplicationServices
{
    public interface INotificationService<TMessage,TMessageId>
    {
        public Task<Memory<byte>?> QueueNotificationAsync(TMessage notification);
        public Task<Memory<byte>?> SendNotificationAsync(TMessage notification, CancellationToken cancellationToken = default);
        public Task<decimal> GetRemainCreditAsync();
        public Task<bool> GetDeliveryReportAsync(TMessageId id);
        INotificationService<NotificationModel, string> SetConfiguration(EndPointConfigModel endPointConfigModel);
    }
}
