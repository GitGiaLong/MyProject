namespace Libraries.Custom.Interfaces.Interactivity
{
    interface ITickTimer
    {
        event EventHandler Tick;
        void Start();
        void Stop();
        TimeSpan Interval { get; set; }
    }
}
