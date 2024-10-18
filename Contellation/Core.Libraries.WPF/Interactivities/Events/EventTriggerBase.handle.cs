namespace Core.Libraries.WPF.Interactivities
{
    public abstract class EventTriggerBase<T> : EventTriggerBase where T : class
    {
        protected EventTriggerBase() : base(typeof(T)) { }

        public new T Source { get { return (T)base.Source; } }

        internal sealed override void OnSourceChangedImpl(object oldSource, object newSource)
        {
            base.OnSourceChangedImpl(oldSource, newSource);
            OnSourceChanged(oldSource as T, newSource as T);
        }

        protected virtual void OnSourceChanged(T oldSource, T newSource) { }
    }
}
