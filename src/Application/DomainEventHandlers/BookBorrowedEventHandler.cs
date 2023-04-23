using BookStore.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookStore.Application.DomainEventHandlers;

public class BookBorrowedEventHandler : INotificationHandler<BookBorrowedEvent>
{
    private ILogger<BookBorrowedEventHandler> _logger;

    public BookBorrowedEventHandler(ILogger<BookBorrowedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(BookBorrowedEvent notification, CancellationToken cancellationToken)
    {
       // can send notification or email here 
       _logger.LogInformation("Book borrowed"); 
    }
}