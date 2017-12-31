using System;
using Subscription.BLL.DTOs;
using Subscription.DAL.Entities.Model;

namespace Subscription.BLL.Services.Interfaces
{
    public interface IProductFacade
    {
        PagedResultsDto GetAllProducts(string language); 
        ProductDto GetProduct(long productId);
        void ProductRequest(UserProductDto userProductDto);
        void EditUserProductRequestByUserId(UserProductDto userProductDto);
        void EditUserProdcutByUserId(long userId, int userConsumer, long productId, Guid backageGuid);
        ProductDto GetProductInfo(string lang, long productId);
        UserProduct GetProductByBackageId(UserProductDto userProductDto);
        UserProductDto EditProductRequest(UserProductDto userProductDto);

    }
}
