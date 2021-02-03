﻿using AutoMapper;
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
        private readonly ISieveProcessor sieveProcessor;
        private readonly IMapper mapper;
        public UsersController(DatabaseContext db, ISieveProcessor sieveProcessor, IMapper mapper)
        {
            this.db = db;
            this.sieveProcessor = sieveProcessor;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = RoleConstants.AdminRoleName)]
        [ProducesResponseType(typeof(List<UserFullInfo>), StatusCodes.Status200OK)]
        public IActionResult Get([FromQuery]SieveModel sieveModel)
        {
            var result = db.Users.AsNoTracking();
            result = sieveProcessor.Apply(sieveModel, result);
            return Ok(result.AsEnumerable().Select(x=>mapper.Map<UserFullInfo>(x)));
        }
    }
}
