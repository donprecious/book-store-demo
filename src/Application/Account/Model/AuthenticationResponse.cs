namespace BookStore.Application.Account.Model
{
    public class AuthenticationResponse
    {
        // public string Id { get; set; }
        // public string UserName { get; set; }
        // public string Email { get; set; }
        // public List<string> Roles { get; set; }
        public bool IsVerified { get; set; }
       
        
        public string Token { get; set; }
        public string RefreshToken { get; set; } 
        
        public DateTime TokenExpireTime { get; set; }

        public UserDto User { get; set; } = new UserDto();
    }
}
