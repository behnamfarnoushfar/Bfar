using Bfar.XCutting.Abstractions.ApplicationServices;
public interface IErrorLookupService
{
    string? DecodeErrorCode(string errorCode);
    string EncodeErrorCode(string status, int layer, string errorType, string detail);
}