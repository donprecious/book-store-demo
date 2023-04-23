using Bogus;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Seed;

public static class Seed
{
    public async static Task Init(IBookStoreContext _bookStoreContext)
    {
        await SeedBooks(_bookStoreContext);
        await SeedCustomer(_bookStoreContext);
    }
    public static async Task SeedBooks(IBookStoreContext _bookStoreContext)
    {
        var hasBooks = await _bookStoreContext.Books.AnyAsync();
        
        if (!hasBooks)
        {
            var fakeBooks = new Faker<Book>()
                .RuleFor(a => a.Author, b => b.Name.FullName())
                .RuleFor(a => a.Publisher, b => b.Company.CompanyName())
                .RuleFor(a => a.Title, b => b.Commerce.ProductName())
                .RuleFor(a => a.ISBN, b => b.Commerce.Ean8())
                .RuleFor(a => a.Unit, b => b.Random.Int(1, 5))
                .Generate(20)
                ;
            
          await  _bookStoreContext.Books.AddRangeAsync(fakeBooks);
          await _bookStoreContext.SaveChangesAsync(CancellationToken.None);
        }
    }

    public static async Task SeedCustomer(IBookStoreContext _bookStoreContext)
    {
        var hasCustomers = await _bookStoreContext.Customers.AnyAsync();
        if (!hasCustomers)
        {
            var fakeCustomers = new Faker<Customer>()
                .RuleFor(a => a.PhoneNumber, c => c.Person.Phone)
                .RuleFor(a => a.Email, c => c.Person.Email)
                .RuleFor(a => a.LastName, c => c.Person.LastName)
                .RuleFor(a => a.FirstName, c => c.Person.FirstName)
                .RuleFor(a => a.Address, c => c.Person.Address.Street)
                .Generate(5);
            
            
            await _bookStoreContext.Customers.AddRangeAsync(fakeCustomers);
            await _bookStoreContext.SaveChangesAsync(CancellationToken.None);
        }
    }
}