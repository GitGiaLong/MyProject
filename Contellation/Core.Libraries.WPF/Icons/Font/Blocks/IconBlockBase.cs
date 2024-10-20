﻿using System.ComponentModel;
using System.Windows.Controls;

namespace Core.Libraries.WPF.Icons.Font.Blocks
{

    public abstract class IconBlockBase<TEnum> : TextBlock where TEnum : struct, IConvertible, IComparable, IFormattable
    {

        private readonly FontFamily _fontFamily;
        protected virtual FontFamily FontFor(TEnum icon) { return _fontFamily; }

        public TEnum Icon
        {
            get { return (TEnum)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(TEnum), typeof(IconBlockBase<TEnum>),
            new PropertyMetadata(default(TEnum), OnIconPropertyChanged));
        private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not IconBlockBase<TEnum> iconBlock) return;
#if NET40
        iconBlock.SetValue(TextOptions.TextRenderingModeProperty, TextRenderingMode.ClearType);
#endif
            iconBlock.SetValue(FontFamilyProperty, iconBlock.FontFor(iconBlock.Icon));
            iconBlock.SetValue(TextAlignmentProperty, TextAlignment.Center);
            iconBlock.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
            iconBlock.SetValue(TextProperty, char.ConvertFromUtf32((int)e.NewValue));
        }

        private void OnTextValueChanged(object sender, EventArgs e)
        {
            var str = Text;
            if (string.IsNullOrEmpty(str) || str.Length > 2 || !Enum.IsDefined(typeof(TEnum), char.ConvertToUtf32(str, 0))) { throw new FormatException(); }
        }


        protected IconBlockBase(FontFamily fontFamily = null)
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("TEnum must be an enum.");
            _fontFamily = fontFamily ?? throw new ArgumentNullException(nameof(fontFamily));

            VerticalAlignment = VerticalAlignment.Center;
            TextAlignment = TextAlignment.Center;

            EventHandler handler = OnTextValueChanged;
            var descriptor = DependencyPropertyDescriptor.FromProperty(TextProperty, typeof(IconBlock));
            descriptor.AddValueChanged(this, handler);
            /// ReSharper disable once VirtualMemberCallInConstructor
            var font = FontFor(Icon);
            if (font != null) FontFamily = font;

            /// remove handler otherwise we get a memory leak
            Unloaded += (s, e) => { descriptor.RemoveValueChanged(this, handler); };
        }


    }
}
