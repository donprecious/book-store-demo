
using BookStore.Application.Account.Interface;
using BookStore.Application.Email;

using BookStore.Application.Interfaces;

using BookStore.Domain.Entities.ModelToDelete.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Identity.Services.Concrete
{
    public class ManageUser : IManageUser
    {
        private IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private IdentityDbContext _context;
        private IEmailService _emailService;
        private IEmailTemplate _emailTemplate;
        private IConfiguration _configuration;
        public ManageUser(IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager, IdentityDbContext context, IEmailService emailService, IConfiguration configuration,
            IEmailTemplate emailTemplate)
        {
            _authorizationService = authorizationService;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
            _emailTemplate = emailTemplate;
        }
 
     } 
     
     
    
}