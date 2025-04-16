using Libraries.Custom.Enums.Icon;
using Libraries.Custom.Helpers.Icon;
using Libraries.Custom.Icons.Font.Blocks;
using Libraries.Custom.Interfaces.Icon;

namespace Libraries.Custom.Icons
{
    public class IconBlock : IconBlockBase<IconType>, IIconFont
    {
        public IconBlock() : base(IconHelper.FontFor(IconType.Star, IconFont.Auto)) { }
        //public IconBlock() : base(IconType.Abacus.WpfFontFor())
        //{
        //}

        //protected override FontFamily FontFor(IconType icon)
        //{
        //    return icon.WpfFontFor(IconFont);
        //}

        public static readonly DependencyProperty IconFontProperty = DependencyProperty.Register(nameof(IconFont), typeof(IconFont), typeof(IconBlock),
            new PropertyMetadata(IconFont.Auto, OnIconFontPropertyChanged));

        private static void OnIconFontPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is IconBlock iconBlock)) return;
            iconBlock.SetValue(FontFamilyProperty, iconBlock.FontFor(iconBlock.Icon));
        }

        public IconFont IconFont
        {
            get { return (IconFont)GetValue(IconFontProperty); }
            set { SetValue(IconFontProperty, value); }
        }

        protected override FontFamily FontFor(IconType icon) { return IconHelper.FontFor(icon, IconFont); }
    }
}
