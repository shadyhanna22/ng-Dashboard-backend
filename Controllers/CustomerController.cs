using Advantage.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Advantage.API.Controller
{
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ApiContext _ctx;

        public CustomerController(ApiContext _ctx)
        {
            this._ctx = _ctx;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_ctx.Customers.OrderBy(c => c.Id));
        }

        [HttpGet ("{id}", Name = "GetCustomer")]
        public IActionResult Get(Guid id)
        {
            return Ok(_ctx.Customers.Find(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody]Customer newCustomer)
        {
            if (newCustomer == null)
            {
                return BadRequest();
            }

            Customer customer = new()
            {
                Id = Guid.NewGuid(),
                Name = newCustomer.Name,
                Email = newCustomer.Email,
                Province = newCustomer.Province
            };

            _ctx.Customers.Add(customer);
            _ctx.SaveChanges();
            return CreatedAtRoute("GetCustomer", new { id = customer.Id }, customer);
        }

        // [HttpDelete("{Id}")]
        // public IActionResult DelectCustomer(Guid id)
        // {
        //     var existingCustomer = Get(id);
        //     if(existingCustomer is null)
        //     {
        //         return NotFound();
        //     }
        //     _ctx.Customers.Remove(existingCustomer);
        //     return NoContent();
        // }

    }
}