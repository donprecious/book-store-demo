using BookStore.Domain.Common;

namespace BookStore.Domain.Entities;

public class Notification: BaseEntity
{
    public string Message { get; set; }
    public  string CustomerId { get; set; }

    private Notification()
    {
        
    }
    public Notification(string  customerId, string message)
    {
        CustomerId = customerId;
        Message = message;
    }
}