using BookStore.Application.Account.Model;
using BookStore.Application.Common.Models;
using BookStore.Domain.Entities.ModelToDelete.IdentityModels;

namespace BookStore.Application.Account.Interface
{
    public interface IAccountService
    {
        Task<Result<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
        Task<Result<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequestByPhone request);
        Task<Result<string>> RegisterAsync(RegisterRequest request, string uri="");
        Task<Result<string>> SendEmailConfirmationOtp(string email, string otp = "");
        Task<Result<string>> ConfirmEmailAsync(string userId, string code);
        Task<Result<string>> ConfirmEmailOtpAsync(string email, string code);
        Task ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<Result<string>> ResetPasswordAsync(ResetPasswordRequest request);
        Task<Result<string>> SendForgetPasswordEmailOtp(string email, string otp = "");
        Task<Result<string>> ConfirmPasswordOtpByEmailAsync(string email, string newPassword, string code);
        Task<Result<AuthenticationResponse>> RefreshTokenAsync(RefreshTokenRequest request);
        Task<Result<string>> LogoutAsync(string userEmail);
        Task<List<ApplicationUser>> GetUsers();

        Task<Result<string>> ConfirmPhoneAsync(string phoneNumber, string code);
        Task<Result<string>> SendPhoneNumberConfirmationSms(string phoneNumber, string otp = "");

    }
}
