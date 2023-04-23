using BookStore.Application.Common.Models;
using BookStore.Application.Library;
using MediatR;

namespace BookStore.Application.Book.Command;

public class RequestBookWhenAvaliableCommand: IRequest<Result>
{
    public string BookId { get; set; }
    public string CustomerId { get; set; }
}

public class RequestBookWhenAvaliableCommandHandler : IRequestHandler<RequestBookWhenAvaliableCommand,Result>
{
    
    private ILibraryService _libraryService;
    public RequestBookWhenAvaliableCommandHandler(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    public async Task<Result> Handle(RequestBookWhenAvaliableCommand request, CancellationToken cancellationToken)
    {
        await _libraryService.RequestWhenBookIsAvaliable(request.BookId, request.CustomerId, cancellationToken); 
        return  Result.Success();
    }
}
