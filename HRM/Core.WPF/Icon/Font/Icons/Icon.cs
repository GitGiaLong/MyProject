using Core.WPF.Icon.Enums;
using Core.WPF.Icon.Font.Icons;
using Core.WPF.Icon.Font.Interfaces;

namespace Core.WPF.Icon.Font
{
    public class Icon : IconBase<IconBlock, IconChar>, IHaveIconFont
    {
        public Icon(IconChar icon) : base(icon) { }

        public IconFont IconFont
        {
            get => IconBlock.IconFont;
            set => IconBlock.IconFont = value;
        }
    }
}
