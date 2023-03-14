using ManasChatBackend.Models;
using ManasChatBackend.ViewModels;

namespace ManasChatBackend.Services;

public interface IUserService
{
    Task<bool> IsValidSignIn(string email, string password);

    Task<UserSignUpResponse> SignUp(UserSignUpRequest user);
    void ActivateUser(string email, string executorEmail);
    bool ConfirmCode(int code, string email);
}