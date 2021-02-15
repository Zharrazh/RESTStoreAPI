using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RESTStoreAPI.Data;
using RESTStoreAPI.Models.Common;
using RESTStoreAPI.Models.User;
using RESTStoreAPI.Models.User.Get;
using RESTStoreAPI.Models.User.Update;
using RESTStoreAPI.Setup.Sieve;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Services
{
    public interface IUsersAPIService
    {
        Task<UserFullInfoResponce> GetUserAsync(int id);

        Task<PageResponce<UserFullInfoResponce>> GetUsersAsync(UserSieveModel sieveModel);

        Task<UserFullInfoResponce> UpdateUserAsync(int id, UserUpdateRequest request);
        Task<UserFullInfoResponce> UpdateUserPasswordAsync(int id, UserPasswordUpdateRequest request);


    }
    public class UsersAPIService : IUsersAPIService
    {
        private readonly DatabaseContext m_db;
        private readonly IMapper m_mapper;
        private readonly ISieveProcessor m_sieveProcessor;
        private readonly IAuthService m_authService;
        private readonly IPasswordService m_passwordService;
        public UsersAPIService(DatabaseContext db, IMapper mapper, ISieveProcessor sieveProcessor, IAuthService authService, IPasswordService passwordService)
        {
            m_db = db;
            m_mapper = mapper;
            m_sieveProcessor = sieveProcessor;
            m_authService = authService;
            m_passwordService = passwordService;
        }
        public async Task<UserFullInfoResponce> GetUserAsync(int id)
        {
            var userDbModel = await m_db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (userDbModel is null)
                throw new NotFoundException();

            return m_mapper.Map<UserFullInfoResponce>(userDbModel);
        }

        public Task<PageResponce<UserFullInfoResponce>> GetUsersAsync(UserSieveModel sieveModel)
        {
            var result = m_db.Users.AsNoTracking();
            result = m_sieveProcessor.ApplySorting(sieveModel, result);
            var paginationResult = m_sieveProcessor.ApplyFilteringAndPagination(sieveModel, result);
            return Task.FromResult(m_mapper.Map<PageResponce<UserFullInfoResponce>>(paginationResult));
        }

        public async Task<UserFullInfoResponce> UpdateUserAsync(int id ,UserUpdateRequest request)
        {
            var updatedUserDb = await m_db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (updatedUserDb is null)
                throw new NotFoundException();
                

            m_mapper.Map(request, updatedUserDb);

            await m_db.SaveChangesAsync();

            return m_mapper.Map<UserFullInfoResponce>(updatedUserDb);
        }

        public async Task<UserFullInfoResponce> UpdateUserPasswordAsync(int id, UserPasswordUpdateRequest request)
        {
            var updatedUser = await m_db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (updatedUser is null)
                throw new NotFoundException();

            if (!m_authService.IsAuthUser(updatedUser) && !m_authService.AuthUserInRole(Roles.AdminRoleName))
                throw new ForbidenPasswordUpdateException();

            updatedUser.PasswordHash = m_passwordService.SaltHash(request.NewPassword);
            updatedUser.Updated = DateTime.UtcNow;

            await m_db.SaveChangesAsync();
            return m_mapper.Map<UserFullInfoResponce>(updatedUser);
        }
    }

    public class NotFoundException : Exception { }
    public class ForbidenPasswordUpdateException : Exception { }
}
