using System.Reflection;

namespace Core.Libraries.WPF.Interactivities
{
    public sealed class EventObserver : IDisposable
    {
        private EventInfo eventInfo;
        private object target;
        private Delegate handler;

        public EventObserver(EventInfo eventInfo, object target, Delegate handler)
        {
            if (eventInfo == null)
            {
                throw new ArgumentNullException(nameof(eventInfo));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            this.eventInfo = eventInfo;
            this.target = target;
            this.handler = handler;
            this.eventInfo.AddEventHandler(this.target, handler);
        }

        public void Dispose()
        {
            eventInfo.RemoveEventHandler(target, handler);
        }
    }
}
