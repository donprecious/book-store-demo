namespace BookStore.Application.Account.Model
{
    public class AuthenticationRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class AuthenticationRequestByPhone
    {
        public string Phone { get; set; }
        public string Password { get; set; }
    }
}