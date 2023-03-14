namespace ManasChatBackend.ViewModels;

public class UserSignUpRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Fullname { get; set; }
    public int? FacultyId { get; set; }
    public int? DepartmentId { get; set; }
    public int? Course { get; set; }
    public int? YearOfAdmission { get; set; }
}