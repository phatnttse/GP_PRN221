using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Blossom_BusinessObjects.Entities.Enums;

namespace Blossom_BusinessObjects.Entities
{
    public class OrderDetail : BaseEntity
    {

        [ForeignKey("Order")]
        public string OrderId { get; set; }
        public virtual Order Order { get; set; }

        [ForeignKey("Seller")]
        public string SellerId { get; set; }
        public virtual Account Seller { get; set; }

        [ForeignKey("Flower")]
        public string FlowerId { get; set; }
        public virtual Flower Flower { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        [EnumDataType(typeof(OrderDetailStatus))]
        public OrderDetailStatus Status { get; set; }

        public bool IsDeleted { get; set; }
    }
}
