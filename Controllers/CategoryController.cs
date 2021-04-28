using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTStoreAPI.Models.Category;
using RESTStoreAPI.Models.Category.Get;
using RESTStoreAPI.Models.Category.Post;
using RESTStoreAPI.Models.Category.Update;
using RESTStoreAPI.Models.Common;
using RESTStoreAPI.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RESTStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ApiControllerBase
    {
        private readonly ICategoriesAPIService m_categoriesAPIService;
        private readonly ICategoriesFileRepoService m_catFileRepo;
        public CategoriesController(ICategoriesAPIService categoriesAPIService, ICategoriesFileRepoService catFileRepo)
        {
            m_categoriesAPIService = categoriesAPIService;
            m_catFileRepo = catFileRepo;
        }

        [HttpPut("{id:int}/pic")]
        [Authorize(Roles = Roles.AdminRoleName)]
        [SwaggerOperation(
            Summary = "Загрузка или обновление картинки для категории (jpg, jpeg, png)"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное обновление картинки")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Категория с таким Id не найдена")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Картинка не соответствует требованиям")]
        public async Task<IActionResult> UploadPic(int id, IFormFile file)
        {
            try
            {
                m_catFileRepo.ValidateAndThrow(file);
            }
            catch
            {
                return BadRequest();
            }

            try
            {
                await m_catFileRepo.UploadPicAsync(id, file);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }


            return Ok();

        }

        [HttpDelete("{id:int}/pic")]
        [Authorize(Roles = Roles.AdminRoleName)]
        [SwaggerOperation(
            Summary = "Удаление картинки для категории (jpg, jpeg, png)"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное обновление картинки")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Категория с таким Id не найдена")]
        public async Task<IActionResult> DeletePic(int id)
        {
            try
            {
                await m_catFileRepo.DeletePicAsync(id);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }


            return Ok();

        }

        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Получение полной информации о категории по id"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное получение полной информации о категории", typeof(CategoryFullResponce))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Категория с таким Id не найдена")]
        public async Task<IActionResult> GetCategoryAsync(int id)
        {
            CategoryFullResponce result;

            try
            {
                result = await m_categoriesAPIService.GetCategoryAsync(id);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("tree")]
        [SwaggerOperation(
            Summary = "Получение полного дерева категорий товаров"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное получение полного дерева категорий товаров", typeof(CategoryTreeResponce))]
        public async Task<IActionResult> GetTreeCategoriesAsync()
        {
            CategoryTreeResponce result = await m_categoriesAPIService.GetTreeCategoryAsync();

            return Ok(result);
        }

        [HttpGet("path/{id:int}")]
        [SwaggerOperation(
            Summary = "Получение полного пути для данной категории от корневой категории"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное получение полного пути для данной категории", typeof(List<CategoryMinResponce>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Категория с таким Id не найдена")]
        public async Task<IActionResult> GetCategoryPathAsync(int id)
        {
            List<CategoryMinResponce> result;
            try
            {
                result = await m_categoriesAPIService.GetCategoryPath(id);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = Roles.AdminRoleName)]
        [SwaggerOperation(
            Summary = "Создание категории (узла или листа)"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное создание новой категории", typeof(CategoryFullResponce))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Неправильно заполнены данные для создания категории (см. описание ошибки)", typeof(BadRequestType))]
        public async Task<IActionResult> CreateCategoryAsync(CreateCategoryRequest request)
        {
            CategoryFullResponce result;

            try
            {
                result = await m_categoriesAPIService.CreateCategoryAsync(request);
            }
            catch (CategoryNameAlreadyExistException)
            {
                ModelState.AddModelError(nameof(request.Name), "A category with the same name already exists");
                return ValidationProblem();
            }
            catch (ParentNodeNotFoundException)
            {
                ModelState.AddModelError(nameof(request.ParentId), "Сategory node with this id does not exist");
                return ValidationProblem();

            }

            return Ok(result);
        }



        [HttpPut("{id:int}")]
        [Authorize(Roles = Roles.AdminRoleName)]
        [SwaggerOperation(
            Summary = "Изменение категории"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное изменение категории", typeof(CategoryFullResponce))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Категория с таким Id не найдена")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Неправильно заполнены данные для изменения категории (см. описание ошибки)", typeof(BadRequestType))]
        public async Task<IActionResult> UpdateCategoryAsync(int id, UpdateCategoryRequest request)
        {
            CategoryFullResponce result;

            try
            {
                result = await m_categoriesAPIService.UpdateCategoryAsync(id, request);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (CategoryNameAlreadyExistException)
            {
                ModelState.AddModelError(nameof(request.Name), "A category with the same name already exists");
                return ValidationProblem();
            }
            catch (ParentNodeNotFoundException)
            {
                ModelState.AddModelError(nameof(request.ParentId), "Сategory node with this id does not exist");
                return ValidationProblem();
            }

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.AdminRoleName)]
        [SwaggerOperation(
            Summary = "Удаление категории"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное удаление категории")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Категория с таким Id не найдена")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            try
            {
                await m_categoriesAPIService.DeleteCategoryAsync(id);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
