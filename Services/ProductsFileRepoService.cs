using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RESTStoreAPI.Data;
using RESTStoreAPI.Data.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Services
{
    public interface IProductsFileRepoService
    {
        void ValidateAndThrow(IFormFile file);
        Task UploadPicAsync(int productId, IFormFile file);
        Task DeletePicAsync(int productId, int fileId);
    }
    public class ProductsFileRepoService : IProductsFileRepoService
    {
        private readonly IFileRepositoryService m_repositoryService;
        private readonly DatabaseContext m_db;
        private readonly IMapper m_mapper;
        private static readonly string productRepoPath = "products";

        public ProductsFileRepoService(IFileRepositoryService repositoryService, DatabaseContext db, IMapper mapper)
        {
            m_repositoryService = repositoryService;
            m_db = db;
            m_mapper = mapper;
        }

        public async Task UploadPicAsync(int productId, IFormFile file)
        {
            ProductDbModel productDb = m_db.Products.FirstOrDefault(x => x.Id == productId);
            if (productDb is null)
                throw new NotFoundException();

            SavedFileInfo picInfo = await m_repositoryService.SaveFileAsync(file, $@"{productRepoPath}/{productId}", "galery", "jpeg");

            productDb.Pics.Add(m_mapper.Map<PictureInfoDbModel>(picInfo));

            await m_db.SaveChangesAsync();
            
        }

        public async Task DeletePicAsync(int productId, int fileId)
        {
            ProductDbModel productDb = m_db.Products.Include(x=> x.Pics).FirstOrDefault(x => x.Id == productId);
            if (productDb is null)
                throw new NotFoundException();

            var deletingProductPic =  productDb.Pics.FirstOrDefault(x => x.Id == fileId);

            m_repositoryService.DeleteFileAsync($@"{productRepoPath}/{productId}/{deletingProductPic.FileName}");

            m_db.Entry(deletingProductPic).State = EntityState.Deleted;

            await m_db.SaveChangesAsync();

        }

        

        public void ValidateAndThrow(IFormFile file)
        {
            m_repositoryService.ValidateAndThrow(file, new string[] { ".jpg", ".jpeg", ".png" });
        }
    }
}
