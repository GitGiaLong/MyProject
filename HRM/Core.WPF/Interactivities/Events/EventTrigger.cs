using System.Windows;

namespace Core.WPF.Interactivities
{
    public class EventTrigger : EventTriggerBase<object>
    {
        public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register("EventName", typeof(string), typeof(EventTrigger), new FrameworkPropertyMetadata("Loaded", new PropertyChangedCallback(OnEventNameChanged)));

        public EventTrigger() { }

        public EventTrigger(string eventName) { EventName = eventName; }

        public string EventName
        {
            get { return (string)this.GetValue(EventNameProperty); }
            set { this.SetValue(EventNameProperty, value); }
        }

        protected override string GetEventName() { return EventName; }

        private static void OnEventNameChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            ((EventTrigger)sender).OnEventNameChanged((string)args.OldValue, (string)args.NewValue);
        }
    }
}
