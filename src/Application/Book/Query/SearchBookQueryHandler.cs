using AutoMapper;
using BookStore.Application.Book.Model;
using BookStore.Application.Common.Models;
using BookStore.Application.Library;
using MediatR;

namespace BookStore.Application.Book.SearchBook;

public class SearchBookQueryHandler : IRequestHandler<SearchBookQuery, Result<PaginatedList<GetBookDto>>>
{
    private ILibraryService _libraryService;
    private IMapper _mapper;
    public SearchBookQueryHandler(ILibraryService libraryService, IMapper mapper)
    {
        _libraryService = libraryService;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedList<GetBookDto>>> Handle(SearchBookQuery request, CancellationToken cancellationToken)
    {
        var result = await _libraryService.FindBooks(request.Title, request.ISBN, request.Author, request.Page, request.PageSize);
       
        return Result<PaginatedList<GetBookDto>>.Success(result);
    }
}