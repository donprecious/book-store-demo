using BookStore.Application.Email.Models;

namespace BookStore.Application.Email;

public interface IEmailService
{
    Task SendAsync(EmailRequest request);
}