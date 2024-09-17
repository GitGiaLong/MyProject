using Core.WPF.Icon.Enums;
using Core.WPF.Icon.Font.IconBlocks;
using Core.WPF.Icon.Font.Interfaces;
using Core.WPF.Icon.Helpers;

namespace Core.WPF.Icon.Font
{
    public class IconBlock : IconBlockBase<IconChar>, IHaveIconFont
    {
        public IconBlock() : base(IconHelper.FontFor(IconChar.Star, IconFont.Auto)) { }
        //public IconBlock() : base(IconChar.Abacus.WpfFontFor())
        //{
        //}

        //protected override FontFamily FontFor(IconChar icon)
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
            get => (IconFont)GetValue(IconFontProperty);
            set => SetValue(IconFontProperty, value);
        }

        protected override FontFamily FontFor(IconChar icon) { return IconHelper.FontFor(icon, IconFont); }
    }
}
