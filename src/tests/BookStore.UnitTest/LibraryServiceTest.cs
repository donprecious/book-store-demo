using BookStore.Application.Library;
using BookStore.Domain.Entities;
using BookStore.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookStore.UnitTest;

public class LibraryServiceTest: IClassFixture<LibraryServiceFixture>
{
    private LibraryService _libraryService;
    private IUnitOfWork _unitOfWork;

    public LibraryServiceTest(LibraryServiceFixture libraryServiceFixture)
    {
        _libraryService = libraryServiceFixture.LibraryService;
        _unitOfWork = libraryServiceFixture._unitOfWork;
    }

    [Fact]
    public async Task CanGetBooks()
    {
        var books = await _libraryService.FindBooks();  
        Assert.True(books.Items.Any());
    }

    [Fact]
    public async Task CanMakeReservation()
    {
        var customer = await _unitOfWork.Repository<Customer>().GetAllAsync().FirstOrDefaultAsync();
      var    book =( await _unitOfWork.Repository<Book>().GetAllAsync()
            .Include(a => a.Reservations)
            .Include(a => a.BorrowedBooks)
            .ToListAsync()).FirstOrDefault(a => a.IsAvailable);

     var reservation = await _libraryService.ReserveBookAsync(book.Id, customer.Id, CancellationToken.None); 
     Assert.NotNull(reservation);
     Assert.True(!reservation.IsCanceled);
    }
    
    
    [Fact]
    public async Task CanCancelReservationWithReserationId()
    {
      
        var reservation = await _unitOfWork.Repository<Reservation>().GetAsync(a => a.IsCanceled == false);
        await   _libraryService.CancelReservationAsync(reservation.Id, CancellationToken.None); 
        var reservationAfterCancelation = await _unitOfWork.Repository<Reservation>().GetAsync(a => a.Id == reservation.Id);
        Assert.True(reservationAfterCancelation.IsCanceled);
       
    }
    
    [Fact]
    public async Task CanCancelReservation()
    {
       // make a reservation  
       var customer = await _unitOfWork.Repository<Customer>().GetAllAsync().FirstOrDefaultAsync();
       var    book =( await _unitOfWork.Repository<Book>().GetAllAsync()
           .Include(a => a.Reservations)
           .Include(a => a.BorrowedBooks)
           .ToListAsync()).FirstOrDefault(a => a.IsAvailable); 
       
       var makeReservation = await _libraryService.ReserveBookAsync(book.Id, customer.Id, CancellationToken.None);




          await _libraryService.CancelReservationAsync(book.Id, customer.Id, CancellationToken.None);
         var reservationAfterCancelation = await _unitOfWork.Repository<Reservation>().GetAsync(a => a.Id == makeReservation.Id);
        Assert.True(reservationAfterCancelation.IsCanceled);
    }
    
    [Fact]
    public async Task CanBorrowBook()
    {
        var customer = await _unitOfWork.Repository<Customer>().GetAllAsync().FirstOrDefaultAsync();
        var    book =( await _unitOfWork.Repository<Book>().GetAllAsync()
            .Include(a => a.Reservations)
            .Include(a => a.BorrowedBooks)
            .ToListAsync()).FirstOrDefault(a => a.IsAvailable);

        var borrowBook = await _libraryService.BorrowBookAsync(book.Id, customer.Id, CancellationToken.None); 
        
        Assert.NotNull(borrowBook);
    }

    [Fact]
    public async Task CanReturnBook()
    {
        var findBookToReturn = _unitOfWork.Repository<BorrowedBook>().GetAllAsync()
            .FirstOrDefault(a => a.IsReturned == false);
        if (findBookToReturn == null)
        {
            var customer = await _unitOfWork.Repository<Customer>().GetAllAsync().FirstOrDefaultAsync();
            var    book =( await _unitOfWork.Repository<Book>().GetAllAsync()
                .Include(a => a.Reservations)
                .Include(a => a.BorrowedBooks)
                .ToListAsync()).FirstOrDefault(a => a.IsAvailable);

            findBookToReturn = await _libraryService.BorrowBookAsync(book.Id, customer.Id, CancellationToken.None); 
        }

        await _libraryService.ReturnBookAsync(findBookToReturn.Bookid, findBookToReturn.CustomerId, CancellationToken.None); 
        
        var bookAfterBorrowed = _unitOfWork.Repository<BorrowedBook>().GetAllAsync()
            .FirstOrDefault(a => a.Id== findBookToReturn.Id);
        
        
        Assert.NotNull(bookAfterBorrowed);
        Assert.True(bookAfterBorrowed.IsReturned);
    }

    [Fact]
    public async Task CanRequestToGetNotified()
    {
        var customers = await _unitOfWork.Repository<Customer>().GetAllAsync().Take(2).ToListAsync();
        var customerA = customers[0];
        var customerB = customers[1];
        var    book =( await _unitOfWork.Repository<Book>().GetAllAsync()
            .Include(a => a.Reservations)
            .Include(a => a.BorrowedBooks)
            .ToListAsync()).FirstOrDefault(a => a.IsAvailable);
        book.Unit = 1;  
        
        var borrowBook = await _libraryService.BorrowBookAsync(book.Id, customerA.Id, CancellationToken.None);

       await  _libraryService.RequestWhenBookIsAvaliable(book.Id, customerB.Id, CancellationToken.None);

       var afterRequestRecord = await _unitOfWork.Repository<CustomerBookAvaliablityRequest>()
           .GetAsync(a => a.CustomerId == customerB.Id && a.BookId == book.Id);
       Assert.NotNull(afterRequestRecord);
       Assert.True(!afterRequestRecord.IsNotified);
    }
}