using BookStore.Application.Common.Models;
using BookStore.Application.Library;
using MediatR;

namespace BookStore.Application.Book.Command;

public class ReserveBookCommand: IRequest<Result>
{
    public string BookId { get; set; }
    public string CustomerId { get; set; }
}

public class ReserveBookCommandHandler : IRequestHandler<ReserveBookCommand,Result>
{
    private ILibraryService _libraryService;

    public ReserveBookCommandHandler(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    public async Task<Result> Handle(ReserveBookCommand request, CancellationToken cancellationToken)
    {
        var result = await _libraryService.ReserveBookAsync(request.BookId, request.CustomerId, cancellationToken); 
        return  Result.Success();
    }
}
