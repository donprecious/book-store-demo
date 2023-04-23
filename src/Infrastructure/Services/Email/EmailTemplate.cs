using System.Reflection;
using BookStore.Application.Email;

namespace Services.Email
{
 
    
    public class  EmailTemplates  : IEmailTemplate {
        
        
        public enum EmailTemplateNameEnum
        {
            None,
            PasswordResetTemplate,
            RegistrationVerification
        }

        private static IDictionary<EmailTemplateNameEnum, string> _emailTemplateDic = new Dictionary<EmailTemplateNameEnum, string>()
        {
            { EmailTemplateNameEnum.None, "email.html"},
            { EmailTemplateNameEnum.PasswordResetTemplate, "passwordReset.html" },
            { EmailTemplateNameEnum.RegistrationVerification , "registrationVerification.html"}
        };

        private  string  GetEmailContent(EmailTemplateNameEnum emailTemplateNameEnum)
        {
                string result = "";
                string template = _emailTemplateDic[emailTemplateNameEnum];
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = $"Core.Mail.EmailTemplates.{template}";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                     result = reader.ReadToEnd();
                }
                
                return result; 
                
        }

        public  string GetDefault(string htmlContent)
        {
            var content = GetEmailContent(EmailTemplateNameEnum.None);
            content = content.Replace("{{content}}", htmlContent);
         
            return content;
        }
        public  string GetPasswordResetEmail(string firstname, string resetUrl)
        {
            var content = GetEmailContent(EmailTemplateNameEnum.PasswordResetTemplate);
          content =  content.Replace("{{firstName}}", firstname);
          content = content.Replace("{{url}}", resetUrl);
          return content;
        }

        public  string GetRegistrationVerifcationEmail(string firstname, string resetUrl)
        {
            var content = GetEmailContent(EmailTemplateNameEnum.RegistrationVerification);
            content = content.Replace("{{firstName}}", firstname);
            content = content.Replace("{{url}}", resetUrl);
            return content;
        }
    }
}