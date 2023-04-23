using BookStore.Domain.Common;
using BookStore.Domain.Enums;

namespace BookStore.Domain.Entities.ModelToDelete;

public class Transaction : BaseEntity
{
    public BankAccount BankAccount { get; set; }
    public string? BankAccountId { get; set; } 
    public TransactionCategory TransactionCategory { get; set; }

    public decimal Amount { get; set; }
    public string? CurrencyCode { get; set; } = ""; 
    public string UserId { get; set; }
    public string? FailedReason { get; set; } = "";
    public TransactionStatus TransactionStatus { get; set; }
    
    public ICollection<GiftCardExchange> GiftCardExchanges { get; set; }
}