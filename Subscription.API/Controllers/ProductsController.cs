using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using Subscription.API.Infrastructure;
using Subscription.API.Models;
using Subscription.API.Providers;
using Subscription.BLL.DataServices.Interfaces;
using Subscription.BLL.DTOs;
using Subscription.BLL.Services.Interfaces;
using Subscription.Common;

namespace Subscription.API.Controllers
{
    public class ProductsController : BaseApiController
    {

        private readonly IProductFacade _productFacade;
        private readonly IUserProductService _userProductService;
        private readonly IProductService _productService;
        private readonly IProductTranslationService _productTranslationService;
        public ProductsController(IProductFacade productFacade, IUserProductService userProductService, IProductTranslationService productTranslationService, IProductService productService)
        {
            _productFacade = productFacade;
            _userProductService = userProductService;
            _productTranslationService = productTranslationService;
            _productService = productService;
        }
        [AuthorizeRoles(Enums.RoleType.GlobalAdmin)]
        [Route("api/Products/GetAllProducts", Name = "GetAllProducts")]
        [HttpGet]
        public IHttpActionResult GetAllProducts()
        {
            var products = _productService.GetAllProducts(Page,PageSize);
            var data = Mapper.Map<List<ProductModel>>(products.Data);

            return PagedResponse("GetAllProducts", 10, 10, products.TotalCount, data, products.IsParentTranslated);


        }
        [AuthorizeRoles(Enums.RoleType.GlobalAdmin)]
        [Route("api/Products/AddProductRequest", Name = "AddProductRequest")]
        [HttpPost]
        public IHttpActionResult AddProductRequest([FromBody] UserProductModel userProductModel)
        {
            _productFacade.ProductRequest(Mapper.Map<UserProductDto>(userProductModel));
            return Ok();
        } 
        [AuthorizeRoles(Enums.RoleType.GlobalAdmin)]
        [Route("api/Products/GetUserProductByUserId", Name = "GetUserProductByUserId")]
        [HttpGet]
        public IHttpActionResult GetUserProductByUserId(long UserId)
        {
            var products = _userProductService.GetProdccutByUserId(UserId);
            foreach (var product in (List<UserProductDto>)products.Data)
            {
                product.ProductTitle = _productTranslationService.ProductTranslationByProductId(Language, product.ProductId).ProductTitle;
            }
            var data = Mapper.Map<List<UserProductModel>>(products.Data);

            return PagedResponse("GetUserProductByUserId", Page, PageSize, products.TotalCount, data, products.IsParentTranslated);

        }

        [AuthorizeRoles(Enums.RoleType.GlobalAdmin)]
        [Route("api/Products/GetProductByBackageId/{backageGuid:Guid}", Name = "GetProductByBackageId")]
        [HttpGet]
        public IHttpActionResult GetProductByBackageId(Guid backageGuid)
        {
            var productObj = _userProductService.GetProductByBackageId(backageGuid);
            var productInfo = _productFacade.GetProductInfo(Language, productObj.ProductId);

            BackageModel backageModel = new BackageModel();
            backageModel.ProductTitle = productInfo.ProductTitle;
            backageModel.UserLimit = productObj.UserLimit;
            backageModel.UserConsumer = productObj.UserConsumer;
            backageModel.StartDate = productObj.StartDate;
            backageModel.EndDate = productObj.EndDate;
            backageModel.UserId = productObj.UserId;
            backageModel.BackageGuid = backageGuid;

            //var data = Mapper.Map<List<UserProductModel>>(products.Data);

            //return PagedResponse("GetUserProductByUserId", Page, PageSize, products.TotalCount, data, products.IsParentTranslated);
            return Ok(backageModel);
        }
         
        [AuthorizeRoles(Enums.RoleType.GlobalAdmin)]
        [Route("api/Products/CreateProduct", Name = "CreateProduct")]
        [HttpPost]
        public IHttpActionResult CreateProduct([FromBody] ProductModel productModel)
        {
            _productFacade.CreateProduct(Mapper.Map<ProductDto>(productModel));
            return Ok();
        }

        [AuthorizeRoles(Enums.RoleType.GlobalAdmin)]
        [Route("api/Products/EditProduct", Name = "EditProduct")]
        [HttpPost]
        public IHttpActionResult EditProduct([FromBody] ProductModel productModel)
        {
            _productFacade.EditProduct(Mapper.Map<ProductDto>(productModel));
            return Ok();
        }


       // [AuthorizeRoles(Enums.RoleType.GlobalAdmin)]
        [Route("api/Products/GetProduct/{productId:long}", Name = "GetProduct")]
        [HttpGet]
        public IHttpActionResult GetProduct(long productId)
        {
            var reurnUser = _productFacade.GetProduct(productId);
            return Ok(reurnUser);
        }

    }

}