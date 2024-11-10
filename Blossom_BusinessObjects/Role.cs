using Microsoft.AspNetCore.Identity;

namespace Blossom_BusinessObjects.Entities
{
    public class Role : IdentityRole, IAuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        /// <summary>
        /// Navigation property for the users in this role.
        /// </summary>
        public ICollection<IdentityUserRole<string>> Users { get; } = new List<IdentityUserRole<string>>();

        /// <summary>
        /// Navigation property for claims in this role.
        /// </summary>
        public ICollection<IdentityRoleClaim<string>> Claims { get; } = new List<IdentityRoleClaim<string>>();
    }
}
