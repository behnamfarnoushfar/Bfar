namespace Bfar.XCutting.Abstractions.Models
{
    public class SubViewModel : ViewElementModel
    {
        public SubViewModel()
        {
            HasError = false;
            IsUnAuthorized= false;
            IsUnderconstruction= false;
            HasWarning= false;
            IsSuccessFull = true;

        }
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
        public string? TargetResource { get; set; }
        public string? TargetAction { get; set; }
        /// <summary>
        /// None-Inuput elements
        /// </summary>
        public List<SubViewModel>? Childs { get; set; }
        /// <summary>
        /// only input elements
        /// </summary>
        public List<InputViewModel>? Inputs { get; set; }
    }
}
