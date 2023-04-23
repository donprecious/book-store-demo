using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStore.Application.Book;
using BookStore.Application.Book.Model;
using BookStore.Application.Common.Exceptions;
using BookStore.Application.Common.Interfaces;
using BookStore.Application.Common.Models;
using BookStore.Domain.Entities;
using BookStore.Domain.Events;
using BookStore.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Library;

public class LibraryService : ILibraryService
{
    private IUnitOfWork _unitOfWork;
    private IRepository<Domain.Entities.Book> _bookRepository;
    private IDomainEventService _domainEventService;
    private IRepository<Reservation> _reservationRepository;
    private IRepository<BorrowedBook> _borrowedBookRepository;
    private IRepository<CustomerBookAvaliablityRequest> _customerBookAvaliablityRequestRepo;
    private IMapper _mapper;
    public LibraryService(IDomainEventService domainEventService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;

        _domainEventService = domainEventService;
        _reservationRepository = _unitOfWork.Repository<Reservation>();
        _bookRepository =  _unitOfWork.Repository<Domain.Entities.Book>();
        _borrowedBookRepository  = _unitOfWork.Repository<BorrowedBook>();
        _customerBookAvaliablityRequestRepo = _unitOfWork.Repository<CustomerBookAvaliablityRequest>();
    }

    public async Task<PaginatedList<GetBookDto>> FindBooks(string title = "", string isbn = "", string author = "", int page =1, int pagesize = 10)
    {
        var query =  _bookRepository.GetAllAsync();
        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(a => a.Title.ToLower().Contains(title.ToLower())); 
        }

        if (!string.IsNullOrEmpty(isbn))
        {
            query = query.Where(a => a.ISBN.ToLower().StartsWith(isbn.ToLower())); 
        }
        if (!string.IsNullOrEmpty(author))
        {
            query = query.Where(a => a.Author.ToLower().StartsWith(author.ToLower())); 
        }

        var recordQuery = query.ProjectTo<GetBookDto>(_mapper.ConfigurationProvider);

        var book = await PaginatedList<GetBookDto>.CreateAsync(recordQuery, page, pagesize);
        return book;
    }
    

    public async Task<Reservation> ReserveBookAsync(string bookId, string customerId, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetAsync(bookId);

        if (book == null)
        {
            throw new NotFoundException($"Book with id {bookId} not found.");
        }

        if (!book.IsAvailable)
        {
            throw new BadRequestException($"Book with id {bookId} is not available for reservation.");
        }

         var reservation = book.Reserve(customerId);
         await _bookRepository.UpdateAsync(book);
         await _unitOfWork.SaveChanges(CancellationToken.None);
         await _domainEventService.DispatchAsync(new BookReservedEvent(reservation));
         return reservation;
    }

    public async Task CancelReservationAsync(string reservationId, CancellationToken cancellationToken)
    {
        var findReservation = await _reservationRepository
            .Get(a=>a.Id == reservationId )
            .Include(a=>a.Book).FirstOrDefaultAsync();
        if (findReservation == null) throw new NotFoundException($"Reservation with {reservationId} not found");

        if (findReservation.IsCanceled) throw new BadRequestException("Reservation already canceled");
        
        findReservation.CancelReservation();
       await _reservationRepository.UpdateAsync(findReservation);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        await _domainEventService.DispatchAsync(new BookReservationCanceledEvent(findReservation));
       
    }

    public async Task CancelReservationAsync(string bookId, string customerId, CancellationToken cancellationToken)
    {
        var findReservation = await _reservationRepository
            .Get(a=>a.BookId ==bookId && a.CustomerId == customerId && a.IsCanceled ==false )
            .Include(a=>a.Book).FirstOrDefaultAsync();
        if (findReservation == null) throw new NotFoundException($"Reservation  not found");

        if (findReservation.IsCanceled) throw new BadRequestException("Reservation already canceled");
        
        findReservation.CancelReservation();
        await _reservationRepository.UpdateAsync(findReservation);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        await _domainEventService.DispatchAsync(new BookReservationCanceledEvent(findReservation));
    }

    public async Task<BorrowedBook> BorrowBookAsync(string bookId, string customerId, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetAsync(bookId);

        if (book == null)
        {
            throw new NotFoundException($"Book with id {bookId} not found.");
        }

        if (!book.IsAvailable)
        {
            throw new BadRequestException($"Book with id {bookId} is not available for reservation.");
        }

        var borrowedBook = book.Borrow(customerId);
        await _bookRepository.UpdateAsync(book);
        await _unitOfWork.SaveChanges(cancellationToken);
        await _domainEventService.DispatchAsync(new BookBorrowedEvent(borrowedBook));
        return borrowedBook;
        
        

    }

    public async Task ReturnBookAsync(string borrowingId, CancellationToken cancellationToken)
    {
        var borrowedBook = await _borrowedBookRepository.GetAsync(borrowingId); 
        
        if (borrowedBook == null) throw new NotFoundException($"Reservation with {borrowingId} not found");

        if (borrowedBook.IsReturned) throw new BadRequestException("Book already Returned");
        borrowedBook.ReturnBook();

        await _borrowedBookRepository.UpdateAsync(borrowedBook);
        await _domainEventService.DispatchAsync(new BookReturnedEvent(borrowedBook));
        await _unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task ReturnBookAsync(string bookId, string customerId, CancellationToken cancellationToken)
    {
        var borrowedBook = await _borrowedBookRepository
            .Get(a=>a.CustomerId == customerId && a.Bookid == bookId 
                                               && a.IsReturned == false)
            .Include(a=>a.Book)
            .FirstOrDefaultAsync(cancellationToken)
            ; 
        
        if (borrowedBook == null) throw new NotFoundException($"Record  not found");

        if (borrowedBook.IsReturned) throw new BadRequestException("Book already Returned");
        borrowedBook.ReturnBook();

        await _borrowedBookRepository.UpdateAsync(borrowedBook);
        await _domainEventService.DispatchAsync(new BookReturnedEvent(borrowedBook));
        await _unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task RequestWhenBookIsAvaliable(string bookId, string customerId, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetAsync(bookId);

        if (book == null)
        {
            throw new NotFoundException($"Book with id {bookId} not found.");
        }

        if (book.IsAvailable)
        {
            throw new BadRequestException($"Book is already avaliable ");
        }

        var hasRequestedBefore = await _customerBookAvaliablityRequestRepo
            .GetAsync(a => a.BookId == bookId && a.CustomerId == customerId);
        if (hasRequestedBefore != null)
        {
            throw new BadRequestException($"Customer has already requested to recieve notification for this book");
        }

         var bookRequest = new CustomerBookAvaliablityRequest(bookId, customerId);
         await _customerBookAvaliablityRequestRepo.CreateAsync(bookRequest);
         await _unitOfWork.SaveChanges(cancellationToken);
    }
}