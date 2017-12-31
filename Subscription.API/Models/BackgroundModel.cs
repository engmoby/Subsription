using System;

namespace Subscription.API.Models
{
    public class BackgroundModel
    {
        public long BackgroundId { get; set; }
        public long UserId { get; set; }  
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; }
        public bool IsImageChange { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public int? LastModifierUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public int? CreatorUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public int? DeleterUserId { get; set; } 
        public bool IsDeleted { get; set; }
    }
}