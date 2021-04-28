using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTStoreAPI.Data;
using RESTStoreAPI.Models.Common;
using RESTStoreAPI.Models.User;
using RESTStoreAPI.Models.User.Get;
using RESTStoreAPI.Models.User.Update;
using RESTStoreAPI.Services;
using RESTStoreAPI.Setup.Sieve;
using Sieve.Models;
using Sieve.Services;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {

        private readonly IUsersAPIService m_usersAPIServicee;
        public UsersController(IUsersAPIService usersAPIService)
        {
            m_usersAPIServicee = usersAPIService;
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = Roles.AdminRoleName)]
        [SwaggerOperation(
            Summary = "Получение информации о пользователе по id"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное получение информации о пользователе", typeof(UserFullInfoResponce))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Пользователь с таким id не найден")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            UserFullInfoResponce result;
            try
            {
                result = await m_usersAPIServicee.GetUserAsync(id);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = Roles.AdminRoleName)]
        [SwaggerOperation(
            Summary = "Получение информации о пользователях постранично с фильтрацией, сортировкой"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное получение информации о пользователе", typeof(PageResponce<UserFullInfoResponce>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Неправильно заполненая форма sieve", typeof(BadRequestType))]
        public async Task<IActionResult> GetAsync([FromQuery]UserSieveModel sieveModel )
        {
            PageResponce<UserFullInfoResponce> result = await m_usersAPIServicee.GetUsersAsync(sieveModel);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = Roles.AdminRoleName)]
        [SwaggerOperation(
            Summary = "Изменение записи о пользователе"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное изменение записи о пользователе", typeof(UserFullInfoResponce))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Неправильно заполненая форма sieve", typeof(BadRequestType))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Пользователь с таким id не найден")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserUpdateRequest request)
        {
            UserFullInfoResponce result;
            try
            {
                result = await m_usersAPIServicee.UpdateUserAsync(id, request);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Ok(result);

        }

        [HttpPut("{id:int}/updatePassword")]
        [Authorize]
        [SwaggerOperation(
            Summary = "Изменение пароля пользователя"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное изменение пароля пользователя", typeof(UserFullInfoResponce))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Неправильно заполненая форма изменения пароля", typeof(BadRequestType))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Пользователь с таким id не найден")]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Возникает если пароль пытается изменить не администратор и не владелец аккаутна", typeof(BadRequestType))]
        public async Task<IActionResult> UpdatePassword([FromRoute] int id, [FromBody] UserPasswordUpdateRequest request)
        {
            UserFullInfoResponce result;
            try
            {
                result = await m_usersAPIServicee.UpdateUserPasswordAsync(id, request);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ForbidenPasswordUpdateException)
            {
                ModelState.AddModelError("", "Only an administrator or account owner can update password");
                return StatusCode(StatusCodes.Status403Forbidden, new BadRequestType(ModelState));
            }

            return Ok(result);
        }

    }
}
