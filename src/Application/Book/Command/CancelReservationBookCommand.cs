using BookStore.Application.Common.Models;
using BookStore.Application.Library;
using MediatR;

namespace BookStore.Application.Book.Command;

public class CancelReserveCommand: IRequest<Result>
{
    public string BookId { get; set; }
    public string CustomerId { get; set; }
}

public class CancelReserveCommandHandler : IRequestHandler<CancelReserveCommand,Result>
{
    private ILibraryService _libraryService;

    public CancelReserveCommandHandler(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    public async Task<Result> Handle(CancelReserveCommand request, CancellationToken cancellationToken)
    {
        await _libraryService.CancelReservationAsync(request.BookId, request.CustomerId, cancellationToken); 
        return  Result.Success();
    }
}
