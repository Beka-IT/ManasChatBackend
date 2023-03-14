using System.Security.Claims;
using ManasChatBackend.Services;
using ManasChatBackend.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> SignIn(string email, string password)
    {
        var isValidLogin = await _userService.IsValidSignIn(email, password);
        if (!isValidLogin)
        {
            return Unauthorized();
        }
        await Login(email);
        
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(UserSignUpRequest user)
    {
        await _userService.SignUp(user);
        await Login(user.Email);
            
        return Ok();
    }
    
    [Authorize]
    [HttpPost]
    public bool ConfirmEmail(int code)
    {
        string email;
        HttpContext.Request.Cookies.TryGetValue("Email", out email);
        return _userService.ConfirmCode(code, email);
    }

    [Authorize]
    [HttpGet]
    public IActionResult ActivateUser(string email)
    {
        string executorEmail;
        HttpContext.Request.Cookies.TryGetValue("Email", out executorEmail);

        _userService.ActivateUser(email, executorEmail);
        return Ok();
    }

    private async Task Login(string email)
    {
        HttpContext.Response.Cookies.Append("Email", email, 
            new CookieOptions { Expires = DateTime.Now.AddMinutes(30) });
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, email),
        };

        var claimsIdentity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme, 
            new ClaimsPrincipal(claimsIdentity));

    }
}