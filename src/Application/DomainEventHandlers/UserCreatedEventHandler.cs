using BookStore.Domain.Entities;
using BookStore.Domain.Events;
using BookStore.Domain.Interface;
using MediatR;

namespace BookStore.Application.DomainEventHandlers;

public class UserCreatedEventHandler: INotificationHandler<UserCreatedEvent>
{
    private IUnitOfWork _unitOfWork;

    public UserCreatedEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UserCreatedEvent userCreatedEvent, CancellationToken cancellationToken)
    {
        var customerRepo = _unitOfWork.Repository<Customer>();
        var getUser =await customerRepo.GetAsync(a => a.Id == userCreatedEvent.User.Id);
        if (getUser == null)
        {
            var customer = new Customer()
            {
                Id = userCreatedEvent.User.Id,
                Email = userCreatedEvent.User.Email,
                Address = "",
                PhoneNumber = userCreatedEvent.User.PhoneNumber,
                FirstName = userCreatedEvent.User.FirstName,
                LastName = userCreatedEvent.User.LastName,
            };
           await customerRepo.CreateAsync(customer);
           await _unitOfWork.SaveChanges(cancellationToken);
        }
    }
}