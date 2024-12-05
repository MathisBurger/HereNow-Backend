namespace PresenceBackend.Models.Request;

/// <summary>
/// Request to change foreign password
/// </summary>
public class ChangeForeignPasswordRequest
{
    /// <summary>
    /// ID of the user to change password
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// The new password
    /// </summary>
    public string NewPassword { get; set; }
}