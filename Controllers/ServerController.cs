using Advantage.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Advantage.API
{
    [Route("api/[controller]")]
    public class ServerController : ControllerBase
    {
        private readonly ApiContext _ctx;
        
        public ServerController (ApiContext _ctx)
        {
            this._ctx = _ctx;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var response = _ctx.Servers.OrderBy(s => s.Id).ToList();
            return Ok(response);
        }

        [HttpGet("{id}", Name="GetServer")]
        public IActionResult Get(Guid id)
        {
            var response = _ctx.Servers.First(s => s.Id == id);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult Message(Guid id, [FromBody] ServerMessage msg)
        {
            var server = _ctx.Servers.Find(id);

            if (server is null)
            {
                return NotFound();
            }

            if (msg.Payload == "activate")
            {
                server.IsOnline = true;
                _ctx.SaveChanges();
            }

            if (msg.Payload == "deactivate")
            {
                server.IsOnline = false;
                _ctx.SaveChanges();
            }

            return NoContent();
        }
    }
}