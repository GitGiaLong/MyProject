using System.Windows;

namespace Core.WPF.Interactivities
{
    public interface IBehavior
    {
        DependencyObject AssociatedObject { get; }

        void Attach(DependencyObject dependencyObject);

        void Detach();
    }
}
