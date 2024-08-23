using System.Windows.Media.Animation;

namespace Core.WPF.Interactivities.Medias
{
    public abstract class StoryboardTrigger : TriggerBase<DependencyObject>
    {
        public static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register("Storyboard", typeof(Storyboard), typeof(StoryboardTrigger),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnStoryboardChanged)));

        public Storyboard Storyboard
        {
            get { return (Storyboard)this.GetValue(StoryboardProperty); }
            set { this.SetValue(StoryboardProperty, value); }
        }

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
