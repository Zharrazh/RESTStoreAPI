using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTStoreAPI.Data;
using RESTStoreAPI.Models.Common.User;
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
        public UsersController(DatabaseContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(List<UserFullInfo>), StatusCodes.Status100Continue)]
        public IActionResult Get()
        {
            return Ok(db.Users.ToList().Select(x => x.ToFullInfo()));
        }
    }
}
