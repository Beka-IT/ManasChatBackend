using ManasChatBackend.Services;
using ManasChatBackend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ManasChatBackend.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<UserSignUpResponse> SignUp(UserSignUpRequest user)
    {
        return await _userService.SignUp(user);
    }
    
    [HttpPost]
    public async Task<bool> ConfirmEmail(int code)
    {
        var email = HttpContext.Session.GetString("Email");
        return _userService.ConfirmCode(code, email);
    }
}