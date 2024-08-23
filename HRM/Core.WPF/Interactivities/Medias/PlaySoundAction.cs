using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Core.WPF.Interactivities.Medias
{
    public class PlaySoundAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(PlaySoundAction), null);
        public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register("Volume", typeof(double), typeof(PlaySoundAction), new PropertyMetadata(0.5));

        public PlaySoundAction() { }

        public Uri Source
        {
            get { return (Uri)this.GetValue(SourceProperty); }
            set { this.SetValue(SourceProperty, value); }
        }

        public double Volume
        {
            get { return (double)this.GetValue(VolumeProperty); }
            set { this.SetValue(VolumeProperty, value); }
        }

        protected virtual void SetMediaElementProperties(MediaElement mediaElement)
        {
            if (mediaElement != null)
            {
                mediaElement.Source = this.Source;
                mediaElement.Volume = this.Volume;
            }
        }

        protected override void Invoke(object parameter)
        {
            if (this.Source == null || this.AssociatedObject == null)
            {
                return;
            }

            Popup popup = new Popup();
            MediaElement mediaElement = new MediaElement();
            popup.Child = mediaElement;
            mediaElement.Visibility = Visibility.Collapsed;

            this.SetMediaElementProperties(mediaElement);

            mediaElement.MediaEnded += delegate
            {
                popup.Child = null;
                popup.IsOpen = false;
            };

            mediaElement.MediaFailed += delegate
            {
                popup.Child = null;
                popup.IsOpen = false;
            };

            popup.IsOpen = true;
        }
    }
}
