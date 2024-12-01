namespace PresenceBackend.Models.Request;

public class ChangeForeignPasswordRequest
{
    public Guid UserId { get; set; }
    public string NewPassword { get; set; }
}