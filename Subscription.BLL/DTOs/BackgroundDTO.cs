using System;
using System.IO;

namespace Subscription.BLL.DTOs
{
    public class BackgroundDto
    {
        public long BackgroundId { get; set; }
        public MemoryStream Image { get; set; }
        public long UserId { get; set; }
        public bool IsPrivate { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsImageChange { get; set; }

        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public long? DeleterUserId { get; set; }
    }
}
