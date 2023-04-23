using BookStore.Application.Interfaces;

using BookStore.Domain.Entities.ModelToDelete;
using BookStore.Domain.Entities.ModelToDelete.IdentityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Contexts
{
    public class IdentityContext
    :  IdentityDbContext<
        ApplicationUser, ApplicationRole, string,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
        ApplicationRoleClaim, ApplicationUserToken>
    {
        


        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
           
            
            base.OnModelCreating(builder);
            // builder.HasDefaultSchema("Identity"); 
            var defaultDbPrefix = "";
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: defaultDbPrefix +"User");
            });

            builder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable(name: defaultDbPrefix+ "Role");
            });

            builder.Entity<ApplicationUserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                entity.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
                entity.ToTable(defaultDbPrefix+"UserRoles");
            });

            builder.Entity<ApplicationUserClaim>(entity =>
            {
                entity.ToTable(defaultDbPrefix + "UserClaims");
            });

            builder.Entity<ApplicationUserLogin>(entity =>
            {
                entity.ToTable(defaultDbPrefix + "UserLogins");
            });

            builder.Entity<ApplicationRoleClaim>(entity =>
            {
                entity.ToTable(defaultDbPrefix + "RoleClaims");

            });

            builder.Entity<ApplicationUserToken>(entity =>
            {
                entity.ToTable(defaultDbPrefix + "UserTokens");
            });
            
            builder.ApplyConfigurationsFromAssembly(typeof(IdentityContext).Assembly);
          
        }
       
 
    }
}
