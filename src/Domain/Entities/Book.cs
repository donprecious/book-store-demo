using BookStore.Domain.Common;

namespace BookStore.Domain.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
    
        public string Publisher { get; set; }
        public int Unit { get; set; }


        public bool IsReserved => Reservations.Any() ? Unit > Reservations.Count(r => !r.IsCanceled) : false;
        public bool IsBorrowed => BorrowedBooks.Any() ? BorrowedBooks.All(a => a.IsReturned == false): false;
        public bool IsAvailable => !IsReserved && !IsBorrowed;

        
        
        public DateTime? ReturnDate => BorrowedBooks
            .Where(a => a.IsReturned == false).MaxBy(a=>a.ReturnedDate)?.ReturnedDate;

        public  ICollection<Reservation> Reservations { get;  set; } = new List<Reservation>();
        public  ICollection<BorrowedBook> BorrowedBooks { get;  set; } = new List<BorrowedBook>();


        public Reservation Reserve(string customerId)
        {
            var reservation = new Reservation(customerId, Id);  
            Reservations.Add(reservation);
            Unit--;
            return reservation;
        }
        
        public BorrowedBook Borrow(string customerId)
        {
            var borrowedBook = new BorrowedBook(customerId, Id);  
            BorrowedBooks.Add(borrowedBook);
            Unit--;
            return borrowedBook;
        }

        public Book()
        {
            
        }

      
    }
}