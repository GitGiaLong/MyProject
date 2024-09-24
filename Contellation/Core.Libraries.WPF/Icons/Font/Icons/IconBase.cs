using Core.Libraries.WPF.Icons.Font.Blocks;

namespace Core.Libraries.WPF.Icons.Font.Icons
{
    public abstract class IconBase<TIconBlock, TIcon> : MarkupExtension where TIconBlock : IconBlockBase<TIcon>, new()
        where TIcon : struct, IConvertible, IComparable, IFormattable
    {
        protected readonly TIconBlock IconBlock;

        protected IconBase(TIcon icon) { IconBlock = new TIconBlock { Icon = icon }; }

        public Brush Foreground
        {
            get { return IconBlock.Foreground; }
            set { IconBlock.Foreground = value; }
        }

        public override object ProvideValue(IServiceProvider serviceProvider) { return IconBlock; }
    }
}
