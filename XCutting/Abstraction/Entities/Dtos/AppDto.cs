namespace Bfar.XCutting.Abstractions.Entities.Dtos
{
    public sealed class AppDto<TModel,TResult>
    {
        public TModel? Parameters { get; set; }
        public TResult? Result { get; set; }
        public int ResultCode { get; set; }
        public string? ResultDescription { get; set; }
        public bool Successed { get; set; }
        public IEnumerable<TResult>? Results { get; set; }
    }
}
