﻿using Core.WPF.Controls.Gravatars;
using Core.WPF.Helpers;
using Core.WPF.Properties;
using System.Windows.Controls;

namespace Core.WPF.Controls
{
    public class Gravatar : ContentControl
    {
        public IGravatar Generator
        {
            get { return (IGravatar)GetValue(GeneratorProperty); }
            set { SetValue(GeneratorProperty, value); }
        }
        public static readonly DependencyProperty GeneratorProperty = DependencyProperty.Register(
        nameof(Generator), typeof(IGravatar), typeof(Gravatar), new PropertyMetadata(new GravatarGenerator()));

        private static void OnIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (Gravatar)d;
            if (ctl.Source != null) return;
            ctl.Content = ctl.Generator.GetGravatar((string)e.NewValue);
        }
        public string Id
        {
            get { return (string)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        public static readonly DependencyProperty IdProperty = DependencyProperty.Register(
            nameof(Id), typeof(string), typeof(Gravatar), new PropertyMetadata(default(string), OnIdChanged));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source), typeof(ImageSource), typeof(Gravatar), new PropertyMetadata(default(ImageSource), OnSourceChanged));

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (Gravatar)d;
            var v = (ImageSource)e.NewValue;

            ctl.Background = v != null ? new ImageBrush(v) { Stretch = Stretch.UniformToFill } 
            : ResourceHelper.GetResourceInternal<Brush>(ResourceToken.SecondaryRegionBrush);
        }

    }
}