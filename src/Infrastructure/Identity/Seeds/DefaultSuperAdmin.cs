using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Domain.Entities.ModelToDelete.IdentityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Seeds
{
    public static class DefaultSuperAdmin
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, IdentityDbContext context)
        {

        }
    }
}
