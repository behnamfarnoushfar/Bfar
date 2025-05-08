using Bfar.XCutting.Abstractions.Enums;

namespace Bfar.XCutting.Abstractions.Models
{
    public class NotificationModel
    {
        public required string Content { get; set; }
        public required List<string> Receipients { get; set; }
        public string? Sender { get; set; }
        public required string Title { get; set; }
        public string? Id { get; set; }
        public bool HasHtml { get; set; }
        public int Priority { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? SendStartFrom { get; set; }
        public DateTime? SendEndTo { get; set; }
        public int MaxRetry { get; set; }
        public SendNotificationStatus Status { get; set; }
        public List<NotificationModel>? Childs { get; set; }
    }
}
