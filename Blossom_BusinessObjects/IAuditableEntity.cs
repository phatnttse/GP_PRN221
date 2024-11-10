namespace Blossom_BusinessObjects.Entities
{
    public interface IAuditableEntity
    {
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
