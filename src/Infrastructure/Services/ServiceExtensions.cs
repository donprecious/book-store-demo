using BookStore.Application.Email;
using BookStore.Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Email;
using Services.Email.Model;
using Services.FileUpload;

namespace Services
{
    public static class ServiceExtensions
    {
        public static void AddSharedServices(this IServiceCollection services, IConfiguration config)
        {
            #region Configure

            // var mailSetting = new MailSettings();
            services.Configure<MailSettings>( config.GetSection("MailSettings"));
            services.Configure<SendGridSetting>(config.GetSection("SendGrid")); 
         
            #endregion
            
        
            #region Services
            services.AddSingleton<IEmailTemplate, EmailTemplates>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IFileUpload, AzureFileUpload>();

            #endregion

        }
      
    }
}
