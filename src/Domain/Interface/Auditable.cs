namespace BookStore.Domain.Interface
{
    public class Auditable : IAuditable
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime?UpdatedAt { get; set; }
        public bool? Active { get; set; }

        public Auditable()
        {
            CreatedAt = DateTime.Now;
        }
    }
}