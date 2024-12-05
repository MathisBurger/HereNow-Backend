using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresenceBackend.Filters;
using PresenceBackend.Models.Database;
using PresenceBackend.Models.Request;
using PresenceBackend.Models.Response;
using PresenceBackend.Services;
using PresenceBackend.Shared;

namespace PresenceBackend.Controllers.v1;

/// <summary>
/// Auth controller
/// </summary>
[Route("v1/auth")]
[ApiController]
public class AuthController: AuthorizedControllerBase
{

    private readonly DbAccess Db;
    private readonly IAuthorization auth;

    public AuthController(DbAccess db, IAuthorization auth)
    {
        Db = db;
        this.auth = auth;
    }

    /// <summary>
    /// Gets current auth info
    /// </summary>
    [AllowAnonymous]
    [HttpGet("")]
    public IActionResult AuthInfo()
    {
        return new OkObjectResult(new DefaultResponseModel("OK", "v1 authorization is enabled", "v1.0.0"));
    }

    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <param name="registerRequest">The registration request</param>
    /// <returns>The created user</returns>
    [TypeFilter(typeof(AuthorizationFilter))]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized();
        }
        
        User? existingUser = await this.Db.UserRepository.FindUserByUsername(registerRequest.Username);
        if (existingUser is not null)
        {
            return BadRequest(new ErrorResponse("Nutzer existiert bereits.", StatusCodes.Status400BadRequest));
        }
        User user = await this.Db.UserRepository.RegisterUser(registerRequest);
        return Ok(user);
    }

    /// <summary>
    /// Logs in a user
    /// </summary>
    /// <param name="loginRequest">The login request</param>
    /// <returns>The refresh token</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!await this.Db.UserRepository.LoginUser(loginRequest))
        {
            return BadRequest(new ErrorResponse("Falscher Nutzername oder Passwort.", StatusCodes.Status400BadRequest));
        }
        User user = (await this.Db.UserRepository.FindUserByUsername(loginRequest.Username))!;
        return Ok(new TokenResponse(await this.auth.GenerateRefreshToken(user), "refresh_token"));
    }

    /// <summary>
    /// Renews a refresh token
    /// </summary>
    /// <param name="tokenRequest">The request with old token</param>
    /// <returns>The new token</returns>
    [HttpPost("refreshToken/renew")]
    public async Task<IActionResult> RenewRefreshToken([FromBody] TokenRequest tokenRequest)
    {
        User? sessionUser = await this.auth.ValidateRefreshToken(tokenRequest.Token);
        if (sessionUser is null)
        {
            return BadRequest(new ErrorResponse("Invalid refresh token.", StatusCodes.Status400BadRequest));
        }
        return Ok(new TokenResponse(await this.auth.GenerateRefreshToken(sessionUser), "refresh_token"));
    }

    /// <summary>
    /// Creates an access token
    /// </summary>
    /// <param name="tokenRequest">Refresh token</param>
    /// <returns>The access token</returns>
    [HttpPost("accessToken")]
    public async Task<IActionResult> GetAccessToken([FromBody] TokenRequest tokenRequest)
    {
        User? sessionUser = await this.auth.ValidateRefreshToken(tokenRequest.Token);
        if (sessionUser is null)
        {
            return BadRequest(new ErrorResponse("Invalid refresh token.", StatusCodes.Status400BadRequest));
        }
        return Ok(new TokenResponse(this.auth.GenerateAccessToken(sessionUser), "access_token"));
    }

    /// <summary>
    /// Logs out an user
    /// </summary>
    /// <param name="tokenRequest">The access token</param>
    /// <returns>Confirmation result</returns>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromQuery] TokenRequest tokenRequest)
    {
        User? sessionUser = await this.auth.ValidateRefreshToken(tokenRequest.Token);
        if (sessionUser is null)
        {
            return BadRequest(new ErrorResponse("Invalid refresh token.", StatusCodes.Status400BadRequest));
        }
        sessionUser.RefreshToken = null;
        this.Db.EntityManager.Update(sessionUser);
        await this.Db.EntityManager.SaveChangesAsync();
        return Ok();
    }
    
}