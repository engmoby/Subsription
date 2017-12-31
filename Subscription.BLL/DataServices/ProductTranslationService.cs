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
    public class ProductTranslationService : Service<ProductTranslation>, IProductTranslationService
    {
        public ProductTranslationService(IRepositoryAsync<ProductTranslation> repository) : base(repository)
        {
            _repository = repository;
        }

        public PagedResultsDto GetAllProductsTranslation(string language)
        {
            PagedResultsDto results = new PagedResultsDto();
            results.TotalCount = _repository.Query(x => !x.Product.IsDeleted && x.Language.ToLower() == language.ToLower()).Select(x => x.Product).Count(x => !x.IsDeleted);
            var aaax = _repository.Query(x => !x.Product.IsDeleted && x.Language.ToLower() == language.ToLower()).Select().ToList();
            var products = _repository.Query(x => !x.Product.IsDeleted && x.Language.ToLower() == language.ToLower()).Select(x => x.Product)
                .OrderBy(x => x.ProductId).ToList();
            results.Data = Mapper.Map<List<Product>, List<ProductDto>>(products, opt =>
            {
                opt.BeforeMap((src, dest) =>
                    {
                        foreach (Product product in src)
                        {
                            product.ProductTranslations = product.ProductTranslations.Where(x => x.Language.ToLower() == language.ToLower()).ToList();
                        }

                    }
                );
            });
            return results;
        }
        public PagedResultsDto GetProductTranslationByProductId(string language,long productId)
        {
            PagedResultsDto results = new PagedResultsDto();
            results.TotalCount = _repository.Query(x => !x.Product.IsDeleted && x.Language.ToLower() == language.ToLower()  && x.ProductId == productId).Select(x => x.Product).Count(x => !x.IsDeleted);
            var aaax = _repository.Query(x => !x.Product.IsDeleted && x.Language.ToLower() == language.ToLower()).Select().ToList();
            var products = _repository.Query(x => !x.Product.IsDeleted && x.Language.ToLower() == language.ToLower() && x.ProductId == productId).Select(x => x.Product)
                .OrderBy(x => x.ProductId).ToList();
            results.Data = Mapper.Map<List<Product>, List<ProductDto>>(products, opt =>
            {
                opt.BeforeMap((src, dest) =>
                    {
                        foreach (Product product in src)
                        {
                            product.ProductTranslations = product.ProductTranslations.Where(x => x.Language.ToLower() == language.ToLower()).ToList();
                        }

                    }
                );
            });
            return results;
        }
        public ProductDto ProductTranslationByProductId(string language, long productId)
        {
            var aaax = _repository.Query(x => !x.Product.IsDeleted && x.Language.ToLower() == language.ToLower()).Select().ToList();
            var products = _repository.Query(x => !x.Product.IsDeleted && x.Language.ToLower() == language.ToLower() && x.ProductId == productId).Select(x => x.Product)
                .OrderBy(x => x.ProductId).FirstOrDefault();
            var results = Mapper.Map<Product, ProductDto>(products, opt =>
            {
                opt.BeforeMap((src, dest) =>
                    {

                        src.ProductTranslations = src.ProductTranslations.Where(x => x.Language.ToLower() == language.ToLower()).ToList();


                    }
                );
            });
            return results;
        }


    }
}