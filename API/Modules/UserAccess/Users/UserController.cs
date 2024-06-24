using API.Configuration.AuthorizationHelpers;
using Microsoft.AspNetCore.Mvc;
using Modules.UserAccess.Application.Contracts;
using Modules.UserAccess.Application.Users.CreateUser;
using Modules.UserAccess.Application.Users.EditUser;

namespace API.Modules.UserAccess.Users;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController(IUserAccessModule userAccessModule) : Controller
{
    [HttpPost]
    [NoPermissionRequired]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        var userId = await userAccessModule.ExecuteCommandAsync(new CreateUserCommand(
            request.Email,
            request.Password,
            request.FirstNameRu,
            request.LastNameRu,
            request.PatronymicRu,
            request.FirstNameEn,
            request.LastNameEn,
            request.PatronymicEn));

        return Ok(userId);
    }
    
    [HttpPost]
    [NoPermissionRequired]
    public async Task<IActionResult> Edit([FromBody] EditUserRequest request)
    {
        await userAccessModule.ExecuteCommandAsync(new EditUserCommand(
            request.UserId,
            request.Email,
            request.Password,
            request.FirstNameRu,
            request.LastNameRu,
            request.PatronymicRu,
            request.FirstNameEn,
            request.LastNameEn,
            request.PatronymicEn));

        return Ok();
    }
}