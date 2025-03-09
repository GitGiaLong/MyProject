using Core.Libraries.WPF.Controls.Datetimes.Scheduletimelines;
using Core.Libraries.WPF.Controls.Datetimes.Scheduletimelines.Model;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Core.Libraries.WPF.Controls
{
    [TemplatePart(Name = ElementGridTimeline, Type = typeof(Grid))]
    [TemplatePart(Name = ElementGridMainGrid, Type = typeof(Grid))]
    [TemplatePart(Name = ElementScrollViewerMainData, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = ElementStackPanelThreads, Type = typeof(StackPanel))]
    [TemplatePart(Name = ElementStackPanelMainData, Type = typeof(StackPanel))]
    [TemplatePart(Name = ElementPopupInfo, Type = typeof(Popup))]
    [TemplatePart(Name = ElementCcInfo, Type = typeof(ContentControl))]
    public class Scheduletimeline : Control
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Grid? grid_Timeline;
        private Grid? grid_MainGrid;
        private ScrollViewer? scrollViewer_MainData;
        private StackPanel? stackPanel_Threads;
        private StackPanel? stackPanel_MainData;
        private Popup? popup_info;
        private ContentControl? cc_info;

        private const string ElementGridTimeline = "grid_Timeline";
        private const string ElementGridMainGrid = "grid_MainGrid";
        private const string ElementScrollViewerMainData = "scrollViewer_MainData";
        private const string ElementStackPanelThreads = "stackPanel_Threads";
        private const string ElementStackPanelMainData = "stackPanel_MainData";
        private const string ElementPopupInfo = "popup_info";
        private const string ElementCcInfo = "cc_info";

        private Point initMousePoint;
        private DateTime initCaptureLeftEdge;
        private DateTime initCaptureRightEdge;
        private TimeSpan initCaptureScalePx;
        private Line NowMarker1;
        private Line NowMarker2;
        private CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en");

        public bool IsOnManipulate { get; private set; }
        public bool IsNeedSidePanel => Data?.IsNeedSidePanel ?? true;

        public TimelinerData Data
        {
            get { return (TimelinerData)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data), typeof(TimelinerData),
                typeof(Scheduletimeline), new PropertyMetadata(null, new PropertyChangedCallback(DataPropertyChangedCallback)));
        private static void DataPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Scheduletimeline)
            {
                Scheduletimeline input = (Scheduletimeline)d;
                if (e.NewValue != null && e.NewValue is TimelinerData)
                {
                    input.PropertyChanged?.Invoke(input, new PropertyChangedEventArgs(nameof(IsNeedSidePanel)));
                }
                input.RedrawGrid();
            }
        }

        public DateTime Now
        {
            get { return (DateTime)GetValue(NowProperty); }
            set { SetValue(NowProperty, value); }
        }
        public static readonly DependencyProperty NowProperty = DependencyProperty.Register(nameof(Now), typeof(DateTime),
                typeof(Scheduletimeline), new PropertyMetadata(DateTime.Now, new PropertyChangedCallback(NowPropertyChangedCallback)));
        private static void NowPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Scheduletimeline)
            {
                Scheduletimeline input = (Scheduletimeline)d;
                if (e.NewValue != null && e.NewValue is DateTime)
                {
                    if (input.TrackNow && e.OldValue != null && e.OldValue != default && e.NewValue != default)
                    {
                        input.LeftEdge += (DateTime)e.NewValue - (DateTime)e.OldValue;
                        input.RightEdge += (DateTime)e.NewValue - (DateTime)e.OldValue;
                        input.RedrawGrid();
                    }
                }
                input.RedrawNowMarker();
            }
        }

        public DateTime LeftEdge
        {
            get { return (DateTime)GetValue(LeftEdgeProperty); }
            set { SetValue(LeftEdgeProperty, value); }
        }
        public static readonly DependencyProperty LeftEdgeProperty = DependencyProperty.Register(nameof(LeftEdge), typeof(DateTime),
                typeof(Scheduletimeline), new PropertyMetadata(DateTime.Now - TimeSpan.FromMinutes(10), new PropertyChangedCallback(EdgePropertyChangedCallback)));

        public DateTime RightEdge
        {
            get { return (DateTime)GetValue(RightEdgeProperty); }
            set { SetValue(RightEdgeProperty, value); }
        }
        public static readonly DependencyProperty RightEdgeProperty = DependencyProperty.Register(nameof(RightEdge), typeof(DateTime),
                typeof(Scheduletimeline), new PropertyMetadata(DateTime.Now + TimeSpan.FromMinutes(10), new PropertyChangedCallback(EdgePropertyChangedCallback)));
        private static void EdgePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Scheduletimeline)
            {
                Scheduletimeline input = (Scheduletimeline)d;
                if (e.NewValue != null && e.NewValue is DateTime)
                {

                }
                input.RedrawGrid();
            }
        }

        public bool TrackNow
        {
            get { return (bool)GetValue(TrackNowProperty); }
            set { SetValue(TrackNowProperty, value); }
        }
        public static readonly DependencyProperty TrackNowProperty = DependencyProperty.Register(nameof(TrackNow), typeof(bool),
            typeof(Scheduletimeline), new PropertyMetadata(false));

        public DataTemplate DataTemplatePopup
        {
            get { return (DataTemplate)GetValue(DataTemplatePopupProperty); }
            set { SetValue(DataTemplatePopupProperty, value); }
        }
        public static readonly DependencyProperty DataTemplatePopupProperty = DependencyProperty.Register(nameof(DataTemplatePopup),
            typeof(DataTemplate), typeof(Scheduletimeline), new PropertyMetadata(null));

        public Scheduletimeline()
        {
            Loaded += OnLoaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            grid_Timeline = GetTemplateChild(ElementGridTimeline) is Grid Timeline ? Timeline : new Grid();
            grid_MainGrid = GetTemplateChild(ElementGridMainGrid) is Grid MainGrid ? MainGrid : new Grid();
            scrollViewer_MainData = GetTemplateChild(ElementScrollViewerMainData) is ScrollViewer MainData ? MainData : new ScrollViewer();
            stackPanel_Threads = GetTemplateChild(ElementStackPanelThreads) is StackPanel Threads ? Threads : new StackPanel();
            stackPanel_MainData = GetTemplateChild(ElementStackPanelMainData) is StackPanel _MainData ? _MainData : new StackPanel();
            popup_info = GetTemplateChild(ElementPopupInfo) is Popup popupinfo ? popupinfo : new Popup();
            cc_info = GetTemplateChild(ElementCcInfo) is ContentControl ccinfo ? ccinfo : new ContentControl();
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            if (e.LeftButton == MouseButtonState.Released)
            {
                IsOnManipulate = false;
                //popup_info.IsOpen = false;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsOnManipulate)));
                Mouse.Capture(null);
                e.Handled = true;
            }
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            if (IsOnManipulate && e.LeftButton == MouseButtonState.Pressed && !popup_info.IsOpen)
            {
                double deltapx = initMousePoint.X - e.GetPosition(this as IInputElement).X;
                //double deltapx = initMousePoint.X - e.GetPosition(this).X;
                LeftEdge = initCaptureLeftEdge + TimeSpan.FromMilliseconds(deltapx * initCaptureScalePx.TotalMilliseconds);
                RightEdge = initCaptureRightEdge + TimeSpan.FromMilliseconds(deltapx * initCaptureScalePx.TotalMilliseconds);
                RedrawGrid();
                e.Handled = true;
            }
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {

            }
            else
            {
                var speed = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl) ? 0.2 : 0.05;
                var xSize = grid_Timeline.ActualWidth;
                var posx = e.GetPosition(this as IInputElement).X;
                //var posx = e.GetPosition(this).X;
                var weight = 0.5;
                if (posx >= 0 && posx < xSize)
                {
                    weight = posx / xSize;
                }

                var span = RightEdge - LeftEdge;
                if (e.Delta < 0)
                {
                    LeftEdge -= TimeSpan.FromMilliseconds(span.TotalMilliseconds * speed * weight);
                    RightEdge += TimeSpan.FromMilliseconds(span.TotalMilliseconds * speed * (1 - weight));
                }
                else
                {
                    LeftEdge += TimeSpan.FromMilliseconds(span.TotalMilliseconds * speed * weight);
                    RightEdge -= TimeSpan.FromMilliseconds(span.TotalMilliseconds * speed * (1 - weight));
                }
            }
            RedrawGrid();
            base.OnPreviewMouseWheel(e);
            //e.Handled = true;
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            RedrawGrid();
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed && Mouse.Capture(this as IInputElement) && !popup_info.IsOpen)
            {
                initMousePoint = e.GetPosition(this as IInputElement);
                //initMousePoint = e.GetPosition(this);
                initCaptureLeftEdge = LeftEdge;
                initCaptureRightEdge = RightEdge;
                initCaptureScalePx = TimeSpan.FromMilliseconds((RightEdge - LeftEdge).TotalMilliseconds / grid_Timeline.ActualWidth);
                IsOnManipulate = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsOnManipulate)));
                //e.Handled = true;

            }
            if (popup_info.IsOpen)
            {
                Mouse.Capture(null);
                popup_info.IsOpen = false;
                IsOnManipulate = false;
                e.Handled = true;
            }
            else base.OnPreviewMouseDown(e);
        }

        public void RedrawNowMarker()
        {
            if (grid_Timeline == null || grid_MainGrid == null) return;
            var xSize = grid_Timeline.ActualWidth;
            var span = RightEdge - LeftEdge;
            var nowOffset = (Now - LeftEdge).ToPixcel(span, xSize);
            if (grid_Timeline.Children.Contains(NowMarker1))
            {
                NowMarker1.X1 = nowOffset;
                NowMarker1.X2 = nowOffset;
            }
            else
            {
                NowMarker1 = new Line
                {
                    X1 = nowOffset,
                    X2 = nowOffset,
                    Y1 = 0,
                    Y2 = grid_Timeline.ActualHeight,
                    Stroke = Brushes.Red,
                    StrokeThickness = 1,
                };
                grid_Timeline.Children.Add(NowMarker1);
            }
            if (grid_MainGrid.Children.Contains(NowMarker2))
            {
                NowMarker2.X1 = nowOffset;
                NowMarker2.X2 = nowOffset;
            }
            else
            {
                NowMarker2 = new Line
                {
                    X1 = nowOffset,
                    X2 = nowOffset,
                    Y1 = 0,
                    Y2 = grid_MainGrid.ActualHeight,
                    Stroke = Brushes.Red,
                    StrokeThickness = 1,
                };
                grid_MainGrid.Children.Add(NowMarker2);
            }
        }

        public void RedrawGrid()
        {
            if (grid_Timeline == null || !IsLoaded) return;
            var xSize = grid_Timeline.ActualWidth;
            grid_Timeline.Children.Clear();
            grid_MainGrid.Children.Clear();

            TextBlock test = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Text = "yyyy.MM.dd HH:MM:SS",
                FontSize = FontSize
            };
            test.Measure(new Size(500, 100));
            double widthTextMajor = test.DesiredSize.Width + 8;
            var span = RightEdge - LeftEdge;
            var majorModeWidthNormal = span.NearSpanMode().ModeToSpan(LeftEdge).ToPixcel(span, xSize);
            var majorModeWidthLess = (span.NearSpanMode() - 1).ModeToSpan(LeftEdge).ToPixcel(span, xSize);

            var majorMode = majorModeWidthLess >= widthTextMajor ? span.NearSpanMode() - 1 : majorModeWidthNormal >= widthTextMajor ? span.NearSpanMode() : span.NearSpanMode() + 1;
            var minorMode = majorMode - 1;

            var leftEdgeMajor = LeftEdge.GetMajorLeftEdge(majorMode);
            var currentMajor = leftEdgeMajor;

            while (currentMajor <= RightEdge)
            {
                var majorSpan = majorMode.ModeToSpan(currentMajor);
                var majorWithPx = majorSpan.ToPixcel(span, xSize);
                var majorOffset = currentMajor - leftEdgeMajor - (LeftEdge - leftEdgeMajor);
                var majorOffsetPx = majorOffset.ToPixcel(span, xSize);

                var border = new Border
                {
                    Width = majorWithPx,
                    Margin = new Thickness(majorOffsetPx, 0, 0, 0),
                    BorderBrush = SystemColors.ActiveBorderBrush,
                    BorderThickness = new Thickness(1, 0, 1, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Child = new StackPanel
                    {
                        Children =
                        {
                            new TextBlock
                            {
                                Text = currentMajor.ToFullString(majorMode),
                                HorizontalAlignment = (majorOffsetPx < 0 && !(majorOffsetPx + majorWithPx > xSize))
                                    ? HorizontalAlignment.Right : (majorOffsetPx + majorWithPx > xSize && !(majorOffsetPx < 0))
                                    ? HorizontalAlignment.Left : HorizontalAlignment.Center,
                                Margin = new Thickness(majorOffsetPx < 0 && majorOffsetPx + majorWithPx > xSize ? (LeftEdge - currentMajor).ToPixcel(span, xSize) : 2, 2, majorOffsetPx < 0 && majorOffsetPx + majorWithPx > xSize ? (currentMajor + majorSpan - RightEdge).ToPixcel(span, xSize) : 2, 2)
                            },
                            new Grid
                            {
                                ClipToBounds = true,
                            }
                        }
                    }
                };

                test = new TextBlock
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Text = "77",
                    FontSize = FontSize
                };
                test.Measure(new Size(100, 100));
                double widthTextMinor = test.DesiredSize.Width + 10;

                var currentMinor = currentMajor;
                var minorSpan = minorMode.ModeToSpan(currentMinor);
                var minorWithPx = minorSpan.ToPixcel(span, xSize);
                var reduceFactor = minorWithPx > 0 && widthTextMinor / minorWithPx > 1 ? (Math.Round(widthTextMinor / minorWithPx / 2, MidpointRounding.AwayFromZero) * 2) : 1;
                minorSpan = TimeSpan.FromMilliseconds(minorSpan.TotalMilliseconds * reduceFactor);
                while (currentMinor < currentMajor + majorSpan - minorSpan)
                {
                    currentMinor += minorSpan;
                    minorSpan = minorMode.ModeToSpan(currentMinor);
                    minorWithPx = minorSpan.ToPixcel(span, xSize);
                    reduceFactor = minorWithPx > 0 && widthTextMinor / minorWithPx > 1 ? (Math.Round(widthTextMinor / minorWithPx / 2, MidpointRounding.AwayFromZero) * 2) : 1;
                    minorSpan = TimeSpan.FromMilliseconds(minorSpan.TotalMilliseconds * reduceFactor);
                    minorWithPx *= reduceFactor;
                    var minorOffset = currentMinor - currentMajor;
                    var minorOffsetPx = minorOffset.ToPixcel(span, xSize);
                    ((border.Child as StackPanel).Children[1] as Grid).Children.Add(new TextBlock
                    {
                        Text = currentMinor.ToLastString(minorMode),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        TextAlignment = System.Windows.TextAlignment.Center,
                        Width = minorWithPx,
                        Margin = new Thickness(minorOffsetPx - minorWithPx / 2, 0, 2, 0)
                    });
                    grid_MainGrid.Children.Add(new Line
                    {
                        X1 = majorOffsetPx + minorOffsetPx,
                        X2 = majorOffsetPx + minorOffsetPx,
                        Y1 = 0,
                        Y2 = grid_MainGrid.ActualHeight,
                        Stroke = SystemColors.ActiveBorderBrush,
                        StrokeThickness = 1,
                    });
                }

                grid_Timeline.Children.Add(border);
                grid_MainGrid.Children.Add(new Line
                {
                    X1 = majorOffsetPx,
                    X2 = majorOffsetPx,
                    Y1 = 0,
                    Y2 = grid_MainGrid.ActualHeight,
                    Stroke = SystemColors.ActiveBorderBrush,
                    StrokeThickness = 1,
                });
                currentMajor += majorSpan;
            }
            RedrawNowMarker();
            RedrawData();
        }

        public void RedrawData()
        {
            if (grid_Timeline == null) return;
            var xSize = grid_Timeline.ActualWidth;
            stackPanel_Threads.Children.Clear();
            stackPanel_MainData.Children.Clear();
            if (Data == null || Data.Items == null || !(Data.Items?.Count() > 0)) return;

            var span = RightEdge - LeftEdge;

            TextBlock test = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Text = "Test",
                FontSize = FontSize
            };
            test.Measure(new Size(100, 100));
            double heightText = test.DesiredSize.Height;

            foreach (var item in Data.Items)
            {
                var isTextUp = (item.Jobs?.Any(x => !string.IsNullOrEmpty(x.TextUp)) ?? false);
                var isTextDown = (item.Jobs?.Any(x => !string.IsNullOrEmpty(x.TextDown)) ?? false);
                var heightItem = heightText + 4 + (isTextUp ? heightText + 2 : 0);
                stackPanel_Threads.Children.Add(new Border
                {
                    BorderBrush = SystemColors.ActiveBorderBrush,
                    BorderThickness = new Thickness(0, 0, 0, 1),
                    Child = new TextBlock
                    {
                        Text = item.Name,
                        Margin = new Thickness(2),
                        Foreground = Brushes.White,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        TextAlignment = System.Windows.TextAlignment.Right,
                        //FlowDirection = FlowDirection.RightToLeft,
                    },
                    Height = heightItem,
                    ToolTip = item.Description
                });
                var bd = new Border
                {
                    BorderBrush = SystemColors.ActiveBorderBrush,
                    BorderThickness = new Thickness(0, 0, 0, 1),
                    Child = new Grid(),
                    Height = heightItem,
                };
                double lastItemLeftEdge = double.NaN;
                foreach (var job in item.Jobs.OrderByDescending(x => x.Begin))
                {
                    if (job.End > LeftEdge && job.Begin < RightEdge)
                    {
                        double width = (job.End - job.Begin).ToPixcel(span, xSize);
                        var gr = new Grid
                        {
                            Margin = new Thickness((job.Begin - LeftEdge).ToPixcel(span, xSize), 0, 0, 0),
                            Background = Brushes.Transparent,
                            HorizontalAlignment = HorizontalAlignment.Left,
                        };
                        gr.PreviewMouseDown += (s, e) =>
                        {
                            cc_info.ContentTemplate = DataTemplatePopup ?? (DataTemplate)this.Resources["defaultTemplate"];
                            cc_info.Content = job;
                            cc_info.Foreground = Brushes.Black;
                            popup_info.IsOpen = true;
                            Mouse.Capture(this);
                            e.Handled = true;
                        };

                        if (width > 6)
                        {
                            var br = new Border
                            {
                                Width = (job.End - job.Begin).ToPixcel(span, xSize),
                                Margin = new Thickness(0, isTextUp ? heightText : 0, 0, isTextDown ? heightText : 0),
                                CornerRadius = new CornerRadius(4),
                                Background = !job.IsStripedColor ? job.Color.Clone()
                                    : new LinearGradientBrush
                                    {
                                        MappingMode = BrushMappingMode.Absolute,
                                        EndPoint = new Point(8, 8),
                                        SpreadMethod = GradientSpreadMethod.Repeat,
                                        GradientStops =
                                        {
                                            new GradientStop
                                            {
                                                Offset = 0,
                                                Color = ((SolidColorBrush)job.Color).Color
                                            },
                                            new GradientStop
                                            {
                                                Offset = 0.5,
                                                Color = ((SolidColorBrush)job.Color).Color
                                            },
                                            new GradientStop
                                            {
                                                Offset = 0.5,
                                                Color = Colors.LightGray
                                            },
                                            new GradientStop
                                            {
                                                Offset = 1,
                                                Color = Colors.LightGray
                                            }
                                        }
                                    },
                                BorderBrush = SystemColors.ActiveBorderBrush,
                                BorderThickness = new Thickness(1),
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Center,
                                Child = new TextBlock
                                {
                                    Text = job.Name,
                                    Margin = new Thickness(1)
                                }
                            };
                            if (!string.IsNullOrEmpty(job.TextUp))
                            {
                                test = new TextBlock
                                {
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Bottom,
                                    Text = job.TextUp,
                                    FontSize = FontSize
                                };
                                test.Measure(new Size(100, 100));
                                var oversize = (job.Begin - LeftEdge).ToPixcel(span, xSize) + test.DesiredSize.Width + 8 > lastItemLeftEdge;

                                if (!oversize)
                                {
                                    var textUp = new TextBlock
                                    {
                                        Text = job.TextUp,
                                        Margin = new Thickness(0, 0, 0, heightText + (isTextDown ? heightText : 0))
                                    };
                                    gr.Children.Add(textUp);
                                }
                            }

                            if (gr.Margin.Left < 0)
                            {
                                br.Width = br.Width + gr.Margin.Left;
                                gr.Margin = new Thickness(0);
                                (br.Child as TextBlock).Text = "<-" + (br.Child as TextBlock).Text;
                            }
                            if (gr.Margin.Left + br.Width > xSize)
                            {
                                //br.Width = br.Margin.Left - br.Width;
                                (br.Child as TextBlock).Text = (br.Child as TextBlock).Text + "->";
                            }
                            lastItemLeftEdge = (job.Begin - LeftEdge).ToPixcel(span, xSize);
                            gr.Children.Add(br);
                        }
                        else
                        {
                            var height = heightItem - 4;
                            var ln = new Path
                            {
                                Data = Geometry.Parse(string.Format(cultureInfo, "M 0 {0} L3 {1} L6 {0} L3 {1} L3 3 L6 0 L3 3 L0 0 L3 3 L3 {1} Z", height, height - 3)),
                                Width = 6,
                                Height = height,
                                Stroke = job.Color.Clone(),
                                StrokeThickness = 2,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Center,
                            };
                            gr.Children.Add(ln);

                            test = new TextBlock
                            {
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Bottom,
                                Text = job.Name,
                                FontSize = FontSize
                            };
                            test.Measure(new Size(100, 100));

                            var oversize = (job.Begin - LeftEdge).ToPixcel(span, xSize) + test.DesiredSize.Width + 8 > lastItemLeftEdge;
                            if (!oversize)
                            {
                                var tx = new TextBlock
                                {
                                    Text = job.Name,
                                    Margin = new Thickness(6, isTextUp ? heightText : 0, 0, isTextDown ? heightText : 0)
                                };
                                gr.Children.Add(tx);
                            }
                            lastItemLeftEdge = (job.Begin - LeftEdge).ToPixcel(span, xSize);
                        }
                    (bd.Child as Grid).Children.Add(gr);
                    }
                }

                stackPanel_MainData.Children.Add(bd);
            }
        }
    }
}