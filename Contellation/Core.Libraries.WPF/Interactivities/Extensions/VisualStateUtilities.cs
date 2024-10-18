using System.Collections;
using System.Windows.Controls;

namespace Core.Libraries.WPF.Interactivities
{
    public static class VisualStateUtilities
    {
        public static bool GoToState(FrameworkElement element, string stateName, bool useTransitions)
        {
            bool success = false;
            if (!string.IsNullOrEmpty(stateName))
            {
                Control targetControl = element as Control;
                if (targetControl != null)
                {
                    targetControl.ApplyTemplate();
                    success = VisualStateManager.GoToState(targetControl, stateName, useTransitions);
                }
                else
                {
                    success = VisualStateManager.GoToElementState(element, stateName, useTransitions);
                }
            }

            return success;
        }

        public static IList GetVisualStateGroups(FrameworkElement targetObject)
        {
            IList visualStateGroups = new List<VisualStateGroup>();

            if (targetObject != null)
            {
                visualStateGroups = VisualStateManager.GetVisualStateGroups(targetObject);

                if (visualStateGroups.Count == 0)
                {
                    int childrenCount = VisualTreeHelper.GetChildrenCount(targetObject);
                    if (childrenCount > 0)
                    {
                        FrameworkElement childElement = VisualTreeHelper.GetChild(targetObject, 0) as FrameworkElement;
                        visualStateGroups = VisualStateManager.GetVisualStateGroups(childElement);
                    }
                }

                // WPF puts UserControl content in a template, so it won't be the direct visual child. However,
                // the Content element is where the VSGs are expected to be located, so check there.
                if (visualStateGroups.Count == 0)
                {
                    UserControl userControl = targetObject as UserControl;
                    if (userControl != null)
                    {
                        FrameworkElement contentElement = userControl.Content as FrameworkElement;
                        if (contentElement != null)
                        {
                            visualStateGroups = VisualStateManager.GetVisualStateGroups(contentElement);
                        }
                    }
                }
            }

            return visualStateGroups;
        }

        public static bool TryFindNearestStatefulControl(FrameworkElement contextElement, out FrameworkElement resolvedControl)
        {
            FrameworkElement frameworkElement = contextElement;

            if (frameworkElement == null)
            {
                /// TODO: should we throw an exception here? Tracked as spec issue 82282.
                resolvedControl = null;
                return false;
            }

            /// Try to find an element which is the immediate child of a UserControl, ControlTemplate or other such "boundary" element
            FrameworkElement parent = frameworkElement.Parent as FrameworkElement;
            bool succesfullyResolved = true;

            /// bubble up looking for a place to stop
            while (!HasVisualStateGroupsDefined(frameworkElement) && ShouldContinueTreeWalk(parent))
            {
                frameworkElement = parent;
                parent = parent.Parent as FrameworkElement;
            }

            if (HasVisualStateGroupsDefined(frameworkElement))
            {
                if (frameworkElement.TemplatedParent != null && frameworkElement.TemplatedParent is Control)
                {
                    /// We didn't need to walk the tree to get this because TemplatedParent is set for all elements in the 
                    /// template.  However, it maintains consistency in our error checking to do it this way.
                    frameworkElement = frameworkElement.TemplatedParent as FrameworkElement;
                }
                else if (parent != null && parent is UserControl)
                {
                    /// if our parent is a UserControl, then use that
                    frameworkElement = parent;
                }
            }
            else
            {
                succesfullyResolved = false;
            }

            resolvedControl = frameworkElement;
            return succesfullyResolved;
        }
        private static bool HasVisualStateGroupsDefined(FrameworkElement frameworkElement)
        {
            return frameworkElement != null && VisualStateManager.GetVisualStateGroups(frameworkElement).Count != 0;
        }

        internal static FrameworkElement FindNearestStatefulControl(FrameworkElement contextElement)
        {
            FrameworkElement resolvedControl = null;
            TryFindNearestStatefulControl(contextElement, out resolvedControl);
            return resolvedControl;
        }

        private static bool ShouldContinueTreeWalk(FrameworkElement element)
        {
            if (element == null)
            {
                /// stop if we can't go any further
                return false;
            }
            else if (element is UserControl)
            {
                /// stop if parent is a UserControl
                return false;
            }
            else if (element.Parent == null)
            {
                /// stop if parent's parent is null AND parent isn't the template root of a ControlTemplate or DataTemplate
                FrameworkElement templatedParent = FindTemplatedParent(element);
                if (templatedParent == null || !(templatedParent is Control) && !(templatedParent is ContentPresenter))
                {
                    return false;
                }
            }
            return true;
        }

        private static FrameworkElement FindTemplatedParent(FrameworkElement parent)
        {
            return parent.TemplatedParent as FrameworkElement;
        }
    }
}
