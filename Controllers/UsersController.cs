using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTStoreAPI.Data;
using RESTStoreAPI.Models.Common;
using RESTStoreAPI.Models.User;
using RESTStoreAPI.Models.User.Update;
using RESTStoreAPI.Setup.Sieve;
using RESTStoreAPI.Utils.Constants;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext db;
        private readonly ISieveProcessor sieveProcessor;
        private readonly IMapper mapper;
        public UsersController(DatabaseContext db, ISieveProcessor sieveProcessor, IMapper mapper)
        {
            this.db = db;
            this.sieveProcessor = sieveProcessor;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = RoleConstants.AdminRoleName)]
        [ProducesResponseType(typeof(UserFullInfoResponce), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get([FromRoute] int id)
        {
            var userDbModel = db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (userDbModel == null)
                return NotFound();
            return Ok(mapper.Map<UserFullInfoResponce>(userDbModel));
        }

        [HttpGet]
        [Authorize(Roles = RoleConstants.AdminRoleName)]
        [ProducesResponseType(typeof(PageResponce<UserFullInfoResponce>), StatusCodes.Status200OK)]
        public IActionResult Get([FromQuery]SieveModel sieveModel)
        {
            var result = db.Users.AsNoTracking();
            result = sieveProcessor.ApplySorting(sieveModel, result);
            var paginationResult = sieveProcessor.ApplyOrderingAndPagination(sieveModel, result);
            return Ok(mapper.Map<PageResponce<UserFullInfoResponce>>(paginationResult));
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = RoleConstants.AdminRoleName)]
        [ProducesResponseType(typeof(UserFullInfoResponce), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestType), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserUpdateRequest request)
        {
            var updatedUserDb = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (updatedUserDb == null)
            {
                return NotFound();
            }

            mapper.Map(request, updatedUserDb);

            await db.SaveChangesAsync();

            return Ok(mapper.Map<UserFullInfoResponce>(updatedUserDb));
        }

    }
}
