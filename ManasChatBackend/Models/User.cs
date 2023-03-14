namespace ManasChatBackend.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Fullname { get; set; }
    public string? Color { get; set; }
    public int? FacultyId { get; set; }
    public int? DepartmentId { get; set; }
    public int? Course { get; set; }
    public int? YearOfAdmission { get; set; }
    public bool IsVerify { get; set; }
    public bool IsActive { get; set; }
}