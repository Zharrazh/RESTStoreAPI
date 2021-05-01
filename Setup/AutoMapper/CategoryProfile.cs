using AutoMapper;
using RESTStoreAPI.Data.DbModels;
using RESTStoreAPI.Models.Category;
using RESTStoreAPI.Models.Category.Post;
using RESTStoreAPI.Models.Category.Update;

namespace RESTStoreAPI.Setup.AutoMapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryDbModel, CategoryMinResponce>();

            CreateMap<CategoryLeafDbModel, CategoryMinResponce>()
                .IncludeBase<CategoryDbModel, CategoryMinResponce>()
                .ForMember(x => x.IsNode, opt => opt.MapFrom(src => false)).IgnoreAllPropertiesWithAnInaccessibleSetter();
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
                .ForMember(x => x.ChildCategories, opt => opt.Ignore())
                .IgnoreAllPropertiesWithAnInaccessibleSetter();


            CreateMap<CategoryDbModel, CategoryFullResponce>();

            CreateMap<CategoryNodeDbModel, CategoryFullResponce>()
                .IncludeBase<CategoryDbModel, CategoryFullResponce>()
                .ForMember(x => x.IsNode, opt => opt.MapFrom(src => true));
            CreateMap<CategoryLeafDbModel, CategoryFullResponce>()
                .IncludeBase<CategoryDbModel, CategoryFullResponce>()
                .ForMember(x => x.IsNode, opt => opt.MapFrom(src => false))
                .ForMember(x => x.ChildCategories, opt => opt.AllowNull())
                .IgnoreAllPropertiesWithAnInaccessibleSetter(); 


            CreateMap<CreateCategoryRequest, CategoryNodeDbModel>();
            CreateMap<CreateCategoryRequest, CategoryLeafDbModel>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<UpdateCategoryRequest, CategoryDbModel>();
            CreateMap<UpdateCategoryRequest, CategoryNodeDbModel>();
            CreateMap<UpdateCategoryRequest, CategoryLeafDbModel>().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
