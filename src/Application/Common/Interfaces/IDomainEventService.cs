using BookStore.Domain.Common;
using BookStore.Domain.Events;

namespace BookStore.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
       
        Task DispatchAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent;

    }
}