namespace PresenceBackend.Models.Response;

public class ErrorResponse(string message, int statusCode)
{
    public string Message { get; set; } = message;
    public int StatusCode { get; set; } = statusCode;
}