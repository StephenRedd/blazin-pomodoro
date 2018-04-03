using System;
using System.Collections.Generic;
using BlazinPomodoro.Shared;

namespace BlazinPomodoro.Server.Services
{
    public interface ITodoService
    {
        bool DeleteItem(Guid id);
        IEnumerable<TodoItem> GetAllItems();
        TodoItem GetItem(Guid id);
        bool SaveItem(TodoItem item);
    }
}