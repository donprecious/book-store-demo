using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BookStore.Application.Book.Command;
using BookStore.Application.Book.Model;
using BookStore.Application.Common.Models;
using BookStore.Domain.Entities;
using BookStore.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using JsonConverter = System.Text.Json.Serialization.JsonConverter;

namespace BookStore.IntegrationTest;

public class LibraryApiTest : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly TestingWebAppFactory<Program> _factory;
    public LibraryApiTest(TestingWebAppFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    [Fact]
    public async Task CanAnyGetBooks()
    { 
        var response = await _client.GetAsync("/api/v1/library/books");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        
        var books = JsonSerializer.Deserialize<Result<PaginatedList<GetBookDto>>>(content, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        Assert.NotNull(books);
        Assert.True(books.Succeeded);
        Assert.True(books.Data.Items.Any());
    }

    [Fact]
    public async Task CanReserveBook()
    {

        Customer customer;
        Book book;
        using (var scope = _factory.Services.CreateScope())
        {
            var customerRepository = scope.ServiceProvider.GetService<IRepository<Customer>>();
            customer = await customerRepository.GetAllAsync().FirstOrDefaultAsync(); 
            var bookRepository = scope.ServiceProvider.GetService<IRepository<Book>>();
            book =(await bookRepository.GetAllAsync()
                .Include(a => a.Reservations)
                .Include(a => a.BorrowedBooks)
                .ToListAsync()).FirstOrDefault(a => a.IsAvailable);
        }

        // Act
        var requestBody = new StringContent(JsonSerializer.Serialize(new
        {
            customerId = customer.Id,
            bookId = book.Id
        }), Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("/api/v1/library/make-reservation", requestBody);
        response.EnsureSuccessStatusCode(); 
        var content = await response.Content.ReadAsStringAsync();
        var result =JsonSerializer.Deserialize<Result>(content, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        Assert.NotNull(result);
        Assert.True(result.Succeeded);
    }
    
    [Fact]
    public async Task CanCancelReservation()
    {

        Reservation reservation;
        IRepository<Reservation> reservatationRepository;
        Book book;
        using (var scope = _factory.Services.CreateScope())
        {
             reservatationRepository = scope.ServiceProvider.GetService<IRepository<Reservation>>();
            reservation = await reservatationRepository.GetAllAsync(a=>!a.IsCanceled).FirstOrDefaultAsync(); 
           
        }

        
        // Act
        var requestBody = new StringContent(JsonSerializer.Serialize(new 
        {
            customerId =reservation.CustomerId,
            bookId = reservation.BookId
        }), Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("/api/v1/library/cancel-reservation", requestBody);
        response.EnsureSuccessStatusCode(); 
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Result>(content, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        var findCancelReservation = await reservatationRepository.GetAsync(a => a.Id == reservation.Id);  
        
        Assert.NotNull(findCancelReservation); 
        Assert.True(findCancelReservation.IsCanceled);
        Assert.NotNull(result);
        Assert.True(result.Succeeded);
    }
    
    [Fact]
    public async Task CanBorrowBook()
    {

        Customer customer;
        Book book;
        using (var scope = _factory.Services.CreateScope())
        {
            var customerRepository = scope.ServiceProvider.GetService<IRepository<Customer>>();
            customer = await customerRepository.GetAllAsync().FirstOrDefaultAsync(); 
            var bookRepository = scope.ServiceProvider.GetService<IRepository<Book>>();
            book = (await bookRepository.GetAllAsync()
                .Include(a => a.Reservations)
                .Include(a => a.BorrowedBooks)
                .ToListAsync()).FirstOrDefault(a => a.IsAvailable);
        }

        // Act
        var requestBody = new StringContent(JsonSerializer.Serialize(new 
        {
            customerId = customer.Id,
            bookId = book.Id
        }), Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("/api/v1/library/borrow", requestBody);
        response.EnsureSuccessStatusCode(); 
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Result>(content, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        Assert.NotNull(result);
        Assert.True(result.Succeeded);
    }
    
    [Fact]
    public async Task CanReturnBook()
    {
        
        BorrowedBook  borrowedBook;
        IRepository<BorrowedBook> borrowedBookRepository;
        Book book;
        using (var scope = _factory.Services.CreateScope())
        {
            borrowedBookRepository = scope.ServiceProvider.GetService<IRepository<BorrowedBook>>();
            borrowedBook = await borrowedBookRepository.GetAllAsync(a=>!a.IsReturned).FirstOrDefaultAsync(); 
           
        }

        
        // Act
        var requestBody = new StringContent(JsonSerializer.Serialize(new 
        {
            customerId =borrowedBook.CustomerId,
            bookId = borrowedBook.Bookid
        }), Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("/api/v1/library/return", requestBody);
        response.EnsureSuccessStatusCode(); 
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Result>(content, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        var findBorrowedBook = await borrowedBookRepository.GetAsync(a => a.Id == borrowedBook.Id);  
        
        Assert.NotNull(findBorrowedBook); 
        Assert.True(findBorrowedBook.IsReturned);
        Assert.NotNull(result);
        Assert.True(result.Succeeded);
    }
    //
    
    [Fact]
    public async Task CanRequestForBookAvaliablity()
    {
        
        BorrowedBook  borrowedBook;
        IRepository<BorrowedBook> borrowedBookRepository;
        IRepository<Book> bookRepository;
        IRepository<CustomerBookAvaliablityRequest> customerBookAvaliablityRepository;
        
        
        Book bookToBorrow;
        Customer customerA;
        Customer customerB; 
        using (var scope = _factory.Services.CreateScope())
        {
            borrowedBookRepository = scope.ServiceProvider.GetService<IRepository<BorrowedBook>>();
            bookRepository = scope.ServiceProvider.GetService<IRepository<Book>>();
            customerBookAvaliablityRepository = scope.ServiceProvider.GetService<IRepository<CustomerBookAvaliablityRequest>>();
            var customerRepo = scope.ServiceProvider.GetService<IRepository<Customer>>();
            borrowedBook = await borrowedBookRepository.GetAllAsync(a=>!a.IsReturned).FirstOrDefaultAsync();
            bookToBorrow = await bookRepository.GetAsync(a => a.IsAvailable && a.Unit == 1);
            var customers = await customerRepo.GetAllAsync().Take(2).ToListAsync();
            customerA = customers[0];
            customerB = customers[1];
        }

        
        // Act
        // borrow the last book first by customer 
        var requestBodyToBorrowBook = new StringContent(JsonSerializer.Serialize( new
        {
            customerId = customerA.Id,
            bookId = bookToBorrow.Id
        }), Encoding.UTF8, "application/json");
        
        var responseFromBorrowedBook = await _client.PostAsync("/api/v1/library/borrow", requestBodyToBorrowBook);
        responseFromBorrowedBook.EnsureSuccessStatusCode();
        var borrowBookContent = await responseFromBorrowedBook.Content.ReadAsStringAsync();
        var borrowBookResult  = JsonSerializer.Deserialize<Result>(borrowBookContent, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        if (!borrowBookResult.Succeeded) throw new Exception("unable to borrow a book : "+ borrowBookResult.Message);
        
        
        var requestBody = new StringContent(JsonSerializer.Serialize(new 
        {
            customerId =customerB.Id,
            bookId = borrowedBook.Bookid
        }), Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("/api/v1/library/request-book", requestBody);
        response.EnsureSuccessStatusCode(); 
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Result>(content, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        var findRequestToBorrowBook = await customerBookAvaliablityRepository.GetAsync(a => a.BookId == borrowedBook.Id
        && a.CustomerId == customerB.Id && !a.IsNotified 
        );
       
        Assert.NotNull(findRequestToBorrowBook);
        Assert.NotNull(result);
        Assert.True(result.Succeeded);
    }
}