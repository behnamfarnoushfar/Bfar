namespace Bfar.XCutting.Abstractions.Models
{
    public class InputViewModel : ViewElementModel
    {

        public required string InputType { get; set; }

        public string? Hint { get; set; }
        public string[]? ClassStyles { get; set; }

        public NameValueModel[]? Values { get; set; }
        public NameValueModel? DefaultValue { get; set; }

        public InputViewModel[]? SubInputs { get; set; }
    }
}
