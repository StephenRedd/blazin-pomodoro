using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazinPomodoro.Server.Services;
using BlazinPomodoro.Shared;
using LiteDB;
using Microsoft.AspNetCore.Mvc;

namespace BlazinPomodoro.Server.Controllers
{
    [Route("api/[controller]")]
    public class TodosController : Controller
    {
        private ITodoService Service { get; }

        public TodosController(ITodoService todoService)
        {
            Service = todoService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Service.GetAllItems());
        }

        [HttpGet("{id}", Name = "GetToDoById")]
        public IActionResult Get(Guid id)
        {
            var item = Service.GetItem(id);
            return item == null ? (IActionResult)NotFound() : Ok(item);

        }

        [HttpPost()]
        public IActionResult Post([FromBody]TodoItem item)
        {
            var res = Service.SaveItem(item)
                ? Created(new Uri($"/api/todos/{item.Id}", UriKind.Relative), item)
                : (IActionResult)StatusCode(500);
            return res;
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]TodoItem item)
        {

            var res = id == item.Id && Service.GetItem(id) != null //ids match and item exists
                ? Service.SaveItem(item)
                    ? Ok(item)
                    : (IActionResult)StatusCode(500)
                : BadRequest();
            return res;

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {

            return Service.DeleteItem(id) ? (IActionResult)NoContent(): BadRequest();
        }
    }
}
