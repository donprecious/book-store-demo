using BookStore.Domain.Entities;

namespace BookStore.Domain.Events;

public class BookReservationExpiredEvent: IDomainEvent
{
    public Reservation Reservation { get; }

    public BookReservationExpiredEvent(Reservation reservation)
    {
        Reservation = reservation;
    }
}