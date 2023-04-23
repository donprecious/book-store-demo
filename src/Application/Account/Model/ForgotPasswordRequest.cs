using System.ComponentModel.DataAnnotations;

namespace BookStore.Application.Account.Model
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
