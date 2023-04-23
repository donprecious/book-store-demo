using BookStore.Application.Book.Model;
using BookStore.Application.Common.Models;
using MediatR;

namespace BookStore.Application.Book.SearchBook;

public class SearchBookQuery :PaginationParameter , IRequest<Result<PaginatedList<GetBookDto>>>
{
    public string Title { get; set; } = "";
    public string ISBN { get; set; } = "";
    public string Author { get; set; } = "";
}