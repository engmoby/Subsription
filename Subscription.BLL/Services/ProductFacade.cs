using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Newtonsoft.Json;
using Subscription.BLL.DataServices.Interfaces;
using Subscription.BLL.DTOs;
using Subscription.BLL.Services.Interfaces;
using Subscription.Common.CustomException;
using Repository.Pattern.UnitOfWork;
using Subscription.BLL.Services.FormToMail;
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
        private readonly IFormToMail _formToMail;
        static HttpClient client = new HttpClient();

        public ProductFacade(IProductService productService, IUnitOfWorkAsync unitOfWork, IProductTranslationService productTranslationService, IUserProductService userProductService, IUserService userService, IFormToMail formToMail) : base(unitOfWork)
        {
            _productService = productService;
            _productTranslationService = productTranslationService;
            _userProductService = userProductService;
            _userService = userService;
            _formToMail = formToMail;
            //   _formToMail = formToMail;
        }
        public ProductFacade(IProductService productService, IProductTranslationService productTranslationService, IUserProductService userProductService, IUserService userService, IFormToMail formToMail)
        {
            _productService = productService;
            _productTranslationService = productTranslationService;
            _userProductService = userProductService;
            _userService = userService;
            _formToMail = formToMail;
            //      _formToMail = formToMail;
        }
        public PagedResultsDto GetAllProducts(string language)
        {
            var results = _productTranslationService.GetAllProducts();
            return results;
        }
        public ProductDto GetProduct(long productId)
        {
            var returnVal = new ProductDto();
            var products = _productService.Query(x => !x.IsDeleted && x.ProductId == productId).Include(p => p.ProductTranslations).Select().OrderBy(x => x.ProductId).FirstOrDefault();
            returnVal = Mapper.Map<ProductDto>(products);
            return returnVal;
        }
        public void EditUserProdcutByUserId(long userId, int userConsumer, Guid backageGuid)
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

            //  _formToMail.SendMail("Product URL", "thanks for your regeistation ", "m.abdo@gmggroupsoft.com");
            var userAccountId = _userService.Find(userProductDto.UserId);

            var productInfo = GetProduct(userProductObj.ProductId);
            var url = productInfo.ApiUrl;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/api/Users/Register");
            request.ContentType = "application/json";
            request.Method = "POST";
            var serializer = JsonConvert.SerializeObject(new
            {
                userName = userAccountId.Email,
                password = userAccountId.Password,
                userAccountId = userAccountId.UserAccountId,
                isActive = userAccountId.IsActive,
                limit = userProductObj.UserLimit,
                packageGuid = userProductObj.BackageGuid,
                start = userProductObj.StartDate,
                end = userProductObj.EndDate,
               // productId = userProductObj.ProductId
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
            string body = PopulateBody(url, userAccountId.FirstName + " " + userAccountId.LastName, userAccountId.Email,
                PasswordHelper.Decrypt(userAccountId.Password), productInfo.TitleDictionary.Values.ElementAt(0), userProductObj.UserLimit.ToString(),
                userProductObj.StartDate.ToShortDateString(), userProductObj.EndDate.ToShortDateString());

            SendHtmlFormattedEmail(userAccountId.Email, "Account Details", body);
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

            var userAccountId = _userService.Find(userProductDto.UserId);
            if (userProductObj != null)
            {
                var productInfo = GetProduct(userProductObj.ProductId);
                var url = productInfo.ApiUrl;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/api/Users/Package");
                request.ContentType = "application/json";
                request.Method = "PUT";
                var serializer = JsonConvert.SerializeObject(new
                {
                    userAccountId = userAccountId.UserAccountId,
                    packageGuid = userProductDto.BackageGuid,
                    start = userProductDto.StartDate,
                    end = userProductObj.EndDate
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

            productObj.ProductTitle = trasnlate.TitleDictionary.Values.ElementAt(0);
            return productObj;
        }
        public void CreateProduct(ProductDto productDto)
        {
            var productObj = Mapper.Map<Product>(productDto);
            foreach (var productName in productDto.TitleDictionary)
            {
                productObj.ProductTranslations.Add(new ProductTranslation
                {
                    ProductName = productName.Value,
                    ProductDescription = productName.Value + " Desc",
                    Language = productName.Key
                });
            }
            _productTranslationService.InsertRange(productObj.ProductTranslations);
            _productService.Insert(productObj);
            SaveChanges();
        }
        public void EditProduct(ProductDto productDto)
        {
            var productObj = _productService.Find(productDto.ProductId);
            var producttraObj = _productTranslationService.Query(x => x.ProductId == productDto.ProductId).Select().ToList();

            if (productObj == null) throw new NotFoundException(ErrorCodes.ProductNotFound);
            //var currencyTrdddanslate = productObj.ProductTranslations.FirstOrDefault(x => x.ProductId == productDto.ProductId);

            foreach (var productName in productDto.TitleDictionary)
            {
                var productTranslation = productObj.ProductTranslations.FirstOrDefault(x => x.Language.ToLower() == productName.Key.ToLower() && x.ProductId == productDto.ProductId);

                if (productTranslation == null)
                {
                    productObj.ProductTranslations.Add(new ProductTranslation
                    {
                        ProductName = productName.Value,
                        ProductDescription = productName.Value + " Desc",
                        Language = productName.Key
                    });
                }
                else
                    productTranslation.ProductName = productName.Value;
            }
            productObj.ApiUrl = productDto.ApiUrl;
            _productService.Update(productObj);
            SaveChanges();
        }
        private string PopulateBody(string url, string username, string email, string password, string productTitle,
            string backageCount, string start, string end)
        {
            string body;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/MailTemplate.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Url}", url);
            body = body.Replace("{Email}", email);
            body = body.Replace("{UserName}", username);
            body = body.Replace("{Password}", password);
            body = body.Replace("{ProductTitle}", productTitle);
            body = body.Replace("{BackageCount}", backageCount);
            body = body.Replace("{Start}", start);
            body = body.Replace("{End}", end);
            return body;
        }
        private void SendHtmlFormattedEmail(string recepientEmail, string subject, string body)
        {
            string FromMail = "gmggroupsoftware@gmail.com";
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("in-v3.mailjet.com");
            mail.From = new MailAddress(FromMail);
            mail.To.Add(recepientEmail);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = body;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("9d7c1de804eabdf8fedf498bffadd546", "93187ce363c3beb198214badc25cdc3c");
            SmtpServer.EnableSsl = false;
            SmtpServer.Send(mail);

        }
    }
}
