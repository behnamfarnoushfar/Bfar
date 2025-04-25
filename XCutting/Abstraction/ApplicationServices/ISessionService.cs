namespace Bfar.XCutting.Abstractions.ApplicationServices
{
    public interface ISessionService
    {
        public void Configure(int Timeout);
        public byte[]? this[string key] { get; set; }
        public bool IsAvailable {  get; }
        public string Id { get; }
        public void Clear();
        public bool Set<T>(string key,T value);
        public T Get<T>(string key);
        public bool Set(string key, byte[] value);
        public byte[]? Get(string key);
        public void Remove(string key);
        public string? GetToken();
    }
}
