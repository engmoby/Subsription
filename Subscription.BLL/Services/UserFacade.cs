using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Web.Http;
using AutoMapper;
using Newtonsoft.Json;
using Subscription.BLL.DataServices.Interfaces;
using Subscription.BLL.DTOs;
using Subscription.BLL.Services.Interfaces;
using Subscription.Common;
using Subscription.Common.CustomException;
using Repository.Pattern.UnitOfWork;
using Subscription.DAL.Entities.Model;
using HttpClient = System.Net.Http.HttpClient;
using HttpResponseMessage = System.Net.Http.HttpResponseMessage;

namespace Subscription.BLL.Services
{
    public class UserFacade : BaseFacade, IUserFacade
    {
        private readonly IUserService _userService;
        private readonly IProductFacade _productFacade;
        private readonly IUserProductService _productService;
        //static HttpClient client = new HttpClient();
        static string url = ConfigurationManager.AppSettings["EcatalogApiUrl"];

        public UserFacade(IUserService userService, IUnitOfWorkAsync unitOfWork, IProductFacade productFacade, IUserProductService productService) : base(unitOfWork)
        {
            _userService = userService;
            _productFacade = productFacade;
            _productService = productService;
        }

        public UserFacade(IUserService userService, IProductFacade productFacade, IUserProductService productService)
        {
            _userService = userService;
            _productFacade = productFacade;
            _productService = productService;
        }
        public UserDto ValidateUser(string email, string password)
        {
            string encryptedPassword = PasswordHelper.Encrypt(password);
            var user = Mapper.Map<UserDto>(_userService.ValidateUser(email, encryptedPassword)) ?? Mapper.Map<UserDto>(_userService.CheckUserIsDeleted(email, encryptedPassword));
            if (user == null) throw new ValidationException(ErrorCodes.UserNotFound);

            return user;
        }
        public UserDto GetUser(long userId)
        {
            return Mapper.Map<UserDto>(_userService.Find(userId));
        }

        public UserDto GetUserByAccountId(Guid userAccountId)
        {
            return Mapper.Map<UserDto>(_userService.Query(x => x.UserAccountId == userAccountId).Select().FirstOrDefault());
        }
        public UserDto RegisterUser(UserDto userDto)
        {
            if (GetUser(userDto.UserId) != null)
            {
                return EditUser(userDto);
            }
            if (_userService.CheckEmailDuplicated(userDto.Email))
            {
                throw new ValidationException(ErrorCodes.MailExist);
            }
            if (_userService.CheckPhoneDuplicated(userDto.Phone1))
            {
                throw new ValidationException(ErrorCodes.PhoneExist);
            }

            var userObj = Mapper.Map<User>(userDto);
            userObj.FirstName = userDto.FirstName;
            userObj.UserAccountId = Guid.NewGuid();
            userObj.LastName = userDto.LastName;
            userObj.Email = userDto.Email;
            userObj.Phone1 = userDto.Phone1;
            userObj.Phone2 = userDto.Phone2;
            userObj.Password = PasswordHelper.Encrypt(userDto.Password);
            userObj.Role = Enums.RoleType.Client;
            userObj.CreationTime = DateTime.Now;
            userObj.IsActive = userDto.IsActive;
            userObj.IsDeleted = false;
            _userService.Insert(userObj);
            SaveChanges();

            //string url = ConfigurationManager.AppSettings["EcatalogApiUrl"];
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://ecatalogbackend.azurewebsites.net/api/Users/Register");
            //request.ContentType = "application/json";
            //request.Method = "POST";
            //var serializer = JsonConvert.SerializeObject(new
            //{
            //    userName = userObj.Email,
            //    password = userObj.Password,
            //    userAccountId = userObj.UserAccountId,
            //    isActive = userObj.IsActive
            //});
            //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            //{
            //    string json = serializer;

            //    streamWriter.Write(json);
            //}
            //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //{

            //    Stream receiveStream = response.GetResponseStream();
            //    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            //    var infoResponse = readStream.ReadToEnd();

            //    response.Close();
            //    receiveStream.Close();
            //    readStream.Close();
            //}
            var userRetuenDto = Mapper.Map<UserDto>(userObj);
            return userRetuenDto;
        }

        public UserDto EditUserInfo(UserDto userDto)
        {
            var getUser = GetUser(userDto.UserId);
            var returnUser = EditUser(getUser);
            return returnUser;
        }
        public UserDto EditUser(UserDto userDto)
        {
            var userObj = _userService.Find(userDto.UserId);
            userObj.Phone1 = (userDto.Phone1 == "" || userDto.Phone1 == "0") ? userDto.Phone1 : userObj.Phone1;
            userObj.Phone2 = (userDto.Phone2 == "" || userDto.Phone2 == "0") ? userDto.Phone2 : null;
            userObj.Password = (userDto.Password != null) ? PasswordHelper.Encrypt(userDto.Password) : userObj.Password;
            userObj.IsActive = userDto.IsActive;
            userObj.IsDeleted = false;
            _userService.Update(userObj);
            SaveChanges();
            List<int> proudctList = new List<int>();

            //var proudctCount = _productFacade.GetAllProducts(Strings.DefaultLanguage).TotalCount;
            var userProudctList = _productService.GetProdccutByUserId(userObj.UserId);
            foreach (var objDto in (List<UserProductDto>)userProudctList.Data)
            {
                var productid = Convert.ToInt32(objDto.ProductId);
                if (!proudctList.Contains(productid))
                    proudctList.Add(productid);

            }
            foreach (var i in proudctList)
            {
                var productInfo = _productFacade.GetProduct(i);
                var url = productInfo.ApiUrl;
                //string url = ConfigurationManager.AppSettings["EcatalogApiUrl"];
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/api/Users");
                request.ContentType = "application/json";
                request.Method = "PUT";
                var serializer = JsonConvert.SerializeObject(new
                {
                    userName = userObj.Email,
                    password = userObj.Password,
                    userAccountId = userObj.UserAccountId,
                    isActive = userObj.IsActive
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
          
            var userRetuenDto = Mapper.Map<UserDto>(userObj);
            return userRetuenDto;
        }
        public PagedResultsDto GetAllProducts(int page, int pageSize)
        {
            var results = _userService.GetAllUsers(page, pageSize);
            return results;
        }

    }
}
