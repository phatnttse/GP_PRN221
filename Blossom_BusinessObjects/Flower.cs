    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using Blossom_BusinessObjects.Entities.Enums;

    namespace Blossom_BusinessObjects.Entities
    {
        public class Flower : BaseEntity
        {

            [Required]
            [ForeignKey("Seller")]
            public string SellerId { get; set; }

            public virtual Account Seller { get; set; }

            public string Name { get; set; }

            [Required]
            [MaxLength(1000)]
            public string Description { get; set; }

            [Column(TypeName = "decimal(10, 2)")]
            public decimal Price { get; set; }

            public int StockQuantity { get; set; }

            public string Address { get; set; }

            public string ImageUrl { get; set; }

            [EnumDataType(typeof(FlowerStatus))]
            public FlowerStatus Status { get; set; }

            public string? RejectReason { get; set; }

            public int Views { get; set; }

            public bool IsDeleted { get; set; }

            public DateTime? ExpireDate { get; set; }

            public DateTime? FlowerExpireDate { get; set; }


            [ForeignKey("FlowerCategory")]
            public string FlowerCategoryId { get; set; }

            public virtual FlowerCategory FlowerCategory { get; set; }

        }
    }
