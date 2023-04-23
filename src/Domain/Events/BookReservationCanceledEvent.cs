using BookStore.Domain.Entities;

namespace BookStore.Domain.Events;

public class BookReservationCanceledEvent: IDomainEvent
{
    public Reservation Reservation { get; }

    public BookReservationCanceledEvent(Reservation reservation)
    {
        Reservation = reservation;
    }
}