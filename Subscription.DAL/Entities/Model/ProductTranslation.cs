using Repository.Pattern.Ef6;

namespace Subscription.DAL.Entities.Model
{
    public class ProductTranslation: Entity
    {
        public long ProductTranslationId { get; set; }
        public string Language { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public long ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
