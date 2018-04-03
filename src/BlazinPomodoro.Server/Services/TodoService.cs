using System;
using System.Collections.Generic;
using System.IO;
using BlazinPomodoro.Shared;
using LiteDB;

namespace BlazinPomodoro.Server.Services
{
    public class TodoService : ITodoService
    {
        private readonly string _dbPath = Path.Combine(Directory.GetCurrentDirectory(), "app_data");

        private string DbFile => Path.Combine(_dbPath, "bp.db");
        public TodoService()
        {
            if (!Directory.Exists(_dbPath))
            {
                Directory.CreateDirectory(_dbPath);
            }
        }

        public TodoItem GetItem(Guid id)
        {
            using (var db = new LiteDatabase(DbFile))
            {
                var col = db.GetCollection<TodoItem>("TodoItems");
                return col.FindById(id);
            }
        }

        public bool SaveItem(TodoItem item)
        {
            using (var db = new LiteRepository(DbFile))
            {
                return db.Upsert(item);
            }
        }

        public bool DeleteItem(Guid id)
        {
            using (var db = new LiteRepository(DbFile))
            {
                return db.Delete<TodoItem>(i => i.Id == id) > 0;
            }
        }

        public IEnumerable<TodoItem> GetAllItems()
        {
            using (var db = new LiteRepository(DbFile))
            {
               return db.Query<TodoItem>().ToEnumerable();
            }
        }
    }
}
