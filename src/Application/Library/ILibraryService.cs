using BookStore.Application.Book.Model;
using BookStore.Application.Common.Models;
using BookStore.Domain.Entities;

namespace BookStore.Application.Library;

public interface ILibraryService
{
    Task<PaginatedList<GetBookDto>> FindBooks(string title = "", string isbn = "", string author = "",
        int page = 1, int pagesize = 10);   
    Task<Reservation> ReserveBookAsync(string bookId, string customerId, CancellationToken cancellationToken);
    Task CancelReservationAsync(string reservationId, CancellationToken cancellationToken);
    Task CancelReservationAsync(string bookId, string customerId, CancellationToken cancellationToken);
    Task<BorrowedBook> BorrowBookAsync(string bookId, string customerId, CancellationToken cancellationToken);
    Task ReturnBookAsync(string borrowingId, CancellationToken cancellationToken);
    Task ReturnBookAsync(string bookId, string customerId, CancellationToken cancellationToken);
    Task RequestWhenBookIsAvaliable(string bookId, string customerId, CancellationToken cancellationToken);
}