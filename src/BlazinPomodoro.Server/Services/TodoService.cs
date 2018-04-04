using System;
using System.Collections.Generic;
using System.IO;
using BlazinPomodoro.Shared;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace BlazinPomodoro.Server.Services
{
    public class TodoService : ITodoService
    {
        private readonly string _dbPath = Path.Combine(Directory.GetCurrentDirectory(), "app_data");

        private Logger _dbLogger = null;

        private string DbFile => Path.Combine(_dbPath, "bp.db");

        private LiteRepository GetRepository() => new LiteRepository(new LiteDatabase(DbFile, null, _dbLogger));

        public TodoService(ILogger<TodoService> logger)
        {
            _dbLogger = new Logger(Logger.FULL, (s) => { logger.LogDebug(s); });
            if (!Directory.Exists(_dbPath))
            {
                Directory.CreateDirectory(_dbPath);
            }
        }

        public TodoItem GetItem(Guid id)
        {
            using (var db = GetRepository())
            {
                return db.SingleById<TodoItem>(id);
            }
        }

        public bool SaveItem(TodoItem item)
        {
            using (var db = GetRepository())
            {
                return db.Upsert(item);
            }
        }

        public bool DeleteItem(Guid id)
        {
            using (var db = GetRepository())
            {
                return db.Delete<TodoItem>(i => i.Id == id) > 0;
            }
        }

        public IEnumerable<TodoItem> GetAllItems()
        {
            using (var db = GetRepository())
            {
               return db.Query<TodoItem>().ToEnumerable();
            }
        }
    }
}
