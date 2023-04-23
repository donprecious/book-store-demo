using BookStore.Domain.Entities;
using BookStore.Domain.Events;
using BookStore.Domain.Interface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookStore.Application.DomainEventHandlers;

public class BookBorrowedEventHandler : INotificationHandler<BookBorrowedEvent>
{
    private ILogger<BookBorrowedEventHandler> _logger;
    private IUnitOfWork _unitOfWork;
    public BookBorrowedEventHandler(ILogger<BookBorrowedEventHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(BookBorrowedEvent notification, CancellationToken cancellationToken)
    {
       // can send notification or email here 
       _logger.LogInformation("Book borrowed");
       var findReservation = await _unitOfWork.Repository<Reservation>()
           .GetAsync(a =>
               a.CustomerId == notification.BorrowedBook.CustomerId &&
               a.BookId == notification.BorrowedBook.CustomerId && !a.IsCanceled);
       if (findReservation != null)
       {
           findReservation.CancelReservation();
           await _unitOfWork.SaveChanges(cancellationToken);
       }
      

    }
}