using System.Diagnostics;

namespace Core.WPF.Interactivities
{
    public class TriggerActionCollection : AttachableCollection<TriggerAction>
    {
        internal TriggerActionCollection() { }

        protected override void OnAttached()
        {
            foreach (TriggerAction action in this)
            {
                Debug.Assert(action.IsHosted, "Action must be hosted if it is in the collection.");
                action.Attach(AssociatedObject);
            }
        }

        protected override void OnDetaching()
        {
            foreach (TriggerAction action in this)
            {
                Debug.Assert(action.IsHosted, "Action must be hosted if it is in the collection.");
                action.Detach();
            }
        }

        internal override void ItemAdded(TriggerAction item)
        {
            if (item.IsHosted)
            {
                throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerActionMultipleTimesExceptionMessage);
            }
            if (AssociatedObject != null)
            {
                item.Attach(AssociatedObject);
            }
            item.IsHosted = true;
        }
        internal override void ItemRemoved(TriggerAction item)
        {
            Debug.Assert(item.IsHosted, "Item should hosted if it is being removed from a TriggerCollection.");
            if (((IBehavior)item).AssociatedObject != null)
            {
                item.Detach();
            }
            item.IsHosted = false;
        }
        protected override Freezable CreateInstanceCore()
        {
            return new TriggerActionCollection();
        }
    }
}
