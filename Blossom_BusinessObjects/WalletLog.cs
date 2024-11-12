using Blossom_BusinessObjects.Entities;
using Blossom_BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blossom_BusinessObjects.Entities.Enums;

namespace Blossom_BusinessObjects
{
    public class WalletLog : BaseEntity
    {

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual Account User { get; set; }

        [Required]
        [Column("Type")]
        public WalletLogTypeEnum Type { get; set; }

        [Column("ActorEnum")]
        public WalletLogActorEnum? ActorEnum { get; set; }

        [Required]
        [Column("Amount", TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column("Balance", TypeName = "decimal(18,2)")]
        public decimal? Balance { get; set; }

        [EnumDataType(typeof(PaymentMethodEnum))]
        public PaymentMethodEnum PaymentMethod { get; set; }

        [EnumDataType(typeof(WalletLogStatusEnum))]
        public WalletLogStatusEnum Status { get; set; }

        [Column("IsRefund")]
        public bool? IsRefund { get; set; }
    }
}
