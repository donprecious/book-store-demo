using BookStore.Application.Email;
using BookStore.Application.Email.Models;
using BookStore.Application.Exceptions;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using Services.Email.Model;
using MailSettings = Services.Email.Model.MailSettings;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        private SendGridSetting _sendGridSetting;
        private readonly ILogger<EmailService> _logger;
      
        public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger, IOptions<SendGridSetting> sendGridSetting)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
            _sendGridSetting = sendGridSetting.Value;
        }

        public async Task SendAsync(EmailRequest request)
        {
           await SendWithSendGrid(request);
        }

      async Task  SendWithSendGrid(EmailRequest request)
        {
            try
            {
                var client = new SendGridClient(_sendGridSetting.ApiKey);
                var from = new EmailAddress(request.From ?? _sendGridSetting.FromEmail,
                    _sendGridSetting.FromEmailUserName);
                var to = new EmailAddress(request.To);
                var message = MailHelper.CreateSingleEmail(from, to, request.Subject, request.Body, request.Body);
                var response = await client.SendEmailAsync(message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message);
            }
         
           
        }

   
     async Task SendWithSMTP(EmailRequest request)
      {
          try
          {
              var email = new MimeMessage();
              email.Sender = MailboxAddress.Parse(request.From ?? _mailSettings.EmailFrom);
              email.To.Add(MailboxAddress.Parse(request.To));
              email.Subject = request.Subject;
              var builder = new BodyBuilder();
              builder.HtmlBody = request.Body;
              email.Body = builder.ToMessageBody();
              using var smtp = new SmtpClient();
              smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
              smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
              await smtp.SendAsync(email);
              smtp.Disconnect(true);
          }
          catch (Exception ex)
          {
              _logger.LogError(ex.Message, ex);
              throw new ApiException(ex.Message);
          }
      }
        
    }
}
