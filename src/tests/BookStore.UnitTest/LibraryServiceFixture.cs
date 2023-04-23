using AutoMapper;
using BookStore.Application.Book.Model;
using BookStore.Application.Common.Interfaces;
using BookStore.Application.Library;
using BookStore.Domain.Common;
using BookStore.Domain.Events;
using BookStore.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence;
using Persistence.Repositories;
using Persistence.Seed;

namespace BookStore.UnitTest;

public class LibraryServiceFixture : IDisposable
{
    public LibraryService LibraryService { get; }
    public IUnitOfWork _unitOfWork;
    public LibraryServiceFixture()
    {
        var options = new DbContextOptionsBuilder<BookStoreDbContext>()
            .UseInMemoryDatabase(databaseName: "book_store_test_db")
            .Options;
        
        var dbContext = new BookStoreDbContext(options); 
         Seed.Init(dbContext).Wait();
        var domainEventServiceMoq =new  Mock<IDomainEventService>();
        domainEventServiceMoq.Setup(a => a.DispatchAsync(It.IsAny<IDomainEvent>()));
        domainEventServiceMoq.Setup(a => a.DispatchAsync(It.IsAny<IDomainEvent>()));
        var unitOfWork = new UnitOfWork(dbContext); 
        
        var configurationProvider = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BookDtoMappingProfile>();
        });
        var _mapper = configurationProvider.CreateMapper();
        LibraryService = new LibraryService(domainEventServiceMoq.Object, unitOfWork, _mapper );

        _unitOfWork = unitOfWork;

    }
    public void Dispose()
    {
    }
}