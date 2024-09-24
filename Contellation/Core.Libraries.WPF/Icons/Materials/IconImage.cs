using Core.Entities.Icons;
using Core.Libraries.WPF.Icons.Extensions;
using Core.Libraries.WPF.Icons.Font.Images;
using Core.Libraries.WPF.Icons.Helpers;

namespace Core.Libraries.WPF.Icons.Materials
{
    public class IconImage : IconImageBase<MaterialIcons>
    {
        protected override ImageSource ImageSourceFor(MaterialIcons icon)
        {
            var size = Math.Max(IconHelper.DefaultSize, Math.Max(ActualWidth, ActualHeight));
            return MaterialDesignFont.Wpf.Value.ToImageSource(icon, Foreground, size);
        }
    }
}
