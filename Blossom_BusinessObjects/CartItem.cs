using System.ComponentModel.DataAnnotations.Schema;

namespace Blossom_BusinessObjects.Entities
{
    public class CartItem : BaseEntity
    {

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual Account User { get; set; }

        [ForeignKey("Flower")]
        public string FlowerId { get; set; }
        public virtual Flower Flower { get; set; }
        public int Quantity { get; set; }

    }
}
