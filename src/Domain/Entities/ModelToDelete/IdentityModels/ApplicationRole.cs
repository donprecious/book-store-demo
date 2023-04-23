using Microsoft.AspNetCore.Identity;

namespace BookStore.Domain.Entities.ModelToDelete.IdentityModels
{
    public class ApplicationRole : IdentityRole<string>
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
