using BookStore.Domain.Entities;

namespace BookStore.Domain.Events;

public class BookReturnedEvent: IDomainEvent
{
    public BorrowedBook BorrowedBook { get; }

    public BookReturnedEvent(BorrowedBook bookBorrowedEvent)
    {
        BorrowedBook = bookBorrowedEvent;
    }
}