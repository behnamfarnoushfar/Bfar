namespace Bfar.XCutting.Abstractions.Models
{
    public sealed class MessageConstruct
    {
        public uint Command { get; set; }
        public string? Resource { get; set; }
        public List<NameValueModel>? Headers { get; set; }
        public DateTime MessageDateTime { get; set; }
        public string? Host { get; set; }
        public string? Source { get; set; }
        public string? Destination { get; set; }
        public Guid MessageId { get; set; }
        public string? Payload { get; set; }
        public bool DirectPayload { get; set; }
        public string? Parameters { get; set; }
        public List<MessageConstruct>? SubMessages { get; set; }
        public bool IsMultipart { get; set; }
        public Stream? InternalBuffer { get; set; }
        public string Title { get; set; }
        public string PayLoadType { get; set; }
        public string PayLoadName { get; set; }
        public string PayloadConfiguration { get; set; }
        public string PayloadGroup { get; set; }
        public List<NameValueModel>? FormParameters { get; set; }
    }
}
