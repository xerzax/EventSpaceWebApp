namespace EventSpace.Shared.Entities
{
    public class BaseEntity
    {
         
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime LastUpdatedAt { get; set; }

        public DateTime DeletedAt { get; set; }

        public bool IsDeleted { get; set; } = false;


    
}
}
