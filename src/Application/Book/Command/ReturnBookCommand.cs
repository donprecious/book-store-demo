using BookStore.Application.Common.Models;
using BookStore.Application.Library;
using MediatR;

namespace BookStore.Application.Book.Command;

public class ReturnBookCommand: IRequest<Result>
{
    public string BookId { get; set; }
    public string CustomerId { get; set; }
}

public class ReturnBookCommandCommandCommandHandler : IRequestHandler<ReturnBookCommand,Result>
{
    private ILibraryService _libraryService;

    public ReturnBookCommandCommandCommandHandler(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    public async Task<Result> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
    {  
         await _libraryService.ReturnBookAsync(request.BookId, request.CustomerId, cancellationToken); 
        return  Result.Success();
    }
}
