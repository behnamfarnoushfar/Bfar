namespace Bfar.XCutting.Abstractions.Dtos
{
    public sealed class DbGeneralResponse
    {
        public long Id { get; set; }
        public int Code { get; set; }
        public string? Message { get; set; }
    }
}
