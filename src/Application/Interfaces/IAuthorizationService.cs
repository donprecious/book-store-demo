using BookStore.Application.Account.Model;

namespace BookStore.Application.Interfaces
{
    public interface IAuthorizationService
    {
         UserDto GetAuthenticatedUser();
    }
}