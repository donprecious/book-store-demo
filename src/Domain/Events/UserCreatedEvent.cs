using BookStore.Domain.Entities;
using BookStore.Domain.Entities.ModelToDelete.IdentityModels;

namespace BookStore.Domain.Events;

public class UserCreatedEvent: IDomainEvent
{
    public ApplicationUser User { get; }

    public UserCreatedEvent(ApplicationUser user)
    {
        User = user;
    }
}