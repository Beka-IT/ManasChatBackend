using ManasChatBackend.Models;

namespace ManasChatBackend.ViewModels;

public class UserSignUpResponse
{
    public User User { get; set; } 
    public string message { get; set; }

}