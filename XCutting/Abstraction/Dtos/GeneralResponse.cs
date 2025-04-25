namespace Bfar.XCutting.Abstractions.Dtos
{
    public sealed class GeneralResponse<T>
    {
        public byte[]? TraceMask { get; set; }
        public int ResultCode { get; set; }
        public string? OperationDescription { get; set; }
        public string? BusinessDescription { get; set; }
        public string? UID { get; set; }
        public bool IsSuccessFull { get; set; }
        public T? Content { get; set; }
    }
}
