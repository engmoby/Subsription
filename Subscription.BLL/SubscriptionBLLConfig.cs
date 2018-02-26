using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.Practices.Unity;
using Subscription.BLL.DataServices;
using Subscription.BLL.DataServices.Interfaces;
using Subscription.BLL.DTOs;
using Subscription.BLL.Services.FormToMail;
using Subscription.Common;
using Subscription.DAL;
using Subscription.DAL.Entities.Model;

namespace Subscription.BLL
{
    public static class SubscriptionBllConfig
    {
        public static void RegisterMappings(MapperConfigurationExpression mapperConfiguration)
        {
            mapperConfiguration.CreateMap<User, UserDto>()
                .ForMember(dto => dto.Password,m => m.MapFrom(src => PasswordHelper.Decrypt(src.Password)));
            mapperConfiguration.CreateMap<UserDto, User>();

            mapperConfiguration.CreateMap<Background, BackgroundDto>();
            mapperConfiguration.CreateMap<BackgroundDto, Background>();

            mapperConfiguration.CreateMap<UserProduct, UserProductDto>();
            mapperConfiguration.CreateMap<UserProductDto, UserProduct>();

            mapperConfiguration.CreateMap<ProductDto, Product>();
            mapperConfiguration.CreateMap<Product, ProductDto>()
                .ForMember(dto => dto.TitleDictionary, m => m.MapFrom(src => src.ProductTranslations.ToDictionary(translation => translation.Language.ToLower(), translation => translation.ProductName)));

            //.ForMember(dest => dest.ProductTitle, m => m.MapFrom(src => src.ProductTranslations.FirstOrDefault().ProductName))
            //.ForMember(dest => dest.ProductDesc, m => m.MapFrom(src => src.ProductTranslations.FirstOrDefault().ProductDescription));

            Mapper.Initialize(mapperConfiguration);
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            SubscriptionDalConfig.RegisterTypes(container);
            container
                .RegisterType<IBackgroundService, BackgroundService>(new PerResolveLifetimeManager())
                .RegisterType<IProductService, ProductService>(new PerResolveLifetimeManager())
                .RegisterType<IProductTranslationService, ProductTranslationService>(new PerResolveLifetimeManager())
                .RegisterType<IUserProductService, UserProductService>(new PerResolveLifetimeManager())

                .RegisterType<IUserService, UserService>(new PerResolveLifetimeManager())
                .RegisterType<IFormToMail, FormToMail>(new PerResolveLifetimeManager());
        }

    }
}
