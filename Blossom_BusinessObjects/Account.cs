using Blossom_BusinessObjects.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blossom_BusinessObjects.Entities
{
    public class Account : IdentityUser, IAuditableEntity
    {
        public string FullName { get; set; }

        public string Avatar { get; set; }

        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }
        public String Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Balance { get; set; }
        public double FeeService { get; set; }
        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public ICollection<IdentityUserRole<string>> Roles { get; } = new List<IdentityUserRole<string>>();

        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public ICollection<IdentityUserClaim<string>> Claims { get; } = new List<IdentityUserClaim<string>>();

    }
}
