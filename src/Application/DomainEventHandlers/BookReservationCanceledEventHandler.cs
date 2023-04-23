using BookStore.Domain.Entities;
using BookStore.Domain.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookStore.Domain.Events;

public class BookReservationCanceledEventHandler : INotificationHandler<BookReservationCanceledEvent>
{
    private ILogger<BookReservationCanceledEventHandler> _logger;
    private IUnitOfWork _unitOfWork;
    private IRepository<CustomerBookAvaliablityRequest> _customerBookAvaliablityRequestRepository;
    private IRepository<Notification> _notificationRepository;
    public BookReservationCanceledEventHandler(ILogger<BookReservationCanceledEventHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _notificationRepository = _unitOfWork.Repository<Notification>();
        _customerBookAvaliablityRequestRepository = _unitOfWork.Repository<CustomerBookAvaliablityRequest>();
    }

    public async Task Handle(BookReservationCanceledEvent bookReservationCanceledEvent,
        CancellationToken cancellationToken)
    {
        // can send notification or email here 
        _logger.LogInformation("Book Returned");
  
        
        if (bookReservationCanceledEvent.Reservation.Book.IsAvailable)
        {
            var customerWhoRequestToBeNotified = await _customerBookAvaliablityRequestRepository.GetAllAsync(a =>
                a.BookId == bookReservationCanceledEvent.Reservation.BookId && !a.IsNotified).ToListAsync(cancellationToken);
            var notifications = new List<Notification>();
            foreach (var customerInfo in customerWhoRequestToBeNotified)
            {
                var notification = new Notification(customerInfo.CustomerId,
                    $"Hello, The book {bookReservationCanceledEvent.Reservation.Book.Title} is now avaliable");
                notifications.Add(notification);
                customerInfo.IsNotified = true;
                await _customerBookAvaliablityRequestRepository.UpdateAsync(customerInfo);
            }
        
            await _notificationRepository.CreateManyAsync(notifications); 
       
            await _unitOfWork.SaveChanges(cancellationToken);
        }
        _logger.LogInformation("Book Returned");  
    }
}