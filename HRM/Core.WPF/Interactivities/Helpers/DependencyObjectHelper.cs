using System.Windows;
using System.Windows.Media;

namespace Core.WPF.Interactivities
{
    public static class DependencyObjectHelper
    {
        public static IEnumerable<DependencyObject> GetSelfAndAncestors(this DependencyObject dependencyObject)
        {
            // Walk up the visual tree looking for the element.
            while (dependencyObject != null)
            {
                yield return dependencyObject;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }
        }
    }
}
