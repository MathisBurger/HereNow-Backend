using Microsoft.AspNetCore.Mvc;
using PresenceBackend.Models.Database;

namespace PresenceBackend.Controllers.v1;

public class AuthorizedControllerBase : ControllerBase
{
    public User? CurrentUser { get; set; }
}