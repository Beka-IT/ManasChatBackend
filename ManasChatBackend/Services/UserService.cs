using ManasChatBackend.Models;
using ManasChatBackend.ViewModels;
using WebApi.Managers;

namespace ManasChatBackend.Services;

public class UserService : IUserService
{
    private readonly UserHelper _userHelper;

    public UserService(UserHelper userHelper)
    {
        _userHelper = userHelper;
    }
    
    public User SignIn(string email, string password)
    {
        throw new NotImplementedException();
    }

    public async Task<UserSignUpResponse> SignUp(UserSignUpRequest user)
    {
        return await _userHelper.CreateUser(user);
    }
    
    public bool ConfirmCode(int code, string email)
    {
        var result = _userHelper.IsConfirmCodeValid(code, email);

        _userHelper.ActivateUserByEmail(email);

        return result;
    }
}