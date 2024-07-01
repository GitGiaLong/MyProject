using System.Globalization;
using System.Windows;
using System.Windows.Media.Animation;

namespace Core.WPF.Interactivities
{
    public abstract class Behavior : Animatable, IBehavior
    {
        private Type associatedType;
        private DependencyObject associatedObject;

        internal event EventHandler AssociatedObjectChanged;

        protected Type AssociatedType
        {
            get
            {
                ReadPreamble();
                return associatedType;
            }
        }

        protected DependencyObject AssociatedObject
        {
            get
            {
                ReadPreamble();
                return associatedObject;
            }
        }

        internal Behavior(Type associatedType)
        {
            this.associatedType = associatedType;
        }

        DependencyObject IBehavior.AssociatedObject { get { return AssociatedObject; } }

        public void Attach(DependencyObject dependencyObject)
        {
            if (dependencyObject != AssociatedObject)
            {
                if (AssociatedObject != null)
                {
                    throw new InvalidOperationException(ExceptionStringTable.CannotHostBehaviorMultipleTimesExceptionMessage);
                }

                if (dependencyObject != null && !AssociatedType.IsAssignableFrom(dependencyObject.GetType()))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                                                                        ExceptionStringTable.TypeConstraintViolatedExceptionMessage,
                                                                        GetType().Name,
                                                                        dependencyObject.GetType().Name,
                                                                        AssociatedType.Name));
                }

                WritePreamble();
                associatedObject = dependencyObject;
                WritePostscript();
                OnAssociatedObjectChanged();

                OnAttached();
            }
        }
        protected virtual void OnAttached() { }

        public void Detach()
        {
            OnDetaching();
            WritePreamble();
            associatedObject = null;
            WritePostscript();
            OnAssociatedObjectChanged();
        }
        protected virtual void OnDetaching() { }

        protected override Freezable CreateInstanceCore()
        {
            Type classType = GetType();
            return (Freezable)Activator.CreateInstance(classType);
        }

        private void OnAssociatedObjectChanged()
        {
            if (AssociatedObjectChanged != null)
            {
                AssociatedObjectChanged(this, new EventArgs());
            }
        }

    }
}
