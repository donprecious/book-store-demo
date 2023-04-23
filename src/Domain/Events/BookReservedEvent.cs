using BookStore.Domain.Entities;

namespace BookStore.Domain.Events;

public class BookReservedEvent: IDomainEvent
{
    public Reservation Reservation { get; }

    public BookReservedEvent(Reservation reservation)
    {
        Reservation = reservation;
    }

}