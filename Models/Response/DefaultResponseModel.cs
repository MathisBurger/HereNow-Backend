namespace PresenceBackend.Models.Response;

/// <summary>
/// Model for default response
/// </summary>
/// <param name="status"></param>
/// <param name="message"></param>
/// <param name="version"></param>
public class DefaultResponseModel(string status, string message, string version)
{
    public string Status { get; } = status;
    public string Message { get; } = message;
    public string Version { get; } = version;
}