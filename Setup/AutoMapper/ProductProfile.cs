using AutoMapper;
using RESTStoreAPI.Data.DbModels;
using RESTStoreAPI.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Setup.AutoMapper
{
    public class ProductProfile: Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDbModel, ProductResponce>().IgnoreAllPropertiesWithAnInaccessibleSetter().ReverseMap();
            CreateMap<CreateProductRequest, ProductDbModel>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<UpdateProductRequest, ProductDbModel>().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
        
    }
}
