namespace Bfar.XCutting.Abstractions.Models
{
    public abstract class ViewElementModel
    {
        protected ViewElementModel()
        {
            IsDisabled = false;
            IsVisible = true;
            IsHidden = false;
        }
        public required string Name { get; set; }
        public required string ElementType { get; set; }
        public required string Id { get; set; }
        public string? Title { get; set; }
        public string[]? InlineStyles { get; set; }
        public NameValueModel[]? Attributes { get; set; }
        public NameValueModel[]? Bindings { get; set; }
        public NameValueModel[]? InlineEvents { get; set; }
        public bool IsHidden { get; set; }
        public bool IsVisible { get; set; }
        public bool IsDisabled { get; set; }
    }
}
