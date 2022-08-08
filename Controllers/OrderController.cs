using Advantage.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Advantage.API.Controller
{
    [Route("api/[controller]")]
    public class Ordercontroller : ControllerBase
    {
        private readonly ApiContext _ctx;

        public Ordercontroller(ApiContext _ctx)
        {
            this._ctx = _ctx;
        }

        // GET api/order/pageNumber/pagesize
        [HttpGet("{pageIndex:int}/{pageSize:int}")]
        public IActionResult Get(int pageIndex, int pageSize)
        {
            var data = _ctx.Orders.Include(o => o.Customer).OrderByDescending(c => c.Placed);

            var page = new PaginatedResponse<Order>(data, pageIndex, pageSize);

            var totalCount = data.Count();
            var totalPages = Math.Ceiling((double)totalCount / pageSize);

            var response = new
            {
                Page = page,
                TotalPages = totalPages
            };

            return Ok(response);
        }

        [HttpGet("GetOrder/{id}", Name = "getOrder")]
        public IActionResult Get(Guid id)
        {
            var data = _ctx.Orders.Include(c => c.Customer).First(o => o.Id == id);
            return Ok(data);
        }

        [HttpGet("ByProvince")]
        public IActionResult GetOrdersByProvince()
        {
            var data = _ctx.Orders.Include(o => o.Customer).ToList();

            var groupedResult = data.GroupBy(o => o.Customer.Province)
                .ToList()
                .Select(grp => new {
                    Province = grp.Key,
                    Total = grp.Sum(s => s.Total)
                }).OrderByDescending(res => res.Total).ToList();

            return Ok(groupedResult);
        }

        [HttpGet("ByCustomer")]
        public IActionResult GetOrdersByCustomer()
        {
            var data = _ctx.Orders.Include(o => o.Customer).ToList();

            var groupedResult = data.GroupBy(o => o.Customer.Id)
                .ToList()
                .Select(grp => new {
                    Name = _ctx.Customers.Find(grp.Key),
                    Total = grp.Sum(s => s.Total)
                }).OrderByDescending(res => res.Total).ToList();

            return Ok(groupedResult);
        }

        // top n clints
        [HttpGet("ByCustomer/{n}")]
        public IActionResult GetOrdersByCustomerTop(int n)
        {
            var data = _ctx.Orders.Include(o => o.Customer).ToList();

            var groupedResult = data.GroupBy(o => o.Customer.Id)
                .ToList()
                .Select(grp => new {
                    Name = _ctx.Customers.Find(grp.Key).Name,
                    Total = grp.Sum(s => s.Total)
                }).OrderByDescending(res => res.Total)
                .Take(n)
                .ToList();

            return Ok(groupedResult);
        }

        [HttpGet("totalday/{n}")]
        public IActionResult GetTotalByDays(int n)
        {
            var data = _ctx.Orders.ToList();

            var groupedResult = data.GroupBy(o => (o.Placed).ToShortDateString())
                .ToList()
                .Select(grp => new {
                    Placed = grp.Key,
                    Total = grp.Sum(s => s.Total)
                }).OrderByDescending(res => res.Placed)
                .Take(n)
                .ToList();

            return Ok(groupedResult);
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