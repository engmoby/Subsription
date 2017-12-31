using Service.Pattern;
using Subscription.BLL.DTOs;
using Subscription.DAL.Entities.Model;

namespace Subscription.BLL.DataServices.Interfaces
{
    public interface IProductService:IService<Product>
    {
        PagedResultsDto GetAllProducts(int page, int pageSize);
        PagedResultsDto GetProdcutByProductId(long productId);
    }
}
