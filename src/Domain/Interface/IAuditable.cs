namespace BookStore.Domain.Interface
{
    public interface IAuditable
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? Active { get; set; } 
    }
}