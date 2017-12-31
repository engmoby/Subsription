using AutoMapper.Configuration;
using Subscription.API.Models;
using Subscription.BLL;
using Subscription.BLL.DTOs;

namespace Subscription.API
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {

            var mapperConfiguration = new MapperConfigurationExpression();

            mapperConfiguration.CreateMap<UserModel, UserDto>();
            mapperConfiguration.CreateMap<UserDto, UserModel>();

            mapperConfiguration.CreateMap<ProductModel, ProductDto>();
            mapperConfiguration.CreateMap<ProductDto, ProductModel>();

            mapperConfiguration.CreateMap<UserProductModel, UserProductDto>();
            mapperConfiguration.CreateMap<UserProductDto, UserProductModel>();

            mapperConfiguration.CreateMap<BackgroundModel, BackgroundDto>();
            mapperConfiguration.CreateMap<BackgroundDto, BackgroundModel>(); 

            SubscriptionBllConfig.RegisterMappings(mapperConfiguration); 

        }
    }
}