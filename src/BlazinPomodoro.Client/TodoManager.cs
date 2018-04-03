using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlazinPomodoro.Shared;
using Microsoft.AspNetCore.Blazor;

namespace BlazinPomodoro.Client
{
    public class TodoManager
    {
        private HttpClient Client { get; }

        public TodoManager(HttpClient httpClient)
        {
            Client = httpClient;
        }

        public async Task<IEnumerable<TodoItem>> GetTodosAsync()
        {
            var result = await Client.GetJsonAsync<IEnumerable<TodoItem>>("/api/todos");
            return result;
        }

        public async Task<TodoItem> ToggleCompleteTodoAsync(TodoItem item)
        {
            item.CompletedOn = item.CompletedOn.HasValue ? (DateTimeOffset?) null : DateTimeOffset.Now;
            await Client.PutJsonAsync($"api/todos/{item.Id}", item);
            return item;
        }

        public async Task<TodoItem> AddTodoAsync(string title)
        {
            var item = new TodoItem
            {
                Id = Guid.NewGuid(),
                Title = title,
                CreatedOn = DateTimeOffset.Now,
                CompletedOn = null
            };
            await Client.PostJsonAsync($"api/todos", item);
            return item;
        }
    }
}