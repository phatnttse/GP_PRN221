using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Blossom_BusinessObjects.Entities.Enums;

namespace Blossom_BusinessObjects.Entities
{
    public class Order : BaseEntity
    {

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual Account User { get; set; }

        public string BuyerName { get; set; }

        public string BuyerPhone { get; set; }

        public string BuyerEmail { get; set; }

        public string BuyerAddress { get; set; }

        public string Note { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalPrice { get; set; }

        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>(); // Navigation property cho OrderDetail

    }
}
