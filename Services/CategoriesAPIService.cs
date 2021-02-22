using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RESTStoreAPI.Data;
using RESTStoreAPI.Data.DbModels;
using RESTStoreAPI.Models.Category;
using RESTStoreAPI.Models.Category.Get;
using RESTStoreAPI.Models.Category.Post;
using RESTStoreAPI.Models.Category.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace RESTStoreAPI.Services
{
    public interface ICategoriesAPIService
    {
        Task<CategoryFullResponce> GetCategoryAsync(int id);

        Task<CategoryTreeResponce> GetTreeCategoryAsync();

        Task<List<CategoryMinResponce>> GetCategoryPath(int id);
        Task<CategoryFullResponce> CreateCategoryNodeAsync(CreateCategoryRequest request);
        Task<CategoryFullResponce> CreateCategoryLeafAsync(CreateCategoryRequest request);
        Task<CategoryFullResponce> UpdateCategoryAsync(int id,UpdateCategoryRequest request);
        Task DeleteCategoryAsync(int id);
    }
    public class CategoriesAPIService : ICategoriesAPIService
    {
        private readonly DatabaseContext m_db;
        private readonly IMapper m_mapper;
        private readonly IAuthService m_authService;
        public CategoriesAPIService(DatabaseContext db, IMapper mapper, IAuthService authService)
        {
            m_db = db;
            m_mapper = mapper;
            m_authService = authService;
        }

        public async Task<CategoryFullResponce> GetCategoryAsync(int id)
        {
            CategoryDbModel result = await m_db.Categories.Include(x=> (x as CategoryNodeDbModel).ChildCategories).FirstOrDefaultAsync(x => x.Id == id);
            if (result is null)
                throw new NotFoundException();

            return m_mapper.Map<CategoryFullResponce>(result);
        }

        public async Task<CategoryTreeResponce> GetTreeCategoryAsync()
        {
            
            CategoryTreeResponce resultTree= new CategoryTreeResponce();

            var allCategories = await  m_db.Categories.Include(x=> (x as CategoryNodeDbModel).ChildCategories).ToListAsync();

            m_db.ChangeTracker.LazyLoadingEnabled = false; // фикс lazy-loading запросов

            
            IEnumerable<CategoryDbModel> rootCategories = allCategories.Where(x => x.ParentId == null);


            var mappedRootCat = rootCategories.Select(x =>m_mapper.Map<CategoryTreeElementResponce>(x)).ToList();
            resultTree.RootCategories =  mappedRootCat;

            return resultTree;
        }

        public async Task<List<CategoryMinResponce>> GetCategoryPath (int id)
        {
            List<CategoryMinResponce> result = new List<CategoryMinResponce>();

            var category = await m_db.Categories.Include(x=> x.Parent).FirstOrDefaultAsync(x => x.Id == id);

            if (category is null)
                throw new NotFoundException();

            result.Add(m_mapper.Map<CategoryMinResponce>(category));

            while (category.ParentId is not null)
            {
                if (category.Parent is null)
                {
                    await m_db.Entry(category).Reference(x => x.Parent).LoadAsync();
                }
                
                result.Add(m_mapper.Map<CategoryMinResponce>(category.Parent));

                category = category.Parent;

            }

            result.Reverse();

            return result ;
        }


        public async Task<CategoryFullResponce> CreateCategoryNodeAsync(CreateCategoryRequest request)
        {
            if (!m_authService.IsAuthUser())
                throw new AuthenticationException();
            if (!m_authService.AuthUserInRole(Roles.AdminRoleName))
                throw new UserNotAdminException();

            if (await m_db.Categories.AnyAsync(x => x.Name == request.Name))
                throw new CategoryNameAlreadyExistException();

            var newCategoryNode = m_mapper.Map<CategoryNodeDbModel>(request);

            if (request.ParentId.HasValue)
            {
                var parentNodeDb = await m_db.NodeCategories.Include(x => x.ChildCategories).FirstOrDefaultAsync(x => x.Id == request.ParentId);
                if (parentNodeDb is null)
                    throw new ParentNodeNotFoundException();

                parentNodeDb.ChildCategories.Add(newCategoryNode);
            }
            else
            {
                await m_db.NodeCategories.AddAsync(newCategoryNode);
            }

            await m_db.SaveChangesAsync();

            return m_mapper.Map<CategoryFullResponce>(newCategoryNode);
        }

        public async Task<CategoryFullResponce> CreateCategoryLeafAsync(CreateCategoryRequest request)
        {
            if (!m_authService.IsAuthUser())
                throw new AuthenticationException();
            if (!m_authService.AuthUserInRole(Roles.AdminRoleName))
                throw new UserNotAdminException();

            if (await m_db.Categories.AnyAsync(x => x.Name == request.Name))
                throw new CategoryNameAlreadyExistException();

            var newCategoryLeaf = m_mapper.Map<CategoryLeafDbModel>(request);

            if (request.ParentId.HasValue)
            {
                var parentNodeDb = await m_db.NodeCategories.Include(x => x.ChildCategories).FirstOrDefaultAsync(x => x.Id == request.ParentId);
                if (parentNodeDb is null)
                    throw new ParentNodeNotFoundException();

                parentNodeDb.ChildCategories.Add(newCategoryLeaf);
            }
            else
            {
                await m_db.LeafCategories.AddAsync(newCategoryLeaf);
            }

            await m_db.SaveChangesAsync();

            return m_mapper.Map<CategoryFullResponce>(newCategoryLeaf);
        }

        public async Task<CategoryFullResponce> UpdateCategoryAsync(int id, UpdateCategoryRequest request)
        {
            if (!m_authService.IsAuthUser())
                throw new AuthenticationException();
            if (!m_authService.AuthUserInRole(Roles.AdminRoleName))
                throw new UserNotAdminException();

            var updatedCategoryDb = await m_db.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (updatedCategoryDb is null)
                throw new NotFoundException();
            if (m_db.Categories.Where(x=> x.Id != updatedCategoryDb.Id).Any(x => x.Name == request.Name))
                throw new CategoryNameAlreadyExistException();
            if (request.ParentId != null)
            {
                if (m_db.NodeCategories.All(x => x.Id != request.ParentId))
                    throw new ParentNodeNotFoundException();
            }

            m_mapper.Map(request, updatedCategoryDb);

            await m_db.SaveChangesAsync();
            //TODO удалить комменты


            //if (updatedCategoryDb is CategoryNodeDbModel)
            //    return m_mapper.Map<CategoryNodeFullResponce>(updatedCategoryDb);
            //else if (updatedCategoryDb is CategoryLeafDbModel)
            //    return m_mapper.Map<CategoryLeafFullResponce>(updatedCategoryDb);
            //else
            //    return m_mapper.Map<CategoryFullResponce>(updatedCategoryDb);

            return m_mapper.Map<CategoryFullResponce>(updatedCategoryDb);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            if (!m_authService.IsAuthUser())
                throw new AuthenticationException();
            if (!m_authService.AuthUserInRole(Roles.AdminRoleName))
                throw new UserNotAdminException();

            var deletedCategoryDb = await m_db.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedCategoryDb is null)
                throw new NotFoundException();

            m_db.Categories.Remove(deletedCategoryDb);

            await m_db.SaveChangesAsync();
        }


    }

    class CategoryNameAlreadyExistException : Exception {}
    class ParentNodeNotFoundException : Exception { }
}
