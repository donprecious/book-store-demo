using Identity.Contexts;
using Identity.Services.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;
using System;
using System.Text;
using System.Text.Json;
using BookStore.Application.Account.Interface;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities.ModelToDelete.IdentityModels;

using Identity.Services;


namespace Identity
{
    public static class ServiceExtensions
    {
        public static void AddPersistanceIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            #region Services
    
            #endregion

            #region Configure
            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            #endregion

            services.AddDbContext<IdentityContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)))
              
                ;
       
            services.AddScoped<IAccountService, AccountService>();
            
            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders()
                .AddTokenProvider("MyApp", typeof(DataProtectorTokenProvider<ApplicationUser>));

            services.AddAuthentication(options =>
               {
                   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                
               })
               .AddJwtBearer(o =>
               {
                   o.RequireHttpsMetadata = false;
                   o.SaveToken = false;
                   o.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.Zero,
                       ValidIssuer = configuration["JWTSettings:Issuer"],
                       ValidAudience = configuration["JWTSettings:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                   };
                
               });

            services.AddScoped<IAuthorizationService, AuthorizationService>();
          

        }
    }
}
