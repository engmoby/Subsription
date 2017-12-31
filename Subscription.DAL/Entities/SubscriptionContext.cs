using System.Data.Entity;
using Repository.Pattern.Ef6;
using Subscription.DAL.Entities.Model;

namespace Subscription.DAL.Entities
{
    public class SubscriptionContext : DataContext
    {
        public DbSet<Background> Backgrounds { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }
        public DbSet<UserProduct> UserProducts{ get; set; }

        public SubscriptionContext() : base("name=SubscriptionDB")
        {
            Database.SetInitializer<SubscriptionContext>(null);
        }
    }
}
