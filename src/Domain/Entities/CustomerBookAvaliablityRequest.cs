using BookStore.Domain.Common;

namespace BookStore.Domain.Entities;

public class CustomerBookAvaliablityRequest: BaseEntity
{
    public string BookId { get; set; } 
    public string CustomerId { get; set; } 
    public bool IsNotified { get; set; }

    private CustomerBookAvaliablityRequest()
    {
        
    }
    public CustomerBookAvaliablityRequest(string bookId, string customerId)
    {
        BookId = bookId;
        CustomerId = customerId;
    }
}