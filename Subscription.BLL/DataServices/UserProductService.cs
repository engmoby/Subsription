using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Repository.Pattern.Repositories;
using Service.Pattern;
using Subscription.BLL.DataServices.Interfaces;
using Subscription.BLL.DTOs;
using Subscription.Common;
using Subscription.DAL.Entities.Model;

namespace Subscription.BLL.DataServices
{
    public class UserProductService : Service<UserProduct>, IUserProductService
    {
        public UserProductService(IRepositoryAsync<UserProduct> repository) : base(repository)
        {
            _repository = repository;
        }

        public PagedResultsDto GetProdccutByUserId(long userId)
        {
            List<UserProduct> query = Queryable().Where(x => x.UserId == userId).ToList();
            PagedResultsDto results = new PagedResultsDto();
            results.TotalCount = query.Select(x => x).Count();
            results.Data = Mapper.Map<List<UserProduct>, List<UserProductDto>>(query);
          
            return results;
        }

        public UserProduct GetProductByBackageId(Guid backageGuid)
        {
            return  Queryable().FirstOrDefault(x => x.BackageGuid == backageGuid); 
        }
     
    }
}