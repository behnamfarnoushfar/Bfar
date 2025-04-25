namespace Bfar.XCutting.Abstractions.Models
{
    public sealed class NameValueModel
    {
        public required string Name { get; set; }
        public int Order { get; set; }
        public bool IsSelected { get; set; }
        public string? Value { get; set; }
        public string? Hint { get; set; }
        public NameValueModel[]? Childs { get; set; }
    }
}
