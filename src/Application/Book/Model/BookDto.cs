using AutoMapper;

namespace BookStore.Application.Book.Model;

public class GetBookDto
{
    public string Title { get; set; }
    public string ISBN { get; set; }
    public string Author { get; set; }
    
    public string Publisher { get; set; }
    public int Unit { get; set; }
    
    public bool IsReserved { get; set; }
    public bool IsBorrowed { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime? ReturnDate { get; set; }

    public GetBookDto()
    {
        
    }
}

public class BookDtoMappingProfile: Profile
{
    
    public BookDtoMappingProfile()
    {
        CreateMap<Domain.Entities.Book, GetBookDto>();
    }
}