namespace Bfar.XCutting.Abstractions.Adapters
{
    public interface ICircuitBreakerAdapter
    {
        public bool IsClosed { get; set; }
        public bool IsOpen { get; set; }
        public bool IsHalfOpen { get; set; }
        public bool Isolated { get; set; }
        string GetCurrentConnection();
        int GetCurrentTimeout();
        bool ShouldAnswer();
        void Trip<TError>(TError? error, params object[]? args) where TError : Exception;
        void Failure();
        void Close();
        void Try();
    }
}
