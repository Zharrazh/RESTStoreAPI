using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RESTStoreAPI.Data;
using RESTStoreAPI.Data.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ApiControllerBase
    {
        private readonly DatabaseContext m_db;
        private readonly IMapper m_mapper;
        public ProductsController(DatabaseContext db, IMapper mapper)
        {
            m_db = db;
            m_mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> Post ()
        {
            ProductDbModel testProduct = new ProductDbModel
            {
                Name = "test",
                Description = "testdesk",
                CategoryId = 1,
                Pics = new List<PictureInfoDbModel>{
                        new PictureInfoDbModel {Path="testpath1", FileName="testfilename1"},
                        new PictureInfoDbModel {Path="testpath2", FileName="testfilename2"}
                    }
            };
            m_db.Products.Add(testProduct);
            await m_db.SaveChangesAsync();

            return Ok(testProduct);
        }
    }
}
