namespace Core.Libraries.WPF.Interactivities
{
    public abstract class TargetedTriggerAction<T> : TargetedTriggerAction where T : class
    {
        protected TargetedTriggerAction() : base(typeof(T)) { }

        protected new T Target { get { return (T)base.Target; } }

        internal sealed override void OnTargetChangedImpl(object oldTarget, object newTarget)
        {
            base.OnTargetChangedImpl(oldTarget, newTarget);
            OnTargetChanged(oldTarget as T, newTarget as T);
        }

        protected virtual void OnTargetChanged(T oldTarget, T newTarget) { }
    }
}
