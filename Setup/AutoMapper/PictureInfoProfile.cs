using AutoMapper;
using RESTStoreAPI.Data.DbModels;
using RESTStoreAPI.Models.Common;
using RESTStoreAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Setup.AutoMapper
{
    public class PictureInfoProfile : Profile
    {
        public PictureInfoProfile()
        {
            CreateMap<SavedFileInfo, PictureInfoDbModel>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<PictureInfoDbModel, PictureInfoResponce>().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
