﻿using Core.Libraries.WPF.Extensions;
using System.Windows.Controls;

namespace Core.Libraries.WPF.Controls.Datetimes
{

    [TemplatePart(Name = ElementButtonConfirm, Type = typeof(Button))]
    public abstract class ClockBase : Control
    {
        protected const string ElementButtonConfirm = "PART_ButtonConfirm";

        protected Button ButtonConfirm;

        protected bool AppliedTemplate;

        public event Action Confirmed;

        public event EventHandler<FunctionEventArgs<DateTime>> DisplayTimeChanged;

        public event EventHandler<FunctionEventArgs<DateTime?>> SelectedTimeChanged
        {
            add => AddHandler(SelectedTimeChangedEvent, value);
            remove => RemoveHandler(SelectedTimeChangedEvent, value);
        }
        public static readonly RoutedEvent SelectedTimeChangedEvent = EventManager.RegisterRoutedEvent(nameof(SelectedTimeChanged), RoutingStrategy.Direct,
            typeof(EventHandler<FunctionEventArgs<DateTime?>>), typeof(ClockBase));

        public string TimeFormat
        {
            get { return (string)GetValue(TimeFormatProperty); }
            set { SetValue(TimeFormatProperty, value); }
        }
        public static readonly DependencyProperty TimeFormatProperty = DependencyProperty.Register(nameof(TimeFormat), typeof(string), 
            typeof(ClockBase), new PropertyMetadata("HH:mm:ss"));

        public DateTime? SelectedTime
        {
            get { return (DateTime?)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }
        public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register(nameof(SelectedTime), typeof(DateTime?), 
            typeof(ClockBase), new PropertyMetadata(default(DateTime?), OnSelectedTimeChanged));

        private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (ClockBase)d;
            var v = (DateTime?)e.NewValue;
            ctl.DisplayTime = v ?? DateTime.Now;
            ctl.OnSelectedTimeChanged(new FunctionEventArgs<DateTime?>(SelectedTimeChangedEvent, ctl) { Info = v });
        }

        public DateTime DisplayTime
        {
            get { return (DateTime)GetValue(DisplayTimeProperty); }
            set { SetValue(DisplayTimeProperty, value); }
        }
        public static readonly DependencyProperty DisplayTimeProperty = DependencyProperty.Register(nameof(DisplayTime), typeof(DateTime), 
            typeof(ClockBase), new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDisplayTimeChanged));

        private static void OnDisplayTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (ClockBase)d;
            var v = (DateTime)e.NewValue;
            ctl.Update(v);
            ctl.OnDisplayTimeChanged(new FunctionEventArgs<DateTime>(v));
        }

        internal bool ShowConfirmButton
        {
            get { return (bool)GetValue(ShowConfirmButtonProperty); }
            set { SetValue(ShowConfirmButtonProperty, value); }
        }
        internal static readonly DependencyProperty ShowConfirmButtonProperty = DependencyProperty.Register(nameof(ShowConfirmButton), typeof(bool), 
            typeof(ClockBase), new PropertyMetadata(false));

        protected virtual void OnSelectedTimeChanged(FunctionEventArgs<DateTime?> e) => RaiseEvent(e);

        protected virtual void OnDisplayTimeChanged(FunctionEventArgs<DateTime> e)
        {
            var handler = DisplayTimeChanged;
            handler?.Invoke(this, e);
        }

        protected void ButtonConfirm_OnClick(object sender, RoutedEventArgs e)
        {
            SelectedTime = DisplayTime;
            Confirmed?.Invoke();
        }

        internal abstract void Update(DateTime time);

        protected void Clock_SelectedTimeChanged(object sender, FunctionEventArgs<DateTime?> e) => SelectedTime = e.Info;

        public virtual void OnClockClosed() { }

        public virtual void OnClockOpened() { }
    }
}
