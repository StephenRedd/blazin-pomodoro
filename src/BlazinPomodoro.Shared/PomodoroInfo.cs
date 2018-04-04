using System;

namespace BlazinPomodoro.Shared {
    public class PomodoroInfo
    {
        public string Id { get; set; }

        public DateTimeOffset? PausedAt { get; set; }

        public DateTimeOffset? ExpiresAt { get; set; }

        public PomodoroTimerState State { get; set; }
    }
}