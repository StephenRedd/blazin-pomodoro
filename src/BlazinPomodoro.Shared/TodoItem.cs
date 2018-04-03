using System;

namespace BlazinPomodoro.Shared
{
    public class TodoItem
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public bool IsDone => CompletedOn.HasValue;

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset? CompletedOn { get; set; }
    }
}