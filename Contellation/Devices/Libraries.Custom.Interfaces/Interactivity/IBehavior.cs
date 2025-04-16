namespace Libraries.Custom.Interfaces
{
    public interface IBehavior
    {
        DependencyObject AssociatedObject { get; }

        void Attach(DependencyObject dependencyObject);

        void Detach();
    }
}
