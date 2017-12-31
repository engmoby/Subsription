using System;
using Service.Pattern;
using Subscription.BLL.DTOs;
using Subscription.DAL.Entities.Model;

namespace Subscription.BLL.DataServices.Interfaces
{
    public interface IUserProductService : IService<UserProduct>
    {
        PagedResultsDto GetProdccutByUserId(long userId);
        UserProduct GetProductByBackageId(Guid backageGuid);
    }
}
