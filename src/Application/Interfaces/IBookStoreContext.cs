using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Interfaces;

public interface IBookStoreContext
{
        
     DbSet<Domain.Entities.Book> Books { get; set; }
     DbSet<Customer> Customers { get; set; }
     DbSet<Reservation> Reservations { get; set; } 
     DbSet<BorrowedBook> BorrowedBooks { get; set; } 
     public DbSet<CustomerBookAvaliablityRequest> CustomerBookAvaliablityRequests { get; set; } 
     public DbSet<Notification> Notifications { get; set; } 
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}