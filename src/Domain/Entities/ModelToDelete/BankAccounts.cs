using BookStore.Domain.Common;

namespace BookStore.Domain.Entities.ModelToDelete;

public class BankAccount : BaseEntity
{
    public string BankName { get; set; }
    public string AccountName { get; set; }
    public string AccountNumber { get; set; }
    public string UserId { get; set; }
}