using System.ComponentModel.DataAnnotations;

namespace Blossom_BusinessObjects.Entities
{
    public class FlowerCategory : BaseEntity
    {
       
        [Required]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        //public virtual ICollection<Flower> Flowers { get; set; } = new List<Flower>();
    }
}
