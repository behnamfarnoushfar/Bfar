namespace Bfar.XCutting.Abstractions.Models
{
    public class InputFormModel
    {
        public required string Name { get; set; }
        public required string Id { get; set; }
        public required string FieldType { get; set; }
        public string? Title { get; set; }
        public string? Hint { get; set; }
        public string[]? ClassStyles { get; set; }
        public string[]? InlineStyles { get; set; }
        public NameValueModel[]? Attributes { get; set; }
        public NameValueModel[]? Bindings { get; set; }
        public NameValueModel[]? InlineEvents { get; set; }
        public string[]? Values { get; set; }
        public string? DefaultValue { get; set; }
        public bool IsHidden { get; set; }
        public bool IsVisible { get; set; }
        public bool IsDisabled { get; set; }
    }
}
