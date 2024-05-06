using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.Respository;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> repository;

        public ProductController(IRepository<Product> repository)
        {
            this.repository = repository;
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(repository.GetAll());
        }

        [HttpGet("GetById")]
        [Route("{id:int}", Name = "ProducttDetailsRoute")]
        public IActionResult GetById(int id)
        {
            return Ok(repository.GetById(id));
        }
        [HttpPost("Add/")]
        public IActionResult AddToCartItem(Product entity)
        {
            if (ModelState.IsValid)
            {
                repository.Add(entity);
                string actionLink = Url.Link("CartItemDetailsRoute", new { id = entity.ProductId });
                return Created(actionLink, entity);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPut("update/{id}")]
        public IActionResult Edit(int id, Product entity)
        {
            if (ModelState.IsValid)
            {
                if (id == entity.ProductId)
                {                  
                    repository.Update(id, entity);
                    return StatusCode(StatusCodes.Status204NoContent);
                }
                return BadRequest("Invalied data");
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            Product product = repository.GetById(id);
            if (product == null)
            {
                return NotFound("Data Not Found");
            }
            else
            {
                try
                {
                    repository.Delete(product);
                    return StatusCode(StatusCodes.Status204NoContent);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
        }
    }
}
