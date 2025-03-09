using System.Windows.Media.Animation;

namespace Core.Libraries.WPF.Interactivities.Medias
{
    public abstract class StoryboardTrigger : TriggerBase<DependencyObject>
    {
        public Storyboard Storyboard
        {
            get { return (Storyboard)this.GetValue(StoryboardProperty); }
            set { this.SetValue(StoryboardProperty, value); }
        }
        public static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register(nameof(Storyboard), typeof(Storyboard), typeof(StoryboardTrigger),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnStoryboardChanged)));

        private static void OnStoryboardChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            StoryboardTrigger storyboardTrigger = sender as StoryboardTrigger;
            if (storyboardTrigger != null)
            {
                storyboardTrigger.OnStoryboardChanged(args);
            }
        }

        protected virtual void OnStoryboardChanged(DependencyPropertyChangedEventArgs args) { }
    }
}
