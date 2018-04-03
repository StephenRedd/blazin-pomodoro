using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazinPomodoro.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazinPomodoro.Server.Controllers
{
    [Route("api/[controller]")]
    public class TodosController : Controller
    {
        [HttpGet]
        public IEnumerable<TodoItem> Get()
        {
            return new [] {
                new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Generated item",
                    CompletedOn = null,
                    CreatedOn = DateTimeOffset.Now.AddHours(-2)
                },
                new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Generated completed item",
                    CompletedOn = DateTimeOffset.Now.AddHours(-1),
                    CreatedOn = DateTimeOffset.Now.AddHours(-2)
                }
            };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
