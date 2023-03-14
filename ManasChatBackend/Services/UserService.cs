using ManasChatBackend.Models;
using ManasChatBackend.ViewModels;
using Server.Helpers;
using WebApi.Managers;

namespace ManasChatBackend.Services;

public class UserService : IUserService
{
    private readonly UserHelper _userHelper;

    public UserService(UserHelper userHelper)
    {
        _userHelper = userHelper;
    }
    
    public async Task<bool> IsValidSignIn(string email, string password)
    {
        return await _userHelper.IsValidUserAsyncForLogin(email, password);
    }

    public async Task<UserSignUpResponse> SignUp(UserSignUpRequest user)
    {
        return await _userHelper.CreateUser(user);
    }

    public void ActivateUser(string email, string executorEmail)
    {
        _userHelper.ActivateUserByEmail(email, executorEmail);
    }

    public bool ConfirmCode(int code, string email)
    {
        var result = _userHelper.IsConfirmCodeValid(code, email);

        if (result)
        {
            _userHelper.VerifyUserByEmail(email);
        }

        return result;
    }
}