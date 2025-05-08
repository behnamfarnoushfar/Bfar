namespace Bfar.XCutting.Abstractions.Models
{
    public sealed class EndPointConfigModel
    {
        public required string TypeName { get; set; }
        public required string Host { get; set; }
        public required int Port { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Authorization { get; set; }
        public string? Cert { get; set; }
        public int? TlsVersion { get; set; }
        public string? ConnectionString { get; set; }
        public Dictionary<string,string>? ExtraOptions { get; set; }

    }
}
