using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlazinPomodoro.Shared;
using Microsoft.AspNetCore.Blazor;

namespace BlazinPomodoro.Client.Services
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

            var result = await Client.PutAsync($"api/todos/{item.Id}",
                new StringContent(JsonUtil.Serialize(item), Encoding.UTF8, "application/json"));
            return await TodoItemFromHttpResponseAsync(result);
        }

        public async Task<bool> DeleteTodoAsync(TodoItem item)
        {
            Console.WriteLine($"Deleting {item.Title} with ID {item.Id}");
            var result = await Client.DeleteAsync($"api/todos/{item.Id}");
            return result.IsSuccessStatusCode;
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
            var result = await Client.PostAsync($"api/todos",
                new StringContent(JsonUtil.Serialize(item), Encoding.UTF8, "application/json"));
            return await TodoItemFromHttpResponseAsync(result);
        }

        private async Task<TodoItem> TodoItemFromHttpResponseAsync(HttpResponseMessage message)
        {
            TodoItem result = null;
            if (message.IsSuccessStatusCode)
            {
                result = JsonUtil.Deserialize<TodoItem>(await message.Content.ReadAsStringAsync());
            }

            return result;
        }
    }
}