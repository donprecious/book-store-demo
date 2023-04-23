using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BookStore.Application.Account.Interface;
using BookStore.Application.Account.Model;
using BookStore.Application.Common.Models;
using BookStore.Application.Email;
using BookStore.Application.Email.Models;
using BookStore.Application.Exceptions;

using BookStore.Domain.Entities.ModelToDelete.IdentityModels;
using Identity.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharedKernel.Model;
using SharedKernel.Utility;

namespace Identity.Services.Concrete
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSettings _jwtSettings;
        private readonly IEmailService _emailService;
        private IEmailTemplate _emailTemplate;
        private IConfiguration _configuration;
        private IdentityContext _context;
        private ILogger<AccountService> _logger;
        public AccountService(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<JWTSettings> jwtSettings,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService, IdentityContext context, IConfiguration configuration, ILogger<AccountService> logger, IEmailTemplate emailTemplate)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _emailService = emailService;
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _emailTemplate = emailTemplate;
        }
        public async Task<Result<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(request.Email.Trim());
            if (user == null)
            {
                return Result<AuthenticationResponse>.Failure(new []{$"Invalid Credentials for '{request.Email}"} , $"Invalid Credentials for '{request.Email}");
              
            }
            
            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, false, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
            {
                return Result<AuthenticationResponse>.Failure(new []{$"Invalid Credentials for '{request.Email}"} , $"Invalid Credentials for '{request.Email}");

              
            }

         
        
            string ipAddress = IpHelper.GetIpAddress();
            var token = await GenerateJWToken(user, ipAddress);
            JwtSecurityToken jwtSecurityToken = token.jwtSecurityToken;
            AuthenticationResponse response = new AuthenticationResponse();
            response.User.Id = user.Id.ToString();
            response.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.User.Email = user.Email;
            response.User.UserName = user.UserName;
            IList<string> rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.User.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.User.PhoneNumber = user.PhoneNumber;
            response.RefreshToken = await GenerateRefreshToken(user);
       
         
            response.TokenExpireTime = token.tokenLifetime;
           
            
            return  Result<AuthenticationResponse>.Success(response, $"Authenticated {user.UserName}");
        }

        public async Task<Result<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequestByPhone request)
        {
            
            ApplicationUser user = await  _userManager.Users.Where(a=>a.PhoneNumber == request.Phone).FirstOrDefaultAsync();
          
            if (user == null)
            {
                return Result<AuthenticationResponse>.Failure(new []{$"Invalid Credentials for '{request.Phone}"} , $"Invalid Credentials for '{request.Phone}");
            }
            
            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, false, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
            {
                return Result<AuthenticationResponse>.Failure(new []{$"Invalid Credentials for '{request.Phone}"} , $"Invalid Credentials for '{request.Phone}");

              
            }
         
        
            string ipAddress = IpHelper.GetIpAddress();
            var token = await GenerateJWToken(user, ipAddress);
            JwtSecurityToken jwtSecurityToken = token.jwtSecurityToken;
            AuthenticationResponse response = new AuthenticationResponse();
            response.User.Id = user.Id.ToString();
            response.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.User.Email = user.Email;
            response.User.UserName = user.UserName;   
            response.User.FirstName = user.FirstName;
            response.User.LastName = user.LastName;
            IList<string> rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.User.Roles = rolesList.ToList();
            response.IsVerified = user.PhoneNumberConfirmed;
            response.User.PhoneNumber = user.PhoneNumber;
            response.RefreshToken = await GenerateRefreshToken(user);
       
         
            response.TokenExpireTime = token.tokenLifetime;
           
            
            return  Result<AuthenticationResponse>.Success(response, $"Authenticated {user.UserName}");
        }

        public async Task<Result<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                
                return  Result<string>.Success(user.Id.ToString(), message: $"Account Confirmed for {user.Email}. You can now use the /api/Account/authenticate endpoint.");
            }
            else
            {
                var errors = result.Errors.Select(a => a.Description).ToArray();
                return Result<string>.Failure(errors);

            }
        }

        public async Task<Result<string>> ConfirmEmailOtpAsync(string email, string code)
        {
            var user = await _userManager.Users.Where(a=>a.Email == email).FirstOrDefaultAsync();
            if (user == null)
            {
                return Result<string>.Failure($"Invalid or expired otp");
            
            }
            
            if (code != user.ResetPasswordToken || string.IsNullOrEmpty(code))
            {
                return Result<string>.Failure($"Invalid or expired otp");
                
            }

            if (DateTime.UtcNow > user.ResetPasswordTokenExpirationTime)
            {
                return Result<string>.Failure($"Invalid or expired otp");
            }
        

            user.EmailConfirmed = true;
            user.ResetPasswordToken = "";
            user.ResetPasswordTokenExpirationTime = null;
            await _userManager.UpdateAsync(user);
            return Result<string>.Success(null, "email  verified");
           
            
        } 
   
        public async Task<Result<string>> ConfirmPhoneAsync(string phoneNumber, string code)
        {
            var user = await _userManager.Users.Where(a=>a.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
            if (user == null)
            {
                return Result<string>.Failure($"Invalid or expired otp");
            
            }
            
            if (code != user.ResetPasswordToken || string.IsNullOrEmpty(code))
            {
                return Result<string>.Failure($"Invalid or expired otp");
                
       }

            if (DateTime.UtcNow > user.ResetPasswordTokenExpirationTime)
            {
                return Result<string>.Failure($"Invalid or expired otp");
            }
        

            user.PhoneNumberConfirmed = true;
            user.ResetPasswordToken = "";
            user.ResetPasswordTokenExpirationTime = null;
            await _userManager.UpdateAsync(user);
            return Result<string>.Success(null, "phone number verified");
           
            
        } 
        
        
        public async Task ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return;

            
            var code = Guid.NewGuid().ToString("N");
            user.ResetPasswordToken = code; 
            user.ResetPasswordTokenExpirationTime = DateTime.Now.AddMinutes(20);
            await  _userManager.UpdateAsync(user);
                // await _userManager.GeneratePasswordResetTokenAsync(user);
            var frontUrl = _configuration.GetValue<string>("Frontend:BaseUrl"); 
         
            var route = frontUrl+$"account/reset-password?email={user.Email}&token={code}";

            _logger.LogInformation("reset password link "+ route );
            var emailContent = _emailTemplate.GetPasswordResetEmail(user.FirstName, route);
            var emailRequest = new EmailRequest()
            {
                Body = emailContent
                ,
                To = request.Email,
                Subject = "Reset Password ",
            };
            await _emailService.SendAsync(emailRequest);
            
        }

       
    
        public async Task<Result<string>> LogoutAsync(string userEmail)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);
            if (user != null)
            {
                await _userManager.RemoveAuthenticationTokenAsync(user, "MyApp", "RefreshToken");
            }
            await _signInManager.SignOutAsync();

            return Result<string>.Success(userEmail, message: $"Logout.");
        }

        public async Task<Result<AuthenticationResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result<AuthenticationResponse>.Failure($"You are not registered with '{request.Email}'.");
            }
            if (!user.EmailConfirmed)
            {
                return Result<AuthenticationResponse>.Failure($"Account Not Confirmed for '{request.Email}'.");
            }

            string refreshToken = await _userManager.GetAuthenticationTokenAsync(user, "MyApp", "RefreshToken");
            bool isValid = await _userManager.VerifyUserTokenAsync(user, "MyApp", "RefreshToken", request.Token);
            if (!refreshToken.Equals(request.Token) || !isValid)
            {
                return Result<AuthenticationResponse>.Failure($"Your token is not valid..");

            }

            string ipAddress = IpHelper.GetIpAddress();
            var token = await GenerateJWToken(user, ipAddress);

            JwtSecurityToken jwtSecurityToken = token.jwtSecurityToken; 
            AuthenticationResponse response = new AuthenticationResponse();
            response.User.Id = user.Id.ToString();
            response.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.User.Email = user.Email;
            response.User.UserName = user.UserName;
            IList<string> rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.User.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.TokenExpireTime = token.tokenLifetime;
            response.RefreshToken = await GenerateRefreshToken(user);

            await _signInManager.SignInAsync(user, false);
            return  Result<AuthenticationResponse>.Success(response, $"Authenticated {user.UserName}");
        }

        public async Task<Result<string>> RegisterAsync(RegisterRequest request, string uri="")
        {
            ApplicationUser findUser = await _userManager.FindByNameAsync(request.UserName);
            if (findUser != null)
            {

                return Result<string>.Failure($"Username '{request.UserName}' is already taken.");
            }
            findUser = await _userManager.FindByEmailAsync(request.Email);
            if (findUser != null)
            {
                return Result<string>.Failure($"Email {request.Email} is already registered.");
            }

            // var findUserByPhone = await _userManager.Users.Where(a => a.PhoneNumber == request.PhoneNumber).FirstOrDefaultAsync();
            // if (findUserByPhone != null)
            // {
            //     return Result<string>.Failure($"Phone number {request.PhoneNumber} is already registered.");
            // }

        
            
            ApplicationUser newUser = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                
                
            };
           
            var result = await _userManager.CreateAsync(newUser, request.Password);
            if (result.Succeeded)
            {
                
                // await _userManager.AddToRoleAsync(newUser, Roles.MerchantAdmin.ToString());

                //todo send otp 
                // var verificationUri = await SendVerificationEmail(newUser, uri);
                await SendEmailConfirmationOtp(request.Email, "123456");
              
                return Result<string>.Success(newUser.Id.ToString(), message: $"User Registered. Kindly confirm your OTP");
            }
            else
            {
                var errors = result.Errors.Select(a => a.Description).ToArray();
                return Result<string>.Failure(errors, errors[0]);
            }
        }

        public async Task<Result<string>> SendPhoneNumberConfirmationSms(string phoneNumber, string otp= "")
        {
            if (string.IsNullOrEmpty(otp))
            {
                otp = RandomNumbers.GenerateTrackingNumber(6); 
            }
            var findUserByPhone = await _userManager.Users.Where(a => a.PhoneNumber == phoneNumber).FirstOrDefaultAsync();
            if (findUserByPhone != null)
            {
                if (findUserByPhone.PhoneNumberConfirmed)
                {
                    return Result<string>.Success($"Phone number already confirmed");
                }
                
                findUserByPhone.ResetPasswordToken = otp; 
                var otpDuration = _configuration.GetValue<int>("OtpDurationInMinutes");
                findUserByPhone.ResetPasswordTokenExpirationTime = DateTime.UtcNow.AddMinutes(otpDuration);
                await   _userManager.UpdateAsync(findUserByPhone); 
                return Result<string>.Success($"otp sent");
            }
            
            return Result<string>.Failure($"Failed to sent otp account may not exist");
            
        }
        
        public async Task<Result<string>> SendEmailConfirmationOtp(string email, string otp= "")
        {
            if (string.IsNullOrEmpty(otp))
            {
                otp = RandomNumbers.GenerateTrackingNumber(6); 
            }
            var findUserByPhone = await _userManager.Users.Where(a => a.Email == email).FirstOrDefaultAsync();
            if (findUserByPhone != null)
            {
                if (findUserByPhone.EmailConfirmed)
                {
                    return Result<string>.Success($"email already confirmed");
                }
                
                findUserByPhone.ResetPasswordToken = otp; 
                var otpDuration = _configuration.GetValue<int>("OtpDurationInMinutes");
                findUserByPhone.ResetPasswordTokenExpirationTime = DateTime.UtcNow.AddMinutes(otpDuration);
                await   _userManager.UpdateAsync(findUserByPhone); 
                return Result<string>.Success($"otp sent");
            }
            
            return Result<string>.Failure($"Failed to sent otp account may not exist");
        }
        public async Task<Result<string>> SendForgetPasswordEmailOtp(string email, string otp= "")
        {
            if (string.IsNullOrEmpty(otp))
            {
                otp = RandomNumbers.GenerateTrackingNumber(6); 
            }
            var findUserByPhone = await _userManager.Users.Where(a => a.Email == email).FirstOrDefaultAsync();
            if (findUserByPhone != null)
            {
               
                findUserByPhone.ResetPasswordToken = otp; 
                var otpDuration = _configuration.GetValue<int>("OtpDurationInMinutes");
                findUserByPhone.ResetPasswordTokenExpirationTime = DateTime.UtcNow.AddMinutes(otpDuration);
                await   _userManager.UpdateAsync(findUserByPhone); 
                return Result<string>.Success($"otp sent");
            }
            
            return Result<string>.Failure($"Failed to sent otp account may not exist");
        }
        public async Task<Result<string>> ConfirmPasswordOtpByEmailAsync(string email,  string newPassword, string code)
        {
            var user = await _userManager.Users.Where(a=>a.Email == email).FirstOrDefaultAsync();
            if (user == null)
            {
                return Result<string>.Failure($"Invalid or expired otp");
            
            }
            
            if (code != user.ResetPasswordToken || string.IsNullOrEmpty(code))
            {
                return Result<string>.Failure($"Invalid or expired otp");
                
            }

            if (DateTime.UtcNow > user.ResetPasswordTokenExpirationTime)
            {
                return Result<string>.Failure($"Invalid or expired otp");
            }

            var hash = _userManager.PasswordHasher.HashPassword(user, newPassword);
            user.PasswordHash = hash;

            user.ResetPasswordToken = "";
            user.ResetPasswordTokenExpirationTime = null;
            await _userManager.UpdateAsync(user);
            return Result<string>.Success(null, "Password Reset successful");
        } 

        public async Task<Result<string>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return  Result<string>.Success(null, message: $" You are not registered with '{request.Email}' .");

            }

            if (user.ResetPasswordToken != request.Token)
            {
                return Result<string>.Success(null, message: $"Invalid or expired token .");
            }

            if (DateTime.Now > user.ResetPasswordTokenExpirationTime)
            {
                return Result<string>.Success(null, message: $"expired token .");
            }

            var generatedResetToken = await _userManager.GeneratePasswordResetTokenAsync(user); 
        
            var result = await _userManager.ResetPasswordAsync(user, generatedResetToken, request.Password);
            if (result.Succeeded)
            {
                return  Result<string>.Success(request.Email, message: $"Successfully resetted the password");
            }
            else
            {
                var messageList =  result.Errors.Select(a => a.Description).ToList();
                var message = string.Join(',', messageList);
               return  Result<string>.Failure(messageList.ToArray(), message);
            }
        }

        public async Task<List<ApplicationUser>> GetUsers()
        {
            return await _userManager.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ToListAsync(); //lazzyloading
        }

        private async Task<(JwtSecurityToken jwtSecurityToken, DateTime tokenLifetime)> GenerateJWToken(ApplicationUser user,  string ipAddress)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
                roleClaims.Add(new Claim(ClaimTypes.Role, roles[i]));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim("ip", ipAddress),
               
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var tokenLifeTime = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: tokenLifeTime,
                signingCredentials: signingCredentials);
            return (jwtSecurityToken, tokenLifeTime);
        }

        private async Task<string> GenerateRefreshToken(ApplicationUser user)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, "MyApp", "RefreshToken");
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, "MyApp", "RefreshToken");
            IdentityResult result = await _userManager.SetAuthenticationTokenAsync(user, "MyApp", "RefreshToken", newRefreshToken);
            if (!result.Succeeded)
            {
               
                throw new ApiException($"An error occured while set refreshtoken.") { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
            return newRefreshToken;
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private async Task<string> SendVerificationEmail(ApplicationUser newUser, string uri)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/account/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{uri}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", newUser.Id.ToString());
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);

            var emailContent = _emailTemplate.GetRegistrationVerifcationEmail(newUser.FirstName, uri);
            var emailRequest = new EmailRequest()
            {
                Body = emailContent,
                To = newUser.Email,
                Subject = "Confirm Registration",
            };
            await _emailService.SendAsync(emailRequest);
           

            return verificationUri;
        }
    }
}
