using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace Core.Libraries.WPF.Interactivities
{
    public abstract class AttachableCollection<T> : FreezableCollection<T>, IBehavior where T : DependencyObject, IBehavior
    {
        private Collection<T> snapshot;
        private DependencyObject associatedObject;

        protected DependencyObject AssociatedObject
        {
            get
            {
                ReadPreamble();
                return associatedObject;
            }
        }

        internal AttachableCollection()
        {
            INotifyCollectionChanged notifyCollectionChanged = this;
            notifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(OnCollectionChanged);

            snapshot = new Collection<T>();
        }

        DependencyObject IBehavior.AssociatedObject
        {
            get
            {
                return AssociatedObject;
            }
        }

        public void Attach(DependencyObject dependencyObject)
        {
            if (dependencyObject != AssociatedObject)
            {
                if (AssociatedObject != null)
                {
                    throw new InvalidOperationException();
                }

                if (Interaction.ShouldRunInDesignMode || !(bool)GetValue(DesignerProperties.IsInDesignModeProperty))
                {
                    WritePreamble();
                    associatedObject = dependencyObject;
                    WritePostscript();
                }
                OnAttached();
            }
        }
        protected abstract void OnAttached();

        public void Detach()
        {
            OnDetaching();
            WritePreamble();
            associatedObject = null;
            WritePostscript();
        }
        protected abstract void OnDetaching();

        internal abstract void ItemAdded(T item);

        internal abstract void ItemRemoved(T item);

        [Conditional("DEBUG")]
        private void VerifySnapshotIntegrity()
        {
            bool isValid = Count == snapshot.Count;
            if (isValid)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (this[i] != snapshot[i])
                    {
                        isValid = false;
                        break;
                    }
                }
            }
            Debug.Assert(isValid, "ReferentialCollection integrity has been compromised.");
        }

        private void VerifyAdd(T item)
        {
            if (snapshot.Contains(item))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.DuplicateItemInCollectionExceptionMessage, typeof(T).Name, GetType().Name));
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (T item in e.NewItems)
                    {
                        try
                        {
                            VerifyAdd(item);
                            ItemAdded(item);
                        }
                        finally
                        {
                            snapshot.Insert(IndexOf(item), item);
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    foreach (T item in e.OldItems)
                    {
                        ItemRemoved(item);
                        snapshot.Remove(item);
                    }
                    foreach (T item in e.NewItems)
                    {
                        try
                        {
                            VerifyAdd(item);
                            ItemAdded(item);
                        }
                        finally
                        {
                            snapshot.Insert(IndexOf(item), item);
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (T item in e.OldItems)
                    {
                        ItemRemoved(item);
                        snapshot.Remove(item);
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    foreach (T item in snapshot)
                    {
                        ItemRemoved(item);
                    }
                    snapshot = new Collection<T>();
                    foreach (T item in this)
                    {
                        VerifyAdd(item);
                        ItemAdded(item);
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                default:
                    Debug.Fail("Unsupported collection operation attempted.");
                    break;
            }
#if DEBUG
            VerifySnapshotIntegrity();
#endif
        }

    }
}
