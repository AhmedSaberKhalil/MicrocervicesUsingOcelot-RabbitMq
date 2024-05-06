using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Models;
using UserAPI.Repository;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> repository;
        private readonly IBusControl _bus;

        public UserController(IRepository<User> repository, IBusControl bus)
        {
            this.repository = repository;
            this._bus = bus;
        }

        [HttpPost("AddTicket")]
        public async Task<IActionResult> CreateTicket(UserDeatails userDeatails)
        {
            if (userDeatails != null)
            {
                userDeatails.BookedOn = DateTime.Now;
                Uri uri = new Uri("rabbitmq://localhost/UserInfo");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(userDeatails);
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(repository.GetAll());
        }

        [HttpGet("GetById")]
        [Route("{id:int}", Name = "UserDetailsRoute")]
        public IActionResult GetById(int id)
        {
            return Ok(repository.GetById(id));
        }
        [HttpPost("Add/")]
        public IActionResult AddToCartItem(User entity)
        {
            if (ModelState.IsValid)
            {
                repository.Add(entity);
                string actionLink = Url.Link("UserDetailsRoute", new { id = entity.UserId });
                return Created(actionLink, entity);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
