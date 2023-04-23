using BookStore.Domain.Common;

namespace BookStore.Domain.Entities;

public class Customer: BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
}