using BookStore.Domain.Common;

namespace BookStore.Domain.Entities.ModelToDelete;

public class GiftCardExchange : BaseEntity
{
    public string TransactionId { get; set; }
    public Transaction Transaction { get; set; }
    public string? BaseCurrencyCode { get; set; } = "";
    public string GiftCardType { get; set; }
    public string? QuoteCurrencyCode { get; set; } = "";
    public string? CountryCode { get; set; } = "";
    public decimal? AmountInCard { get; set; }
    public decimal DeclaredAmount { get; set; }
    public decimal AmountRecieved { get; set; }
    public string GiftCardUrl { get; set; } = "";
    
    public string? GiftCardId { get; set; }
    public GiftCards? GiftCard { get; set; }
    
    public GiftCardRates? GiftCardRate { get; set; }
    
}