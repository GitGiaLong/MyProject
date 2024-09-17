using Core.WPF.Controls.Screenshots;
using Core.WPF.Extensions;

namespace Core.WPF.Controls
{
    public class Screenshot
    {
        public static event EventHandler<FunctionEventArgs<ImageSource>> Snapped;

        public void Start() => new ScreenshotWindow(this).Show();

        internal void OnSnapped(ImageSource source) => Snapped?.Invoke(this, new FunctionEventArgs<ImageSource>(source));
    }
}
