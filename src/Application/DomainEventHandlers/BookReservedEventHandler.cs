using BookStore.Application.DomainEventHandlers;
using BookStore.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookStore.Domain.Events;


public class BookReservedEventHandler : INotificationHandler<BookReservedEvent>
{
    private ILogger<BookReservedEventHandler> _logger;

    public BookReservedEventHandler(ILogger<BookReservedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(BookReservedEvent bookReservedEvent, CancellationToken cancellationToken)
    {
        // can send notification or email here 
        _logger.LogInformation("Book Returned");  
        // todo check if book is avaliable and send notification or email to user 
    }
}