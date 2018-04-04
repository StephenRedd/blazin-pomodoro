using System;
using System.Timers;

namespace BlazinPomodoro.Shared
{
    public class PomodoroTimer
    {
        public PomodoroTimer(string timerName, Action elapsedCallback, Action expiredCallback,
            double pomodoroLength = 25d)
        {
            PomodoroInfo = new PomodoroInfo
            {
                Id = timerName,
                State = PomodoroTimerState.Stopped,
                PomodoroLength = pomodoroLength
            };
            ElapsedCallback = elapsedCallback;
            ExpiredCallback = expiredCallback;
            Timer = new Timer(500)
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

        public string TimeRemaining =>
            PomodoroInfo.State == PomodoroTimerState.Expired
                ? "00:00"
                : PomodoroInfo.State == PomodoroTimerState.Paused
                    ? ((PomodoroInfo.ExpiresAt ?? (PomodoroInfo.PausedAt ?? DateTimeOffset.Now)) -
                       (PomodoroInfo.PausedAt ?? DateTimeOffset.Now)).ToString(@"mm\:ss")
                    : ((PomodoroInfo.ExpiresAt ?? DateTimeOffset.Now.AddMinutes(PomodoroInfo.PomodoroLength)) -
                       DateTimeOffset.Now).ToString(@"mm\:ss");

        public void Start()
        {
            switch (PomodoroInfo.State)
            {
                case PomodoroTimerState.Stopped:
                case PomodoroTimerState.Expired:
                    PomodoroInfo.ExpiresAt = DateTimeOffset.Now.AddMinutes(PomodoroInfo.PomodoroLength);
                    break;
                case PomodoroTimerState.Running:
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
                PomodoroInfo.PausedAt = null;
                PomodoroInfo.ExpiresAt = null;
                ExpiredCallback?.Invoke();
                Timer.Stop();
            }
        }
    }
}