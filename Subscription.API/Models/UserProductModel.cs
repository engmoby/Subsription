using System;

namespace Subscription.API.Models
{
    public class UserProductModel
    {
        public long UserProductId { get; set; }
        public long ProductId { get; set; }
        public long UserId { get; set; }
        public bool IsDeleted { get; set; }
        public int UserLimit { get; set; }
        public int UserConsumer { get; set; }
        public int TotalPrice { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public long? DeleterUserId { get; set; }
        public Guid BackageGuid { get; set; }
        public string ProductTitle { get; set; }

    }
}
