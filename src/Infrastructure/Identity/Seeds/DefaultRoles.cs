using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using BookStore.Domain.Entities.ModelToDelete.IdentityModels;

namespace Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
        {
            //Seed Roles
            //
            // await roleManager.CreateAsync(new ApplicationRole()
            // {
            //     Name = Roles.Admin.ToString(),
            //     CreatedDate = DateTime.Now
            // });
            // await roleManager.CreateAsync(new ApplicationRole()
            // {
            //     Name = Roles.MerchantAdmin.ToString(),
            //     CreatedDate = DateTime.Now
            // });
            // await roleManager.CreateAsync(new ApplicationRole()
            // {
            //     Name = Roles.MerchantUser.ToString(),
            //     CreatedDate = DateTime.Now
            // });
        }
    }
}
