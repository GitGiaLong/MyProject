using System.Windows.Media.Animation;

namespace Core.Libraries.WPF.Interactivities.Medias
{
    public abstract class StoryboardAction : TriggerAction<DependencyObject>
    {
        public Storyboard Storyboard
        {
            get { return (Storyboard)this.GetValue(StoryboardProperty); }
            set { this.SetValue(StoryboardProperty, value); }
        }
        public static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register(nameof(Storyboard), typeof(Storyboard), typeof(StoryboardAction),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnStoryboardChanged)));

        private static void OnStoryboardChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            StoryboardAction storyboardAction = sender as StoryboardAction;
            if (storyboardAction != null)
            {
                storyboardAction.OnStoryboardChanged(args);
            }
        }

        protected virtual void OnStoryboardChanged(DependencyPropertyChangedEventArgs args) { }
    }
}
