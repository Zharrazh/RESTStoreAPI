using AutoMapper;
using Microsoft.AspNetCore.Http;
using RESTStoreAPI.Data;
using RESTStoreAPI.Data.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Services
{
    public interface ICategoriesFileRepoService
    {
        void ValidateAndThrow(IFormFile file);
        Task UploadPicAsync(int id,IFormFile file);
        Task DeletePicAsync(int id);
    }
    public class CategoriesFileRepoService : ICategoriesFileRepoService
    {
        private readonly IFileRepositoryService m_repositoryService;
        private readonly DatabaseContext m_db;
        private readonly IMapper m_mapper;
        private static readonly string categoryRepoPath = "categories";
        public CategoriesFileRepoService(IFileRepositoryService repositoryService, DatabaseContext db, IMapper mapper)
        {
            m_repositoryService = repositoryService;
            m_db = db;
            m_mapper = mapper;
        }
        public async Task DeletePicAsync(int id)
        {
            CategoryDbModel categoryDb = m_db.Categories.FirstOrDefault(x => x.Id == id);
            if (categoryDb is null)
                throw new NotFoundException();
            if (categoryDb.Pic is null)
                return;
            m_repositoryService.DeleteFileAsync($"{categoryRepoPath}\\{id}\\{categoryDb.Pic.FileName}");

            categoryDb.Pic = null;
            await m_db.SaveChangesAsync();
        }

        public async Task UploadPicAsync(int id, IFormFile file)
        {
            CategoryDbModel categoryDb = m_db.Categories.FirstOrDefault(x => x.Id == id);
            if (categoryDb is null)
                throw new NotFoundException();
            if (categoryDb.Pic is not null)
            {
                m_repositoryService.DeleteFileAsync(@$"{categoryRepoPath}/{id}/{categoryDb.Pic.FileName}");
            }

            SavedFileInfo picInfo =  await m_repositoryService.SaveFileAsync(file, @$"{categoryRepoPath}/{id}", "main", "jpeg");

            categoryDb.Pic = m_mapper.Map<PictureInfoDbModel>(picInfo);

            await m_db.SaveChangesAsync();

        }

        public void ValidateAndThrow(IFormFile file)
        {
            m_repositoryService.ValidateAndThrow(file, new string[] { ".jpg", ".jpeg", ".png" });
        }
    }

}
