using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blossom_BusinessObjects.Entities;

namespace Blossom_BusinessObjects
{
    public class Notification : BaseEntity
    {
        [Required]
        public string ReceiverId { get; set; }

        public virtual Account Receiver { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public string Message { get; set; }

        [Required]
        public NotificationTypeEnum Type { get; set; }

        [Required]
        public DestinationScreenEnum DestinationScreen { get; set; }

        public bool IsRead { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public enum NotificationTypeEnum
    {
        WELCOME,
        FLOWER_LISTING_STATUS,
        ORDER_STATUS,
        MARKETING
    }

    public enum DestinationScreenEnum
    {
        HOME,
        ORDER_DETAILS,
        PROMOTIONS,
        PROFILE
    }
}
