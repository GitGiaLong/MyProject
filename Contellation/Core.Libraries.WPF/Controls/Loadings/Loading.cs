using Core.Libraries.WPF.Controls.Loadings;
using Core.Libraries.WPF.Controls.Loadings.Indicators;
using System.Windows.Controls;

namespace Core.Libraries.WPF.Controls
{
    /// <summary>
    /// A control featuring a range of loading indicating animations.
    /// </summary>
    [TemplatePart(Name = TemplateBorderName, Type = typeof(Border))]
    public class Loading : Control
    {
        /// ReSharper disable once InconsistentNaming
        protected Border PART_Border;
        internal const string TemplateBorderName = "PART_Border";

        /// <summary> Get/set the speed ratio of the animation.</summary>
        public double SpeedRatio
        {
            get { return (double)GetValue(SpeedRatioProperty); }
            set { SetValue(SpeedRatioProperty, value); }
        }
        /// <summary>Identifies the <see cref="Loading.SpeedRatio" /> dependency property.</summary>
        public static readonly DependencyProperty SpeedRatioProperty = DependencyProperty.Register(nameof(SpeedRatio), typeof(double),
            typeof(Loading), new PropertyMetadata(1d, OnSpeedRatioChanged));

        /// <summary> Get/set whether the loading is active.</summary>
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }
        /// <summary> Identifies the <see cref="Loading.IsActive" /> dependency property. </summary>
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof(IsActive), typeof(bool),
            typeof(Loading), new PropertyMetadata(true, OnIsActiveChanged));

        public LoadingMode Mode
        {
            get { return (LoadingMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(LoadingMode),
            typeof(Loading), new PropertyMetadata(default(LoadingMode)));

        static Loading() { DefaultStyleKeyProperty.OverrideMetadata(typeof(Loading), new FrameworkPropertyMetadata(typeof(Loading))); }

        private static void OnSpeedRatioChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var li = (Loading)o;

            if (li.PART_Border == null || li.IsActive == false) { return; }

            SetStoryBoardSpeedRatio(li.PART_Border, (double)e.NewValue);
        }

        private static void OnIsActiveChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var li = (Loading)o;

            if (li.PART_Border == null) { return; }

            if ((bool)e.NewValue == false)
            {
                VisualStateManager.GoToElementState(li.PART_Border, VisualStateNames.InactiveState.Name, false);
                li.PART_Border.SetCurrentValue(VisibilityProperty, Visibility.Collapsed);
            }
            else
            {
                VisualStateManager.GoToElementState(li.PART_Border, VisualStateNames.ActiveState.Name, false);

                li.PART_Border.SetCurrentValue(VisibilityProperty, Visibility.Visible);

                SetStoryBoardSpeedRatio(li.PART_Border, li.SpeedRatio);
            }
        }

        private static void SetStoryBoardSpeedRatio(FrameworkElement element, double speedRatio)
        {
            var activeStates = element.GetActiveVisualStates();
            foreach (var activeState in activeStates) activeState.Storyboard.SetSpeedRatio(element, speedRatio);
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call 
        /// <see cref="System.Windows.FrameworkElement.ApplyTemplate()"/>.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Border = (Border)GetTemplateChild(TemplateBorderName);

            if (PART_Border == null) { return; }

            VisualStateManager.GoToElementState(PART_Border, IsActive ? VisualStateNames.ActiveState.Name : VisualStateNames.InactiveState.Name, false);

            SetStoryBoardSpeedRatio(PART_Border, SpeedRatio);

            PART_Border.SetCurrentValue(VisibilityProperty, IsActive ? Visibility.Visible : Visibility.Collapsed);
        }
    }
}
