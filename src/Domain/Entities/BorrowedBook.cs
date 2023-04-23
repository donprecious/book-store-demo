using BookStore.Domain.Common;

namespace BookStore.Domain.Entities;

public class BorrowedBook : BaseEntity
{
    public string CustomerId { get; set; }
    public Customer Customer { get; set; }

    public string Bookid { get; set; }
    public Book Book { get; set; }
    public bool IsReturned { get; set; } 
    public DateTime BorrowedDate { get; set; }
    public DateTime? ReturnedDate { get; set; }

    private BorrowedBook()
    {
        
    }
    public BorrowedBook(string customerId, string bookId)
    {
        CustomerId = customerId;
        Bookid = bookId;
        IsReturned = false;
        BorrowedDate = DateTime.UtcNow;
    }
    
    public void ReturnBook()
    {
        IsReturned = true;
        Book.Unit--;
    }
}