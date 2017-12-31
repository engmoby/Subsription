using System;
using Subscription.Common;

namespace Subscription.API.Models
{
    public class UserModel
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public Enums.RoleType Role { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public long? DeleterUserId { get; set; } 
        public string UserProductTitle { get; set; }
        public int UserLimit { get; set; }
        public int UserConsumer { get; set; }
        public Guid UserAccountId { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid BackageGuid { get; set; }
        public int ProductCount { get; set; }

    }
}