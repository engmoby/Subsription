using System.Web.Http;
using AutoMapper;
using Subscription.API.Infrastructure;
using Subscription.API.Models;
using Subscription.API.Providers;
using Subscription.BLL.DTOs;
using Subscription.BLL.Services.Interfaces;
using Subscription.Common;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Subscription.BLL.DataServices.Interfaces;

namespace Subscription.API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IUserProductService _userProductService;
        private readonly IProductTranslationService _productTranslationService;
        private readonly IProductService _productService;
        private readonly IUserFacade _userFacade;
        private readonly IProductFacade _productFacade;
        public UsersController(IUserFacade userFacade, IUserService userService, IUserProductService userProductService, IProductService productService, IProductTranslationService productTranslationService, IProductFacade productFacade)
        {
            _userFacade = userFacade;
            _userService = userService;
            _userProductService = userProductService;
            _productService = productService;
            _productTranslationService = productTranslationService;
            _productFacade = productFacade;
        }
        [AuthorizeRoles(Enums.RoleType.GlobalAdmin)] 
        [Route("api/Users", Name = "RegisterUser")]
        [HttpPost]
        public IHttpActionResult RegisterUser([FromBody] UserModel userModel)
        {
            var reurnUser = _userFacade.RegisterUser(Mapper.Map<UserDto>(userModel));
 
            return Ok(reurnUser);
        }
        [AuthorizeRoles(Enums.RoleType.GlobalAdmin)]
        [Route("api/Users/EditRegisterUser", Name = "EditRegisterUser")]
        [HttpPost]
        public IHttpActionResult EditRegisterUser([FromBody] UserModel userModel)
        {
            var reurnUser = _userFacade.EditUserInfo(Mapper.Map<UserDto>(userModel));
            var userProduct = new UserProductDto();
            userProduct.UserId = reurnUser.UserId;
            userProduct.BackageGuid = userModel.BackageGuid;
            userProduct.StartDate = userModel.StartDate;
            userProduct.EndDate = userModel.EndDate;
            _productFacade.EditUserProductRequestByUserId(userProduct);
            return Ok(reurnUser);
        }
        [AuthorizeRoles(Enums.RoleType.GlobalAdmin)]
        [Route("api/Users/GetAllUsers", Name = "GetAllUsers")]
        [HttpGet]
        public IHttpActionResult GetAllUsers(int page = Page, int pagesize = PageSize)
        {
            var getAllDataForuser = _userService.GetAllUsers(page, pagesize);
            var userList = Mapper.Map<List<UserModel>>(getAllDataForuser.Data);
            foreach (var userModel in userList)
            {
               // var productCount = 0;
                var productList = Mapper.Map<List<UserProductModel>>(_userProductService.GetProdccutByUserId(userModel.UserId).Data);
                //foreach (var userProductModel in productList)
                //{
                //    productCount++;
                //    //userModel.UserLimit = userProductModel.UserLimit;
                //    //userModel.UserConsumer = userProductModel.UserConsumer;
                //    //userModel.StartDate = userProductModel.StartDate;
                //    //userModel.EndDate = userProductModel.EndDate;
                //    //var productInfo = Mapper.Map<List<ProductModel>>(_productTranslationService.GetProductTranslationByProductId(Language, userProductModel.ProductId).Data);

                //    //foreach (var productModel in productInfo)
                //    //{
                //    //    productCount++;
                //    //    userModel.UserProductTitle = productModel.ProductTitle;
                //    //}

                //}
                userModel.ProductCount = productList.Count;

            }

            PagedResultsDto results = new PagedResultsDto();
            results.TotalCount = getAllDataForuser.TotalCount;
            results.Data = Mapper.Map<List<UserModel>, List<UserDto>>(userList);

            //  return Ok(userList);
            return PagedResponse("GetAllUsers", Page, PageSize, results.TotalCount, userList, results.IsParentTranslated);


        }
         
        [Route("api/Users/EditUserConsumer", Name = "EditUserConsumer")]
        [HttpPost]
        public IHttpActionResult EditUserConsumer([FromBody] UserModel userModel)
        {
            var reurnUser = _userFacade.GetUserByAccountId(userModel.UserAccountId);
            _productFacade.EditUserProdcutByUserId(reurnUser.UserId, userModel.UserConsumer, 1, userModel.BackageGuid);
            return Ok(reurnUser);
        }
        [AuthorizeRoles(Enums.RoleType.GlobalAdmin)]
        [Route("api/Users/GetUserById", Name = "GetUserById")]
        [HttpGet]
        public IHttpActionResult GetUserById(long userId)
        {
            var reurnUser = _userFacade.GetUser(userId);
            return Ok(reurnUser);
        }
    }

}