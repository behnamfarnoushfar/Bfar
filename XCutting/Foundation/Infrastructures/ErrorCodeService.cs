using System.Text;

public sealed class ErrorCodeService : IErrorLookupService
{
    private const string invalidErrorCode = "Invalid Error Code.";
    private const string unknownLayer = "Unknown Layer";
    private const string unknownResult = "Unknown Result";
    private const string unknownType = "Unknown Type";
    private const string unknownDetail = "Unknown Detail";
    private const string separator = " - ";
    private readonly short layerCount;
    private readonly string[][] errorCodeParts;
    private readonly string zero = "0";

    public ErrorCodeService(short layerCount = 4)
    {
        this.layerCount = layerCount;
        errorCodeParts = new string[layerCount][];
        for (int i = 0; i < layerCount; i++)
            errorCodeParts[i] = new[] { zero, zero, zero };
    }

    #region Dictionaries
    private static readonly Dictionary<string, string> ResultCodes = new()
    {
        { "200", "Success" },
        { "400", "Client Error" },
        { "500", "Server Error" }
    };

    private static readonly Dictionary<int, string> SystemLayers = new()
    {
        { 3, "Application Layer - Authentication" },
        { 2, "Domain Layer - Business Logic" },
        { 1, "Service Layer - Business Logic" },
        { 0, "Infrastructure Layer - Database Operations" }
    };

    private static readonly Dictionary<string, string> ErrorTypes = new()
    {
        { "1", "Validation Error" },
        { "2", "Database Error" },
        { "3", "Timeout Error" },
        { "4", "Service Failure - Circuit Breaker Triggered" }
    };

    private static readonly Dictionary<string, string> ErrorDetail = new()
    {
        { "01", "Operation Completed Successfully" },
        { "02", "Required Field Missing" },
        { "03", "Duplicate Entry" },
        { "04", "Network Failure" },
        { "05", "Unauthorized Access" }
    };
    #endregion

    public string EncodeErrorCode(string status, int layer, string errorType, string detail)
    {
        if (layer < 0 || layer >= layerCount)
            return invalidErrorCode;

        errorCodeParts[layer][0] = status;
        errorCodeParts[layer][1] = errorType;
        errorCodeParts[layer][2] = detail;

        return !ResultCodes.ContainsKey(status) ||
               !SystemLayers.ContainsKey(layer) ||
               !ErrorTypes.ContainsKey(errorType) ||
               !ErrorDetail.ContainsKey(detail)
            ? invalidErrorCode
            : string.Join(string.Empty, errorCodeParts.Select(x => string.Join(string.Empty, x)));
    }

    public string? DecodeErrorCode(string errorCode)
    {
        if (string.IsNullOrEmpty(errorCode) || errorCode.Length != layerCount * 3)
            return null;

        StringBuilder builder = new();
        for (int i = 0; i < layerCount; i++)
        {
            builder.Append(SystemLayers.GetValueOrDefault(i, unknownLayer)).Append(separator);
            builder.Append(ResultCodes.GetValueOrDefault(errorCode.Substring(i * 6, 3), unknownResult)).Append(separator);
            builder.Append(ErrorTypes.GetValueOrDefault(errorCode.Substring(i * 6 + 3, 1), unknownType)).Append(separator);
            builder.Append(ErrorDetail.GetValueOrDefault(errorCode.Substring(i * 6 + 4, 2), unknownDetail));
        }
        return builder.ToString();
    }
}