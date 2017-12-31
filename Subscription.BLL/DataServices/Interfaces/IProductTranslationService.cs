using Service.Pattern;
using Subscription.BLL.DTOs;
using Subscription.DAL.Entities.Model;

namespace Subscription.BLL.DataServices.Interfaces
{
    public interface IProductTranslationService : IService<ProductTranslation>
    {
        PagedResultsDto GetAllProductsTranslation(string language);
        PagedResultsDto GetProductTranslationByProductId(string language, long productId);
        ProductDto ProductTranslationByProductId(string language, long productId);
    }
}
