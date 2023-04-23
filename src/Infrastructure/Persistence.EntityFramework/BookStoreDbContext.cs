using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class BookStoreDbContext : DbContext
    , IBookStoreContext
{

    
    public DbSet<Book> Books { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Reservation> Reservations { get; set; } 
    public DbSet<BorrowedBook> BorrowedBooks { get; set; } 
    public DbSet<CustomerBookAvaliablityRequest> CustomerBookAvaliablityRequests { get; set; } 
    public DbSet<Notification> Notifications { get; set; } 
    
    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookStoreDbContext).Assembly);
      
        base.OnModelCreating(modelBuilder);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

   

}