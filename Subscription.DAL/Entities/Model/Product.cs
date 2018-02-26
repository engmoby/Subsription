using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace Subscription.DAL.Entities.Model
{
    public sealed class Product : Entity
    {
        public Product()
        {
            ProductTranslations= new List<ProductTranslation>(); 
        }
        public ICollection<ProductTranslation> ProductTranslations { get; set; } 
        public long ProductId { get; set; } 
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public long? DeleterUserId { get; set; }
        public int Price { get; set; }
        public string ApiUrl { get; set; }
    }
}
