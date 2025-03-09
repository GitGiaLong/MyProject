using System.Windows.Media.Animation;

namespace Core.Libraries.WPF.Interactivities.Medias
{
    public class StoryboardCompletedTrigger : StoryboardTrigger
    {
        public StoryboardCompletedTrigger() { }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (this.Storyboard != null)
            {
                this.Storyboard.Completed -= this.Storyboard_Completed;
            }
        }

        protected override void OnStoryboardChanged(DependencyPropertyChangedEventArgs args)
        {
            Storyboard oldStoryboard = args.OldValue as Storyboard;
            Storyboard newStoryboard = args.NewValue as Storyboard;

            if (oldStoryboard != newStoryboard)
            {
                if (oldStoryboard != null)
                {
                    oldStoryboard.Completed -= this.Storyboard_Completed;
                }
                if (newStoryboard != null)
                {
                    newStoryboard.Completed += this.Storyboard_Completed;
                }
            }
        }

        private void Storyboard_Completed(object sender, EventArgs e) { this.InvokeActions(e); }
    }
}
