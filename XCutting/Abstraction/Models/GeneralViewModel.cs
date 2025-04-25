namespace Bfar.XCutting.Abstractions.Models
{
    public sealed class GeneralViewModel
    {
        public bool IsUnderconstruction { get; set; }
        public bool IsSuccessFull { get; set; }
        public bool HasError { get; set; }
        public bool HasWarning { get; set; }
        public bool IsUnAuthorized { get; set; }
        public int CurrentCount { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public decimal TotalCalculation { get; set; }
        public string? ResultMessage { get; set; }
        public string? CurrentStyles { get; set; }
        public string? CurrentScripts { get; set; }
        List<GeneralViewModel>? SubResults { get; set; }
        List<InputFormModel>? Forms { get; set; }
    }
}
