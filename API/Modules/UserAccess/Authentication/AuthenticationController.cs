using API.Configuration.AuthorizationHelpers;
using Microsoft.AspNetCore.Mvc;
using Modules.UserAccess.Application.Authentication.Authenticate;
using Modules.UserAccess.Application.Authentication.Refresh;
using Modules.UserAccess.Application.Contracts;

namespace API.Modules.UserAccess.Authentication;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthenticationController(IUserAccessModule userAccessModule) : Controller
{
    [HttpPost]
    [NoPermissionRequired]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request)
    {
        var result = await userAccessModule.ExecuteCommandAsync(new AuthenticateCommand(request.Email, request.Password));

        if (result.IsAuthenticated)
        {
            return Ok(result.Tokens);
        }
        
        return Unauthorized(result.Error);
    }
    
    [HttpPost]
    [NoPermissionRequired]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var result = await userAccessModule.ExecuteCommandAsync(new RefreshCommand(request.RefreshToken, request.AccessToken));

        if (result.IsAuthenticated)
        {
            return Ok(result.Tokens);
        }
        
        return Unauthorized(result.Error);
    }
}