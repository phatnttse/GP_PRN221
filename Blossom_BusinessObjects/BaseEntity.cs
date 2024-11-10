using System.ComponentModel.DataAnnotations;

namespace Blossom_BusinessObjects.Entities
{
    public class BaseEntity : IAuditableEntity
    {
        [Key]
        public string Id { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public BaseEntity()
        {
            Id = Guid.NewGuid().ToString(); 
        }
    }
}
