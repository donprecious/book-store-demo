using BookStore.Application.Common.Interfaces;
using BookStore.Domain.Events;
using MediatR;

namespace BookStore.Application.InMemoryEvent;

public class InMemoryEventPublisher: IDomainEventService
{
    private readonly IMediator _mediator;

    public InMemoryEventPublisher(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task DispatchAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
    {
        await _mediator.Publish(domainEvent);
    }
}