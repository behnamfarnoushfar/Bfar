namespace Bfar.XCutting.Abstractions.Entities.Dtos
{
    public sealed class RequestCaptchaDto
    {
        public required string ValidationKey { get; set; }
        public required string CaptchaValue { get; set; }
        public DateTime Expire { get; set; }
    }
}
