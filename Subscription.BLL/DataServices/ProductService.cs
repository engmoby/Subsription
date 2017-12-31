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
    public class ProductService : Service<Product>, IProductService
    {
        public ProductService(IRepositoryAsync<Product> repository) : base(repository)
        {
            _repository = repository;
        }

        public PagedResultsDto GetAllProducts(int page, int pageSize)
        {
            var query = Queryable().Where(x => x.IsActive);
            PagedResultsDto results = new PagedResultsDto();
            results.TotalCount = query.Select(x => x).Count();

            results.Data = Mapper.Map<List<Product>, List<ProductDto>>(query.OrderBy(x => x.ProductId).Skip((page - 1) * pageSize)
                .Take(pageSize).ToList());

            return results;
        }
        public PagedResultsDto GetProdcutByProductId(long productId)
        {
            List<Product> query = Queryable().Where(x => x.ProductId == productId).ToList();
            PagedResultsDto results = new PagedResultsDto();
            results.TotalCount = query.Select(x => x).Count();
            results.Data = Mapper.Map<List<Product>, List<ProductDto>>(query);
            return results;
        }
    }
}