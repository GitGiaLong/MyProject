using System.Diagnostics;
using System.Globalization;

namespace Core.Libraries.WPF.Interactivities
{

    internal sealed class NameResolver
    {
        private string name;
        private FrameworkElement nameScopeReferenceElement;

        public event EventHandler<NameResolvedEventArgs> ResolvedElementChanged;

        public string Name
        {
            get { return name; }
            set
            {
                DependencyObject oldObject = Object;
                name = value;
                UpdateObjectFromName(oldObject);
            }
        }

        public DependencyObject Object
        {
            get
            {
                if (string.IsNullOrEmpty(Name) && HasAttempedResolve)
                {
                    return NameScopeReferenceElement;
                }
                return ResolvedObject;
            }
        }

        public FrameworkElement NameScopeReferenceElement
        {
            get { return nameScopeReferenceElement; }
            set
            {
                FrameworkElement oldHost = NameScopeReferenceElement;
                nameScopeReferenceElement = value;
                OnNameScopeReferenceElementChanged(oldHost);
            }
        }

        private FrameworkElement ActualNameScopeReferenceElement
        {
            get
            {
                if (NameScopeReferenceElement == null || !Interaction.IsElementLoaded(NameScopeReferenceElement))
                {
                    return null;
                }
                return GetActualNameScopeReference(NameScopeReferenceElement);
            }
        }

        private DependencyObject ResolvedObject { get; set; }

        private bool PendingReferenceElementLoad { get; set; }

        private bool HasAttempedResolve { get; set; }

        private void OnNameScopeReferenceElementChanged(FrameworkElement oldNameScopeReference)
        {
            if (PendingReferenceElementLoad)
            {
                oldNameScopeReference.Loaded -= new RoutedEventHandler(OnNameScopeReferenceLoaded);
                PendingReferenceElementLoad = false;
            }
            HasAttempedResolve = false;
            UpdateObjectFromName(Object);
        }

        private void UpdateObjectFromName(DependencyObject oldObject)
        {
            DependencyObject newObject = null;

            ResolvedObject = null;

            if (NameScopeReferenceElement != null)
            {
                if (!Interaction.IsElementLoaded(NameScopeReferenceElement))
                {
                    NameScopeReferenceElement.Loaded += new RoutedEventHandler(OnNameScopeReferenceLoaded);
                    PendingReferenceElementLoad = true;
                    return;
                }

                if (!string.IsNullOrEmpty(Name))
                {
                    FrameworkElement namescopeElement = ActualNameScopeReferenceElement;
                    if (namescopeElement != null)
                    {
                        newObject = namescopeElement.FindName(Name) as DependencyObject;
                    }

                    if (newObject == null)
                    {
                        Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, ExceptionStringTable.UnableToResolveTargetNameWarningMessage, Name));
                    }
                }
            }
            HasAttempedResolve = true;
            ResolvedObject = newObject;
            if (oldObject != Object)
            {
                OnObjectChanged(oldObject, Object);
            }
        }

        private void OnObjectChanged(DependencyObject oldTarget, DependencyObject newTarget)
        {
            if (ResolvedElementChanged != null)
            {
                ResolvedElementChanged(this, new NameResolvedEventArgs(oldTarget, newTarget));
            }
        }

        private FrameworkElement GetActualNameScopeReference(FrameworkElement initialReferenceElement)
        {
            Debug.Assert(Interaction.IsElementLoaded(initialReferenceElement));
            FrameworkElement nameScopeReference = initialReferenceElement;

            if (IsNameScope(initialReferenceElement))
            {
                nameScopeReference = initialReferenceElement.Parent as FrameworkElement ?? nameScopeReference;
            }
            return nameScopeReference;
        }

        private bool IsNameScope(FrameworkElement frameworkElement)
        {
            FrameworkElement parentElement = frameworkElement.Parent as FrameworkElement;
            if (parentElement != null)
            {
                object resolvedInParentScope = parentElement.FindName(Name);
                return resolvedInParentScope != null;
            }
            return false;
        }

        private void OnNameScopeReferenceLoaded(object sender, RoutedEventArgs e)
        {
            PendingReferenceElementLoad = false;
            NameScopeReferenceElement.Loaded -= new RoutedEventHandler(OnNameScopeReferenceLoaded);
            UpdateObjectFromName(Object);
        }
    }
}
