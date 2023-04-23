using Microsoft.AspNetCore.Identity;

namespace BookStore.Domain.Entities.ModelToDelete.IdentityModels
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}
