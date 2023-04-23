using BookStore.Domain.Common;

namespace BookStore.Domain.Entities.ModelToDelete;

public class TempRegistration : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; } 
    public string Email { get; set; }
    public string PhoneNumber { get; set; } 
    public string Password { get; set; }
    public string OTP { get; set; }
}