using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subscription.Common;

namespace Subscription.BLL.DTOs
{
    public class ProductDto
    {
        public long ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDesc { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public long? DeleterUserId { get; set; }
        public int Price { get; set; }
        public Dictionary<string, string> TitleDictionary { get; set; }
        public string ApiUrl { get; set; }


    }
}
