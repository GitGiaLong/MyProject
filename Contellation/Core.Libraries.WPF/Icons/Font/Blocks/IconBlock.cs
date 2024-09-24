using Core.Entities.Icons;
using Core.Libraries.WPF.Icons.Font.Blocks;
using Core.Libraries.WPF.Icons.Helpers;
using Core.Libraries.WPF.Icons.Interfaces;

namespace Core.Libraries.WPF.Icons.Font
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
