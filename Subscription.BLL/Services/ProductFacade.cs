using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using Subscription.BLL.DataServices.Interfaces;
using Subscription.BLL.DTOs;
using Subscription.BLL.Services.Interfaces;
using Subscription.Common.CustomException;
using Repository.Pattern.UnitOfWork;
using Subscription.BLL.Services.ManageStorage;
using Subscription.Common;
using Subscription.DAL.Entities.Model;

namespace Subscription.BLL.Services
{
    public class ProductFacade : BaseFacade, IProductFacade
    {
        private readonly IUserProductService _userProductService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly IProductTranslationService _productTranslationService;
     //   private readonly IFormToMail _formToMail;
        static HttpClient client = new HttpClient();

        public ProductFacade(IProductService productService, IUnitOfWorkAsync unitOfWork, IProductTranslationService productTranslationService, IUserProductService userProductService, IUserService userService) : base(unitOfWork)
        {
            _productService = productService;
            _productTranslationService = productTranslationService;
            _userProductService = userProductService;
            _userService = userService;
         //   _formToMail = formToMail;
        }

        public ProductFacade(IProductService productService, IProductTranslationService productTranslationService, IUserProductService userProductService, IUserService userService)
        {
            _productService = productService;
            _productTranslationService = productTranslationService;
            _userProductService = userProductService;
            _userService = userService;
      //      _formToMail = formToMail;
        }
        public PagedResultsDto GetAllProducts(string language)
        {
            var results = _productTranslationService.GetAllProductsTranslation(language);
            return results;
        }
        public ProductDto GetProduct(long productId)
        {
            return Mapper.Map<ProductDto>(_productService.Find(productId));
        }
        public void EditUserProdcutByUserId(long userId, int userConsumer, long productId, Guid backageGuid)
        {
            var query = _userProductService.Query(x => x.UserId == userId && x.BackageGuid == backageGuid).Select().FirstOrDefault();
            if (query != null) query.UserConsumer = userConsumer;
            _userProductService.Update(query);
            SaveChanges();
        }
        public void EditUserProductRequestByUserId(UserProductDto userProductDto)
        {
            var query = _userProductService.Query(x => x.UserId == userProductDto.UserId && x.BackageGuid == userProductDto.BackageGuid).Select().FirstOrDefault();
            if (query != null)
            {
                query.StartDate = userProductDto.StartDate;
                query.EndDate = userProductDto.EndDate;
            }
            _userProductService.Update(query);
            SaveChanges();
        }
        public void ProductRequest(UserProductDto userProductDto)
        {
            var userProduct = GetProductByBackageId(userProductDto);
            if (userProduct != null)
            {
                EditProductRequest(userProductDto);
                return;
            }
            var userProductObj = Mapper.Map<UserProduct>(userProductDto);
            userProductObj.UserId = userProductDto.UserId;
            userProductObj.ProductId = userProductDto.ProductId;
            userProductObj.UserLimit = userProductDto.UserLimit;
            userProductObj.TotalPrice = userProductDto.TotalPrice;
            userProductObj.StartDate = userProductDto.StartDate;
            userProductObj.EndDate = userProductDto.EndDate;
            userProductObj.BackageGuid = Guid.NewGuid();
            userProductObj.CreationTime = DateTime.Now;
            _userProductService.Insert(userProductObj);
            SaveChanges();
           //_formToMail.SendMail("Product URL", "thanks for your regeistation ", "eng.mohammadabdo@gmail.com");
            var userAccountId = _userService.Find(userProductDto.UserId);
            string url = ConfigurationManager.AppSettings["EcatalogApiUrl"];
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://ecatalogbackend.azurewebsites.net/api/Users/Package");
            request.ContentType = "application/json";
            request.Method = "PUT";
            var serializer = JsonConvert.SerializeObject(new
            {
                maxNumberOfWaiters = userProductObj.UserLimit,
                userAccountId = userAccountId.UserAccountId,
                packageGuid = userProductObj.BackageGuid,
                start = userProductObj.StartDate,
                end = userProductObj.EndDate,
            });
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = serializer;

                streamWriter.Write(json);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {

                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                var infoResponse = readStream.ReadToEnd();

                response.Close();
                receiveStream.Close();
                readStream.Close();
            }
        }
        public UserProductDto EditProductRequest(UserProductDto userProductDto)
        {
            var userProductObj = GetProductByBackageId(userProductDto);
            if (userProductObj != null)
            {
                userProductObj.StartDate = userProductDto.StartDate;
                userProductObj.EndDate = userProductDto.EndDate;
                _userProductService.Update(userProductObj);
            }
            SaveChanges();

            string url = ConfigurationManager.AppSettings["EcatalogApiUrl"];
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/Users/Package");
            request.ContentType = "application/json";
            request.Method = "PUT";
            if (userProductObj != null)
            {
                var serializer = JsonConvert.SerializeObject(new
                {
                    packageGuid = userProductObj.BackageGuid,
                    start = userProductObj.StartDate,
                    end = userProductObj.EndDate,
                });
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = serializer;

                    streamWriter.Write(json);
                }
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {

                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                var infoResponse = readStream.ReadToEnd();

                response.Close();
                receiveStream.Close();
                readStream.Close();
            }
            var userRetuenDto = Mapper.Map<UserProductDto>(userProductObj);
            return userRetuenDto;
        }
        public UserProduct GetProductByBackageId(UserProductDto userProductDto)
        {
            return _userProductService.Query(x => x.BackageGuid == userProductDto.BackageGuid).Select().FirstOrDefault();
        }
        public ProductDto GetProductInfo(string lang, long productId)
        {
            var productObj = Mapper.Map<ProductDto>(_productService.Find(productId));
            var trasnlate = _productTranslationService.ProductTranslationByProductId(lang, productObj.ProductId);

            productObj.ProductTitle = trasnlate.ProductTitle;
            return productObj;
        }
    }
}
