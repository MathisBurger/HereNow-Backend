namespace PresenceBackend.Models.Response;

public class DefaultResponseModel(string status, string message, string version)
{
    public string Status { get; } = status;
    public string Message { get; } = message;
    public string Version { get; } = version;
}