using BookStore.Domain.Common;

namespace BookStore.Domain.Entities;

public class Reservation : BaseEntity
{
    public string BookId { get; private set; }
    public  Book Book { get; private set; }
    public string CustomerId { get; private set; }
    public  Customer Customer { get; private set; }
    public DateTime ReservedDate { get; private set; }
    public bool IsCanceled { get;  private set; }

    private Reservation()
    {
        
    }
    public Reservation(string customerId, string bookId)
    {
        BookId = bookId;
        CustomerId = customerId;
        ReservedDate = DateTime.UtcNow;
        IsCanceled = false;
    }

    public void CancelReservation()
    {
        IsCanceled = true;
        Book.Unit++;
    }
}

