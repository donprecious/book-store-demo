namespace Services.Email.Model
{
    public class SendGridSetting
    {
        public string ApiKey { get; set; }
        public string FromEmail { get; set; }
        public string FromEmailUserName { get; set; }
        
    }
}