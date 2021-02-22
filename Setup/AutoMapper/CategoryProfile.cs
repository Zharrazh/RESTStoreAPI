using AutoMapper;
using RESTStoreAPI.Data.DbModels;
using RESTStoreAPI.Models.Category;
using RESTStoreAPI.Models.Category.Post;
using RESTStoreAPI.Models.Category.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Setup.AutoMapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryDbModel, CategoryMinResponce>();

            CreateMap<CategoryLeafDbModel, CategoryMinResponce>()
                .IncludeBase<CategoryDbModel, CategoryMinResponce>()
                .ForMember(x => x.IsNode, opt => opt.MapFrom(src => false));
            CreateMap<CategoryNodeDbModel, CategoryMinResponce>()
                .IncludeBase<CategoryDbModel, CategoryMinResponce>()
                .ForMember(x => x.IsNode, opt => opt.MapFrom(src => true));


            CreateMap<CategoryDbModel, CategoryTreeElementResponce>();

            CreateMap<CategoryNodeDbModel, CategoryTreeElementResponce>()
                .IncludeBase<CategoryDbModel, CategoryTreeElementResponce>()
                .ForMember(x => x.IsNode, opt => opt.MapFrom(src => true));
            CreateMap<CategoryLeafDbModel, CategoryTreeElementResponce>()
                .IncludeBase<CategoryDbModel, CategoryTreeElementResponce>()
                .ForMember(x => x.IsNode, opt => opt.MapFrom(src => false))
                .ForMember(x => x.ChildCategories, opt => opt.Ignore());


            CreateMap<CategoryDbModel, CategoryFullResponce>();

            CreateMap<CategoryNodeDbModel, CategoryFullResponce>()
                .IncludeBase<CategoryDbModel, CategoryFullResponce>()
                .ForMember(x => x.IsNode, opt => opt.MapFrom(src => true));
            CreateMap<CategoryLeafDbModel, CategoryFullResponce>()
                .IncludeBase<CategoryDbModel, CategoryFullResponce>()
                .ForMember(x => x.IsNode, opt => opt.MapFrom(src => false))
                .ForMember(x => x.ChildCategories, opt => opt.AllowNull());


            CreateMap<CreateCategoryRequest, CategoryNodeDbModel>();
            CreateMap<CreateCategoryRequest, CategoryLeafDbModel>();
            CreateMap<UpdateCategoryRequest, CategoryDbModel>();
            CreateMap<UpdateCategoryRequest, CategoryNodeDbModel>();
            CreateMap<UpdateCategoryRequest, CategoryLeafDbModel>();
        }
    }
}
