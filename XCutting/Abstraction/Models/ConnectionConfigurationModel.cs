namespace Bfar.XCutting.Abstractions.Models
{
    public  sealed class ConnectionConfigurationModel
    {
        public int Port { get; set; }
        public int SendTimeout { get; set; }
        public int SendReceiveTimeout { get; set; }
        public int ConnectionTimeout { get; set; }
        public string? Host { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        /// <summary>
        /// Could be db name or Queue name
        /// </summary>
        public string? ResourceName { get; set; }
        /// <summary>
        /// Could be exchange name
        /// </summary>
        public string? SectionName { get; set; }
        public string? Cert { get; set; }
        public string? ConnectionString { get; set; }
    }
}
