namespace Bfar.XCutting.Foundation.Diagnostics
{
    public static class ThrowHelper
    {
        private const string circuitIsOpen = "CircuitIsOpen";
        private const string queueNameIsNotDeclare = "QueueNameIsNotDeclare";
        private static NotImplementedException nie = new NotImplementedException();

        public static void CircuitIsOpen(in string? connection, in string? subject)
        {
            throw new InvalidOperationException($"{circuitIsOpen} for {connection} by {subject}");
        }
        public static void NotFound(in string data)
        {
            throw new KeyNotFoundException(data);
        }
        public static void InvalidInputModel()
        {
            throw new InvalidDataException();
        }

        public static void InvalidInputModel(in string payload)
        {
            throw new InvalidDataException(payload);
        }

        public static void NotImplemented() => throw nie;

        public static void CannotObtainToken(in string result)
        {
            throw new UnauthorizedAccessException(result);
        }

        public static void QueueNameIsNotDeclare()
        {
            throw new ArgumentException(queueNameIsNotDeclare);
        }

        public static void CannotChangeRecordsInReposity(in string info)
        {
            throw new ArgumentException(info);
        }
    }
}
