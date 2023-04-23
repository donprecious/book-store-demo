namespace BookStore.Application.Email;

public interface IEmailTemplate
{
    string GetRegistrationVerifcationEmail(string firstname, string resetUrl);
    string GetPasswordResetEmail(string firstname, string resetUrl);
    string GetDefault(string htmlContent);
}