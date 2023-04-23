using BookStore.Application.Common.Models;
using BookStore.Application.Library;
using MediatR;

namespace BookStore.Application.Book.Command;

public class BorrowBookCommand: IRequest<Result>
{
    public string BookId { get; set; }
    public string CustomerId { get; set; }
}

public class BorrowBookCommandCommandHandler : IRequestHandler<BorrowBookCommand,Result>
{
    private ILibraryService _libraryService;

    public BorrowBookCommandCommandHandler(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    public async Task<Result> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
    {
        var result = await _libraryService.BorrowBookAsync(request.BookId, request.CustomerId, cancellationToken); 
        return  Result.Success();
    }
}
