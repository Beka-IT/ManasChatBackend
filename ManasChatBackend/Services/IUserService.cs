using ManasChatBackend.Models;
using ManasChatBackend.ViewModels;

namespace ManasChatBackend.Services;

public interface IUserService
{
    User SignIn(string email, string password);

    Task<UserSignUpResponse> SignUp(UserSignUpRequest user);

    bool ConfirmCode(int code, string email);
}