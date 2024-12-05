using Microsoft.AspNetCore.Mvc;
using PresenceBackend.Models.Database;

namespace PresenceBackend.Controllers.v1;

/// <summary>
/// The controller base for all controllers with auth
/// </summary>
public class AuthorizedControllerBase : ControllerBase
{
    /// <summary>
    /// The current logged-in user
    /// </summary>
    public User? CurrentUser { get; set; }
}