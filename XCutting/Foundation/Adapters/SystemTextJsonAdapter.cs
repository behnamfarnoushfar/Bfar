using Bfar.XCutting.Abstractions.Adapters;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Bfar.XCutting.Foundation.Adapters
{
    public sealed class SystemTextJsonParserAdapter : IJsonParserAdapter
    {
        private readonly JsonSerializerOptions jsonOption;
        public SystemTextJsonParserAdapter()
        {
            jsonOption = new JsonSerializerOptions();
            jsonOption.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        }
        public T? Derialize<T>(string data) => string.IsNullOrEmpty(data) ? default : JsonSerializer.Deserialize<T>(data, jsonOption);

        public T? Derialize<T>(ReadOnlySpan<byte> span) => JsonSerializer.Deserialize<T>(span, jsonOption);

        public string Serialize<T>(T data) => JsonSerializer.Serialize(data, jsonOption);

        public ReadOnlyMemory<byte> SerializeBuffer<T>(T data) => JsonSerializer.SerializeToUtf8Bytes(data, jsonOption);
    }
}
