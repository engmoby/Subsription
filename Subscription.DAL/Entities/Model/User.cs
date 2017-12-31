using System;
using System.ComponentModel.DataAnnotations;
using Repository.Pattern.Ef6;
using Subscription.Common;

namespace Subscription.DAL.Entities.Model
{
    public class User : Entity
    {
        [Key]
        public long UserId { get; set; }
        [Required]
        public Guid UserAccountId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Phone1 { get; set; }
        public string Phone2 { get; set; } 
        [Required]
        public Enums.RoleType Role { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public long? DeleterUserId { get; set; }

    }
}
