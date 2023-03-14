using System.Net.Mail;
using System.Text.RegularExpressions;
using MailKit.Net.Smtp;
using MailKit.Security;
using ManasChatBackend.Helpers;
using ManasChatBackend.Models;
using ManasChatBackend.ViewModels;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace WebApi.Managers;

public class UserHelper
{
    private readonly AppDbContext _db;

    public UserHelper(AppDbContext db)
    {
        this._db = db;
    }

    public async Task<bool> IsValidUserAsyncForLogin(string login, string password)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == login);

        if (user is not null)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }

        return false;
    }
    
    public async Task<UserSignUpResponse> CreateUser(UserSignUpRequest user)
    {
        Regex regex = new Regex(@"^([\w\.\-]+)@manas.edu.kg");
        
        Match match = regex.Match(user.Email);

        if (!match.Success)
        {
            return new UserSignUpResponse()
            {
                User = null,
                message = "Вы можете зарегистрироваться только через email КТУМ"
            };
        }
        
        if (user.Password == "")
        {
            return new UserSignUpResponse()
            {
                User = null,
                message = "Введите пароль!"
            };
        }

        var existedUser = await _db.Users.FirstOrDefaultAsync(a => a.Email == user.Email);

        if (existedUser != null)
        {
            return new UserSignUpResponse()
            {
                User = null,
                message = "Это имя пользователя уже занято!"
            };
        }

        var newAccount = new User()
        {
            Email = user.Email,
            Fullname = user.Fullname,
            FacultyId = user.FacultyId,
            DepartmentId = user.DepartmentId,
            Course = user.Course,
            YearOfAdmission = user.YearOfAdmission,
            Color = ColorGenerator.GenerateColor(),
            IsActive = false,
            IsVerify = false
        };
        
        newAccount.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        await _db.Users.AddAsync(newAccount);
        await _db.SaveChangesAsync();
        newAccount = await _db.Users.FirstOrDefaultAsync(a => a.Email == user.Email);
        var confirmCode = await GenerateConfirmCode(newAccount);
        SendConfirmCodeToEmail(confirmCode, newAccount.Email);
        
        return new UserSignUpResponse()
        {
            User = newAccount,
            message = "Вы успешно зарегистрировались!"
        };
        
    }

    public void VerifyUserByEmail(string email)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == email);
        user.IsVerify = true;
        var confirmationCode = _db.ConfirmationCodes.FirstOrDefault(c => c.UserId == user.Id);
        _db.ConfirmationCodes.Remove(confirmationCode);
        _db.SaveChanges();
    }
    
    public void ActivateUserByEmail(string email)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == email);
        user.IsActive = true;
        _db.SaveChanges();
    }
    
    private async Task<int> GenerateConfirmCode(User user)
    {
        var random = new Random();
        var code = random.Next(100000, 999999);
        var userId = _db.Users.FirstOrDefault(u => u.Email == user.Email).Id;
        
        var confirmationCode = new ConfirmationCode()
        {
            UserId = userId,
            Code = code
        };
        await _db.ConfirmationCodes.AddAsync(confirmationCode);
        await _db.SaveChangesAsync();
        
        return code;
    }

    public bool IsConfirmCodeValid(int code, string email)
    {
        var userId = _db.Users.FirstOrDefault(u => u.Email == email).Id;
        return  _db.ConfirmationCodes
            .Any(c => c.UserId == userId && c.Code == code);
    }


    public void SendConfirmCodeToEmail(int code, string userEmail)
    {
        var email = new MimeMessage();

        email.From.Add(MailboxAddress.Parse("afaricanistan@gmail.com"));

        email.To.Add(MailboxAddress.Parse(userEmail));

        email.Subject = "Title";

        email.Body = new TextPart(TextFormat.Html) { Text = $"<h1>{code}</h1>" };

 

// send email

        using var smtp = new SmtpClient();

        smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

        smtp.Authenticate("1904.01028@manas.edu.kg", "Alina251001");

        smtp.Send(email);

        smtp.Disconnect(true);
    }

}