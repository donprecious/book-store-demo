using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AutoMapper;
using BookStore.Application.Book.Model;
using BookStore.Application.Common.Interfaces;
using BookStore.Application.Common.Models;
using BookStore.Application.Library;
using BookStore.Domain.Entities;
using BookStore.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
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
   
 
}