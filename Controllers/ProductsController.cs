using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTStoreAPI.Data;
using RESTStoreAPI.Models.Common;
using RESTStoreAPI.Models.Product;
using RESTStoreAPI.Services;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace RESTStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ApiControllerBase
    {

        private readonly IProductsAPIService m_productsAPIService;
        public ProductsController(IProductsAPIService productsAPIService)
        {

            m_productsAPIService = productsAPIService;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Получение страницы продуктов (с фильтрацией и сортировкой)"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное получение страницы", typeof(PageResponce<ProductResponce>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Ошибка в запросе")]
        public async Task<IActionResult> Get([FromQuery] ProductSieveModel sieve)
        {

            PageResponce<ProductResponce> productPage = await m_productsAPIService.GetAsync(sieve);
            return Ok(productPage);

        }

        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Получение продукта по id"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное получение продукта", typeof(ProductResponce))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Продукт не найден")]

        public async Task<IActionResult> Get(int id)
        {
            try
            {
                ProductResponce product = await m_productsAPIService.GetAsync(id);
                return Ok(product);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        

        [HttpPost]
        [Authorize(Roles = Roles.AdminRoleName)]
        [SwaggerOperation(
            Summary = "Создание нового продукта"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное создание продукта", typeof(ProductResponce))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Ошибка в запросе")]

        public async Task<IActionResult> Post(CreateProductRequest request)
        {
            try
            {
                ProductResponce product = await m_productsAPIService.CreateAsync(request);
                return Ok(product);
            }
            catch (CategoryLeafNotFoundException)
            {
                ModelState.AddModelError(nameof(CreateProductRequest.CategoryId), "Сategory leaf with this id does not exist");
                return ValidationProblem();
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = Roles.AdminRoleName)]
        [SwaggerOperation(
            Summary = "Изменение продукта"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное изменение продукта", typeof(ProductResponce))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Ошибка в запросе")]

        public async Task<IActionResult> Update(int id, UpdateProductRequest request)
        {
            try
            {
                ProductResponce product = await m_productsAPIService.UpdateAsync(id, request);
                return Ok(product);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (CategoryLeafNotFoundException)
            {
                ModelState.AddModelError(nameof(CreateProductRequest.CategoryId), "Сategory leaf with this id does not exist");
                return ValidationProblem();
            }

        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.AdminRoleName)]
        [SwaggerOperation(
            Summary = "Удаление продукта"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное удаление продукта")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Продукт с таким id не найден")]

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await m_productsAPIService.DeleteAsync(id);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }

        [HttpPut("pic/{id:int}")]
        [Authorize(Roles = Roles.AdminRoleName)]
        [SwaggerOperation(
            Summary = "Загрузка картинки в галерею продукта"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешная загрузка")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Продукт с таким id не найден")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Файл не соответствует требованиям")]


        public async Task<IActionResult> UploadPic(int id, IFormFile file)
        {
            try
            {
                await m_productsAPIService.UploadPicAsync(id, file);
                return Ok();
            }
            catch (UnavailableFileExtensionException)
            {
                ModelState.AddModelError(nameof(file), "Unavaible file extension");
                return ValidationProblem();
            }
            catch (UnavailableFileSizeException)
            {
                ModelState.AddModelError(nameof(file), "Unavaible file size");
                return ValidationProblem();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }

        [HttpDelete("pic/{productId:int}")]
        [Authorize(Roles = Roles.AdminRoleName)]
        [Authorize(Roles = Roles.AdminRoleName)]
        [SwaggerOperation(
            Summary = "Удаление картинки из галереи продукта"
            )]
        [SwaggerResponse(StatusCodes.Status200OK, "Успешное удаление")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Продукт или картинка с таким id не найдена")]

        public async Task<IActionResult> DeletePic(int productId, [FromQuery] int fileId)
        {
            try
            {
                await m_productsAPIService.DeletePicAsync(productId, fileId);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

        }

    }
}
