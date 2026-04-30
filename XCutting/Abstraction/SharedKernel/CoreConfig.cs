namespace Bfar.XCutting.Abstractions.SharedKernel
{
    public sealed class CoreConfig
    {
        public DomainConfig Domain { get; set; }
        public AppConfig App { get; set; }
        public SharedKernelPortConfig SharedKernelPortConfig { get; set; }

        public sealed class DomainConfig
        {
            public string ConnectionString { get; set; }
        }
        public sealed class AppConfig
        {
            public int AppId { get; set; }
            public string Authorization { get; set; }
            public string Host { get; set; }
        }
    }
}
