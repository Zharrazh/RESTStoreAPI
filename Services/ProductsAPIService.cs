using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RESTStoreAPI.Data;
using RESTStoreAPI.Data.DbModels;
using RESTStoreAPI.Models.Common;
using RESTStoreAPI.Models.Product;
using RESTStoreAPI.Setup.Sieve;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace RESTStoreAPI.Services
{
    public interface IProductsAPIService
    {
        Task<ProductResponce> GetAsync(int id);
        Task<PageResponce<ProductResponce>> GetAsync(ProductSieveModel sieve);
        Task<ProductResponce> CreateAsync(CreateProductRequest request);
        Task<ProductResponce> UpdateAsync(int id, UpdateProductRequest request);
        Task DeleteAsync(int id);

        Task UploadPicAsync(int productId, IFormFile file);
        Task DeletePicAsync(int productId, int fileId);
    }
    public class ProductsAPIService : IProductsAPIService
    {

        private readonly DatabaseContext m_db;
        private readonly IMapper m_mapper;
        private readonly ISieveProcessor m_sieveProcessor;
        private readonly IAuthService m_authService;
        private readonly IProductsFileRepoService m_productsFileRepo;
        public ProductsAPIService(DatabaseContext db, IMapper mapper, ISieveProcessor sieveProcessor, IAuthService authService, IProductsFileRepoService productsFileRepo)
        {
            m_db = db;
            m_mapper = mapper;
            m_sieveProcessor = sieveProcessor;
            m_authService = authService;
            m_productsFileRepo = productsFileRepo;
        }

        public async Task<ProductResponce> GetAsync(int id)
        {
            var productDb = await m_db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (productDb is null)
                throw new NotFoundException();

            return m_mapper.Map<ProductResponce>(productDb);
        }

        public Task<PageResponce<ProductResponce>> GetAsync(ProductSieveModel sieve)
        {
            var result = m_db.Products.Include(x => x.Pics).AsNoTracking();
            result = m_sieveProcessor.ApplySorting(sieve, result);
            var paginationResult = m_sieveProcessor.ApplyFilteringAndPagination(sieve, result);
            return Task.FromResult(m_mapper.Map<PageResponce<ProductResponce>>(paginationResult));

        }


        public async Task<ProductResponce> CreateAsync(CreateProductRequest request)
        {
            if (!m_authService.IsAuthUser())
                throw new AuthenticationException();
            if (!m_authService.AuthUserInRole(Roles.AdminRoleName))
                throw new UserNotAdminException();

            var categoryDb = await m_db.LeafCategories.FirstOrDefaultAsync(x => x.Id == request.CategoryId);
            if (categoryDb is null)
                throw new CategoryLeafNotFoundException();

            var newProduct = m_mapper.Map<ProductDbModel>(request);

            categoryDb.Products.Add(newProduct);

            await m_db.SaveChangesAsync();

            return m_mapper.Map<ProductResponce>(newProduct);

        }

        public async Task<ProductResponce> UpdateAsync(int id, UpdateProductRequest request)
        {
            if (!m_authService.IsAuthUser())
                throw new AuthenticationException();
            if (!m_authService.AuthUserInRole(Roles.AdminRoleName))
                throw new UserNotAdminException();

            var updatedProductDb = await m_db.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (updatedProductDb is null)
                throw new NotFoundException();

            var categoryDb = await m_db.LeafCategories.FirstOrDefaultAsync(x => x.Id == request.CategoryId);
            if (categoryDb is null)
                throw new CategoryLeafNotFoundException();

            m_mapper.Map(request, updatedProductDb);

            await m_db.SaveChangesAsync();

            return m_mapper.Map<ProductResponce>(updatedProductDb);

        }

        public async Task DeleteAsync(int id)
        {
            if (!m_authService.IsAuthUser())
                throw new AuthenticationException();
            if (!m_authService.AuthUserInRole(Roles.AdminRoleName))
                throw new UserNotAdminException();

            var deletingProduct = await m_db.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (deletingProduct is null)
                throw new NotFoundException();

            m_db.Entry(deletingProduct).State = EntityState.Deleted;

            await m_db.SaveChangesAsync();
        }

        public async Task UploadPicAsync(int productId, IFormFile file)
        {
            m_productsFileRepo.ValidateAndThrow(file); //throw UnavailableFileExtension, UnavailableFileSizeException

            await m_productsFileRepo.UploadPicAsync(productId, file); 
        }

        public async Task DeletePicAsync(int productId, int fileId)
        {
            await m_productsFileRepo.DeletePicAsync(productId, fileId);
        }
    }

    class CategoryLeafNotFoundException : Exception { }
}
