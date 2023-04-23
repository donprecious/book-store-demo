using BookStore.Domain.Interface;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Domain.Entities.ModelToDelete.IdentityModels
{
    public class ApplicationUser : IdentityUser<string>, IAuditable
    {

        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";



        public ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>() { };

        public string CreatedBy { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string ResetPasswordToken { get; set; } = "";
        public DateTime? ResetPasswordTokenExpirationTime { get; set; }

        public bool? Active { get; set; } = true;

        public ApplicationUser()
        {
         Id =   Guid.NewGuid().ToString();
        }
        
    }
}
