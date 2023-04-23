using BookStore.Domain.Entities;
using BookStore.Domain.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookStore.Domain.Events;


public class BookReservationExpiredEventHandler : INotificationHandler<BookReservationExpiredEvent>
{
    private ILogger<BookReservationExpiredEventHandler> _logger;
    private IUnitOfWork _unitOfWork;
    private IRepository<CustomerBookAvaliablityRequest> _customerBookAvaliablityRequestRepository;
    private IRepository<Notification> _notificationRepository;
    public BookReservationExpiredEventHandler(ILogger<BookReservationExpiredEventHandler> logger,  IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _notificationRepository = _unitOfWork.Repository<Notification>();
        _customerBookAvaliablityRequestRepository = _unitOfWork.Repository<CustomerBookAvaliablityRequest>();
    }

    public async Task Handle(BookReservationExpiredEvent bookReservationExpiredEvent, CancellationToken cancellationToken)
    {
        // can send notification or email here 
        _logger.LogInformation("Book Reservation expired");  
       
        // can send notification or email here 
        
        if (bookReservationExpiredEvent.Reservation.Book.IsAvailable)
        {
            var customerWhoRequestToBeNotified = await   _customerBookAvaliablityRequestRepository.GetAllAsync(a =>
                a.BookId == bookReservationExpiredEvent.Reservation.BookId && !a.IsNotified).ToListAsync();
            var notifications = new List<Notification>();
            foreach (var customerInfo in customerWhoRequestToBeNotified)
            {
                var notification = new Notification(customerInfo.CustomerId,
                    $"Hello, The book {bookReservationExpiredEvent.Reservation.Book.Title} is now avaliable");
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