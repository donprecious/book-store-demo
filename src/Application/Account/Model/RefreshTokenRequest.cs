namespace BookStore.Application.Account.Model
{
    public class RefreshTokenRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
