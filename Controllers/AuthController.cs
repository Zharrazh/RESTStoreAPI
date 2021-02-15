using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTStoreAPI.Models.Auth;
using RESTStoreAPI.Models.Auth.GetToken;
using RESTStoreAPI.Models.Auth.Register;
using RESTStoreAPI.Models.Common;
using RESTStoreAPI.Models.User;
using RESTStoreAPI.Services;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace RESTStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthAPIService m_authAPIService;

        public AuthController(IAuthAPIService authAPIService)
        {
            m_authAPIService = authAPIService;
        }

        [HttpPost("getToken")]
        [SwaggerOperation(
            Summary = "Получение токена аутентификации и информации о нем"
            )]
        [SwaggerResponse(StatusCodes.Status200OK,"Токен и его описание", typeof(TokenInfoResponce))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Ошибка в логине или пароле", typeof(BadRequestType))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "Пользователь не активен", typeof(BadRequestType))]

        public async Task<IActionResult> GetToken(GetTokenRequest request)
        {
            TokenInfoResponce result = default;
            try
            {
                result = await m_authAPIService.GetTokenAsync(request);
            }
            catch (WrongLoginOrPasswordException)
            {
                ModelState.AddModelError("", "Wrong login or password");
                return BadRequest(ModelState);
            }
            catch (UserNotActiveException)
            {
                ModelState.AddModelError("", "This user is inactive");
                return StatusCode(StatusCodes.Status406NotAcceptable,new BadRequestType(ModelState));
            }

            return Ok(result);
        }

        [HttpPost("registration")]
        [SwaggerOperation(
            Summary = "Регистрация нового пользователя (обычного или админа)"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешная регистрация нового пользователя", typeof(RegisterResponce))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Неправильно заполненая форма регистрации", typeof(BadRequestType))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Возникает при попытке создать пользователя с правами администратора, без аутентификации пользователя", typeof(BadRequestType))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Возникает при попытке создать пользователя с правами администратора, пользователем, без прав администратора", typeof(BadRequestType))]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            RegisterResponce result;

            try
            {
                result = await m_authAPIService.RegisterAsync(request);
            }
            catch (AuthenticationException)
            {
                ModelState.AddModelError("isAdmin", "Only an administrator can create a user with administrator rights");
                return Unauthorized(ModelState);
            }
            catch (UserNotAdminException)
            {
                ModelState.AddModelError("isAdmin", "Only an administrator can create a user with administrator rights");
                return StatusCode(StatusCodes.Status403Forbidden, new BadRequestType(ModelState));
            }

            return Ok(result);
        }

        [HttpGet("me")]
        [Authorize]
        [SwaggerOperation(
            Summary = "Получение информации о аутентифицированном пользователе"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешная регистрация нового пользователя", typeof(RegisterResponce))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Возникает если в базе данных не найдена информация о пользователе для таких credentials", typeof(BadRequestType))]
        public async Task<IActionResult> Me()
        {
            UserFullInfoResponce result;

            try
            {
                result = await m_authAPIService.MeAsync();
            }
            catch (InvalidCredentialException)
            {
                ModelState.AddModelError("", "No user information found for these credentials");
                return StatusCode(StatusCodes.Status404NotFound, new BadRequestType(ModelState));
            }

            return Ok(result);
        }
    }
}
