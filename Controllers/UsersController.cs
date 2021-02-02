using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTStoreAPI.Data;
using RESTStoreAPI.Models.User;
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
        private readonly SieveProcessor sieveProcessor;
        public UsersController(DatabaseContext db, SieveProcessor sieveProcessor)
        {
            this.db = db;
            this.sieveProcessor = sieveProcessor;
        }

        [HttpGet]
        [Authorize(Roles = RoleConstants.AdminRoleName)]
        [ProducesResponseType(typeof(List<UserFullInfo>), StatusCodes.Status100Continue)]
        public IActionResult Get([FromQuery]SieveModel sieveModel)
        {
            var result = db.Users.AsNoTracking();
            result = sieveProcessor.Apply(sieveModel, result);
            return Ok(result.AsEnumerable().Select(x=>x.ToFullInfo()));
        }
    }
}
