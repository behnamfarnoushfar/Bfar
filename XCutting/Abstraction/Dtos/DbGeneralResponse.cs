namespace Bfar.XCutting.Abstractions.Dtos
{
    public sealed class DbGeneralResponse
    {
        public long Result { get; set; }
        public int Code { get; set; }
        public string? Msg { get; set; }
    }
}
