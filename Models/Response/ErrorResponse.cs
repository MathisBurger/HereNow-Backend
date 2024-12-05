namespace PresenceBackend.Models.Response;

/// <summary>
/// Error response
/// </summary>
/// <param name="message"></param>
/// <param name="statusCode"></param>
public class ErrorResponse(string message, int statusCode)
{
    public string Message { get; set; } = message;
    public int StatusCode { get; set; } = statusCode;
}