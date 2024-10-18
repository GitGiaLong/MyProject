using System.Windows.Threading;

namespace Core.Libraries.WPF.Interactivities.Extensions
{
    public class TimerTrigger : EventTrigger
    {
        public double MillisecondsPerTick
        {
            get { return (double)this.GetValue(MillisecondsPerTickProperty); }
            set { this.SetValue(MillisecondsPerTickProperty, value); }
        }
        public static readonly DependencyProperty MillisecondsPerTickProperty = DependencyProperty.Register(nameof(MillisecondsPerTick), typeof(double),
            typeof(TimerTrigger), new FrameworkPropertyMetadata(1000.0));

        public int TotalTicks
        {
            get { return (int)this.GetValue(TotalTicksProperty); }
            set { this.SetValue(TotalTicksProperty, value); }
        }
        public static readonly DependencyProperty TotalTicksProperty = DependencyProperty.Register(nameof(TotalTicks), typeof(int),
            typeof(TimerTrigger), new FrameworkPropertyMetadata(-1));

        private ITickTimer timer;
        private EventArgs eventArgs;
        private int tickCount;

        public TimerTrigger() : this(new DispatcherTickTimer()) { }

        internal TimerTrigger(ITickTimer timer)
        {
            this.timer = timer;
        }

        protected override void OnEvent(EventArgs eventArgs)
        {
            this.StopTimer();

            this.eventArgs = eventArgs;
            this.tickCount = 0;

            this.StartTimer();
        }

        protected override void OnDetaching()
        {
            this.StopTimer();

            base.OnDetaching();
        }

        internal void StartTimer()
        {
            if (this.timer != null)
            {
                this.timer.Interval = TimeSpan.FromMilliseconds(this.MillisecondsPerTick);
                this.timer.Tick += this.OnTimerTick;
                this.timer.Start();
            }
        }

        internal void StopTimer()
        {
            if (this.timer != null)
            {
                this.timer.Stop();
                this.timer.Tick -= this.OnTimerTick;
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (this.TotalTicks > 0 && ++this.tickCount >= this.TotalTicks)
            {
                this.StopTimer();
            }

            this.InvokeActions(this.eventArgs);
        }

        internal class DispatcherTickTimer : ITickTimer
        {
            private DispatcherTimer dispatcherTimer;

            public DispatcherTickTimer()
            {
                this.dispatcherTimer = new DispatcherTimer();
            }

            public event EventHandler Tick
            {
                add { this.dispatcherTimer.Tick += value; }
                remove { this.dispatcherTimer.Tick -= value; }
            }

            public TimeSpan Interval
            {
                get { return this.dispatcherTimer.Interval; }
                set { this.dispatcherTimer.Interval = value; }
            }

            public void Start()
            {
                this.dispatcherTimer.Start();
            }

            public void Stop()
            {
                this.dispatcherTimer.Stop();
            }
        }
    }
}
