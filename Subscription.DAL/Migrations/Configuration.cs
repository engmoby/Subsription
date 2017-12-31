using Subscription.Common;
using Subscription.DAL.Entities.Model;

namespace Subscription.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Entities.SubscriptionContext>
    {
        public Configuration()
        {
          AutomaticMigrationsEnabled = true;
           AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Entities.SubscriptionContext context)
        {
            //context.Users.Add(new User
            //{
            //    IsDeleted = false,
            //    Password = "wArilz/QIT55GuLgpRQlCHX0lir/WTXM8yc33MPiN3Bl26dnvS752gHPadYZoL20",
            //    Email = "admin@gmail.com",
            //    Role = Enums.RoleType.GlobalAdmin
            //});
            //context.Products.Add(new Product
            //{
            //    ProductId = 1,
            //    IsDeleted = false,
            //    IsActive = true
            //});
            //var product = context.Products.Find(1);
            //context.ProductTranslations.Add(new ProductTranslation
            //{
            //    Product = product,
            //    ProductId = 1,
            //    ProductName = "E-Gatalog",
            //    ProductDescription = " E-GatalogE-GatalogE-GatalogE-GatalogE-Gatalog",
            //    Language = "en-US"
            //});



            //context.Products.Add(new Product
            //{
            //    ProductId = 2,
            //    IsDeleted = false,
            //    IsActive = true
            //});
            //var product2 = context.Products.Find(2);
            //context.ProductTranslations.Add(new ProductTranslation
            //{
            //    Product = product2,
            //    ProductId = 2,
            //    ProductName = "Invetation",
            //    ProductDescription = "Invetation",
            //    Language = "en-US"
            //});
        }
    }

}
