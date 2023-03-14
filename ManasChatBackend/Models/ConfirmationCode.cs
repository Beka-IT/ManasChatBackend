namespace ManasChatBackend.Models;

public class ConfirmationCode
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int Code { get; set; }
}