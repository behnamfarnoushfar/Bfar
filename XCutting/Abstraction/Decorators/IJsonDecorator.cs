namespace Bfar.XCutting.Abstractions.Decorators
{
    public interface IJsonDecorator
    {
        public string Serialize<T>(T data);
        public T? Derialize<T>(string data);
        public T? Derialize<T>(ReadOnlySpan<byte> span);
        public ReadOnlyMemory<byte> SerializeBuffer<T>(T data);
    }
}
