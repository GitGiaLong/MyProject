namespace Core.Libraries.WPF.Interactivities
{
    public class EventTrigger : EventTriggerBase<object>
    {
        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }
        public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register(nameof(EventName), typeof(string),
            typeof(EventTrigger), new FrameworkPropertyMetadata("Loaded", new PropertyChangedCallback(OnEventNameChanged)));

        public EventTrigger() { }

        public EventTrigger(string eventName) { EventName = eventName; }

        protected override string GetEventName() { return EventName; }

        private static void OnEventNameChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            ((EventTrigger)sender).OnEventNameChanged((string)args.OldValue, (string)args.NewValue);
        }
    }
}
