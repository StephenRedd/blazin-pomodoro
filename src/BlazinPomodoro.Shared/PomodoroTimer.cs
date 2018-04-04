using System;
using System.Timers;

namespace BlazinPomodoro.Shared
{
    public class PomodoroTimer
    {
        public PomodoroTimer(string timerName, Action elapsedCallback, Action expiredCallback)
        {
            PomodoroInfo = new PomodoroInfo { Id = timerName };
            ElapsedCallback = elapsedCallback;
            ExpiredCallback = expiredCallback;
            Timer = new Timer(1000)
            {
                AutoReset = true
            };
            Timer.Elapsed += TimerOnElapsed;
        }

        private Action ElapsedCallback { get; }

        private Action ExpiredCallback { get; }

        private Timer Timer { get; }

        private PomodoroInfo PomodoroInfo { get; }

        public PomodoroTimerState Status => PomodoroInfo.State;

        public bool IsExpired => PomodoroInfo.State == PomodoroTimerState.Expired;

        public string TimeRemaining => PomodoroInfo.State == PomodoroTimerState.Paused
            ? ((PomodoroInfo.ExpiresAt ?? (PomodoroInfo.PausedAt ?? DateTimeOffset.Now)) - (PomodoroInfo.PausedAt ?? DateTimeOffset.Now)).ToString(@"mm\:ss")
            : ((PomodoroInfo.ExpiresAt ?? DateTimeOffset.Now) - DateTimeOffset.Now).ToString(@"mm\:ss");

        public void Start()
        {
            switch (PomodoroInfo.State)
            {
                case PomodoroTimerState.Stopped:
                    PomodoroInfo.ExpiresAt = DateTimeOffset.Now.AddMinutes(25);
                    break;
                case PomodoroTimerState.Running:
                case PomodoroTimerState.Expired:
                    //do nothing
                    return;
                case PomodoroTimerState.Paused:
                    if (PomodoroInfo.ExpiresAt.HasValue && PomodoroInfo.PausedAt.HasValue)
                    {
                        PomodoroInfo.ExpiresAt =
                            PomodoroInfo.ExpiresAt.Value.Add(DateTimeOffset.Now - PomodoroInfo.PausedAt.Value);
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            PomodoroInfo.PausedAt = null;
            PomodoroInfo.State = PomodoroTimerState.Running;
            Timer.Start();
        }

        public void Pause()
        {
            switch (PomodoroInfo.State)
            {
                case PomodoroTimerState.Stopped:
                case PomodoroTimerState.Expired:
                case PomodoroTimerState.Paused:
                    return;
                case PomodoroTimerState.Running:
                    PomodoroInfo.PausedAt = DateTimeOffset.Now;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            PomodoroInfo.State = PomodoroTimerState.Paused;
            Timer.Stop();
        }

        public void Reset()
        {
            PomodoroInfo.PausedAt = null;
            PomodoroInfo.ExpiresAt = null;
            PomodoroInfo.State = PomodoroTimerState.Stopped;
            Timer.Stop();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            ElapsedCallback?.Invoke();
            if (PomodoroInfo.State == PomodoroTimerState.Running && PomodoroInfo.ExpiresAt.HasValue &&
                PomodoroInfo.ExpiresAt <= DateTimeOffset.Now)
            {
                PomodoroInfo.State = PomodoroTimerState.Expired;
            }

            if (IsExpired)
            {
                ExpiredCallback?.Invoke();
            }
        }
    }
}