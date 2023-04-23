using BookStore.Domain.Common;

namespace BookStore.Domain.Entities.ModelToDelete;

public class GiftCards: BaseEntity
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }

    public ICollection<GiftCardRates> Rates { get; set; }
}

public class GiftCardRates: BaseEntity{

    public GiftCards GiftCard { get; set; }
    public string GiftCardId { get; set; }
    public string Description { get; set; } 
    public string GiftCardType { get; set; }
    public string CountryCode { get; set; }
    public string BaseCurrencyCode { get; set; }
    public string QuoteCurrencyCode { get; set; }
    public decimal MinVal { get; set; }
    public decimal MaxVal { get; set; }
    public decimal Rate { get; set; }
}