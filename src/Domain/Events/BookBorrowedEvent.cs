using BookStore.Domain.Entities;

namespace BookStore.Domain.Events;

public class BookBorrowedEvent: IDomainEvent
{
    public BorrowedBook BorrowedBook { get; }

    public BookBorrowedEvent(BorrowedBook bookBorrowedEvent)
    {
        BorrowedBook = bookBorrowedEvent;
    }
}