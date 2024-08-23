using Core.WPF.Interactivities.Enums;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Core.WPF.Interactivities.Layouts
{

    public sealed class FluidMoveBehavior : FluidMoveBehaviorBase
    {
        public Duration Duration
        {
            get { return (Duration)this.GetValue(DurationProperty); }
            set { this.SetValue(DurationProperty, value); }
        }
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration", typeof(Duration), typeof(FluidMoveBehavior), new PropertyMetadata(new Duration(TimeSpan.FromSeconds(1.0))));

        public TagType InitialTag
        {
            get { return (TagType)this.GetValue(InitialTagProperty); }
            set { this.SetValue(InitialTagProperty, value); }
        }

        public static readonly DependencyProperty InitialTagProperty = DependencyProperty.Register("InitialTag", typeof(TagType), typeof(FluidMoveBehavior), new PropertyMetadata(TagType.Element));

        public string InitialTagPath
        {
            get { return (string)this.GetValue(InitialTagPathProperty); }
            set { this.SetValue(InitialTagPathProperty, value); }
        }

        public static readonly DependencyProperty InitialTagPathProperty = DependencyProperty.Register("InitialTagPath", typeof(string), typeof(FluidMoveBehavior), new PropertyMetadata(String.Empty));

        private static readonly DependencyProperty initialIdentityTagProperty = DependencyProperty.RegisterAttached("InitialIdentityTag", typeof(object), typeof(FluidMoveBehavior), new PropertyMetadata(null));
        private static object GetInitialIdentityTag(DependencyObject obj) { return obj.GetValue(initialIdentityTagProperty); }
        private static void SetInitialIdentityTag(DependencyObject obj, object value) { obj.SetValue(initialIdentityTagProperty, value); }

        public bool FloatAbove
        {
            get { return (bool)this.GetValue(FloatAboveProperty); }
            set { this.SetValue(FloatAboveProperty, value); }
        }

        public static readonly DependencyProperty FloatAboveProperty = DependencyProperty.Register("FloatAbove", typeof(bool), typeof(FluidMoveBehavior), new PropertyMetadata(true));

        public IEasingFunction EaseX
        {
            get { return (IEasingFunction)this.GetValue(EaseXProperty); }
            set { this.SetValue(EaseXProperty, value); }
        }

        public static readonly DependencyProperty EaseXProperty = DependencyProperty.Register("EaseX", typeof(IEasingFunction), typeof(FluidMoveBehavior), new PropertyMetadata(null));

        public IEasingFunction EaseY
        {
            get { return (IEasingFunction)this.GetValue(EaseYProperty); }
            set { this.SetValue(EaseYProperty, value); }
        }

        public static readonly DependencyProperty EaseYProperty = DependencyProperty.Register("EaseY", typeof(IEasingFunction), typeof(FluidMoveBehavior), new PropertyMetadata(null));

        private static readonly DependencyProperty overlayProperty = DependencyProperty.RegisterAttached("Overlay", typeof(object), typeof(FluidMoveBehavior), new PropertyMetadata(null));
        private static object GetOverlay(DependencyObject obj) { return obj.GetValue(overlayProperty); }
        private static void SetOverlay(DependencyObject obj, object value) { obj.SetValue(overlayProperty, value); }

        private static readonly DependencyProperty cacheDuringOverlayProperty = DependencyProperty.RegisterAttached("CacheDuringOverlay", typeof(object), typeof(FluidMoveBehavior), new PropertyMetadata(null));
        private static object GetCacheDuringOverlay(DependencyObject obj) { return obj.GetValue(cacheDuringOverlayProperty); }
        private static void SetCacheDuringOverlay(DependencyObject obj, object value) { obj.SetValue(cacheDuringOverlayProperty, value); }

        private static readonly DependencyProperty hasTransformWrapperProperty = DependencyProperty.RegisterAttached("HasTransformWrapper", typeof(bool), typeof(FluidMoveBehavior), new PropertyMetadata(false));
        private static bool GetHasTransformWrapper(DependencyObject obj) { return (bool)obj.GetValue(hasTransformWrapperProperty); }
        private static void SetHasTransformWrapper(DependencyObject obj, bool value) { obj.SetValue(hasTransformWrapperProperty, value); }

        private static Dictionary<object, Storyboard> transitionStoryboardDictionary = new Dictionary<object, Storyboard>();

        protected override bool ShouldSkipInitialLayout
        {
            get
            {
                return base.ShouldSkipInitialLayout || (this.InitialTag == TagType.DataContext);
            }
        }

        protected override void EnsureTags(FrameworkElement child)
        {
            base.EnsureTags(child);

            if (this.InitialTag == TagType.DataContext)
            {
                object tagValue = child.ReadLocalValue(initialIdentityTagProperty);
                if (!(tagValue is BindingExpression))
                {
                    child.SetBinding(initialIdentityTagProperty, new Binding(this.InitialTagPath));
                }
            }
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Trying to keep the number of function parameters down to a minimum.")]
        internal override void UpdateLayoutTransitionCore(FrameworkElement child, FrameworkElement root, object tag, TagData newTagData)
        {
            TagData tagData;
            Rect previousRect;
            bool parentChange = false;
            bool usingBeforeLoaded = false;
            object initialTag = GetInitialIdentityTag(child);

            bool gotData = TagDictionary.TryGetValue(tag, out tagData);

            if (gotData && tagData.InitialTag != initialTag)
            {
                gotData = false;
                TagDictionary.Remove(tag);
            }
            if (!gotData)
            {
                TagData spawnData;

                if (initialTag != null && TagDictionary.TryGetValue(initialTag, out spawnData))
                {
                    previousRect = TranslateRect(spawnData.AppRect, root, newTagData.Parent);
                    parentChange = true;
                    usingBeforeLoaded = true;
                }
                else
                {
                    previousRect = Rect.Empty;
                }

                tagData = new TagData() { ParentRect = Rect.Empty, AppRect = Rect.Empty, Parent = newTagData.Parent, Child = child, Timestamp = DateTime.Now, InitialTag = initialTag };
                TagDictionary.Add(tag, tagData);
            }
            else if (tagData.Parent != VisualTreeHelper.GetParent(child))
            {
                previousRect = TranslateRect(tagData.AppRect, root, newTagData.Parent);
                parentChange = true;
            }
            else
            {
                previousRect = tagData.ParentRect;
            }

            FrameworkElement originalChild = child;

            if ((!FluidMoveBehavior.IsEmptyRect(previousRect) && !FluidMoveBehavior.IsEmptyRect(newTagData.ParentRect)) && (!IsClose(previousRect.Left, newTagData.ParentRect.Left) || !IsClose(previousRect.Top, newTagData.ParentRect.Top)) ||
                (child != tagData.Child && transitionStoryboardDictionary.ContainsKey(tag)))
            {
                Rect currentRect = previousRect;
                bool forceFloatAbove = false;

                Storyboard oldTransitionStoryboard = null;
                if (transitionStoryboardDictionary.TryGetValue(tag, out oldTransitionStoryboard))
                {
                    object tagOverlay = GetOverlay(tagData.Child);
                    AdornerContainer adornerContainer = (AdornerContainer)tagOverlay;

                    forceFloatAbove = (tagOverlay != null); // if floating before, we need to keep floating
                    FrameworkElement elementWithTransform = tagData.Child;

                    if (tagOverlay != null)
                    {
                        Canvas overlayCanvas = adornerContainer.Child as Canvas;
                        if (overlayCanvas != null)
                        {
                            elementWithTransform = overlayCanvas.Children[0] as FrameworkElement;
                        }
                    }

                    if (!usingBeforeLoaded)
                    {
                        Transform transform = GetTransform(elementWithTransform);
                        currentRect = transform.TransformBounds(currentRect);
                    }

                    transitionStoryboardDictionary.Remove(tag);
                    oldTransitionStoryboard.Stop();
                    oldTransitionStoryboard = null;
                    RemoveTransform(elementWithTransform);

                    if (tagOverlay != null)
                    {
                        System.Windows.Documents.AdornerLayer.GetAdornerLayer(root).Remove(adornerContainer);
                        TransferLocalValue(tagData.Child, FluidMoveBehavior.cacheDuringOverlayProperty, FrameworkElement.RenderTransformProperty);
                        SetOverlay(tagData.Child, null);
                    }
                }

                object overlay = null;

                if (forceFloatAbove || (parentChange && this.FloatAbove))
                {
                    Canvas canvas = new Canvas() { Width = newTagData.ParentRect.Width, Height = newTagData.ParentRect.Height, IsHitTestVisible = false };

                    Rectangle rectangle = new Rectangle() { Width = newTagData.ParentRect.Width, Height = newTagData.ParentRect.Height, IsHitTestVisible = false };
                    rectangle.Fill = new VisualBrush(child);
                    canvas.Children.Add(rectangle);
                    AdornerContainer adornerContainer = new AdornerContainer(child) { Child = canvas };
                    overlay = adornerContainer;

                    SetOverlay(originalChild, overlay);

                    System.Windows.Documents.AdornerLayer adorners = System.Windows.Documents.AdornerLayer.GetAdornerLayer(root);
                    adorners.Add(adornerContainer);

                    TransferLocalValue(child, FrameworkElement.RenderTransformProperty, FluidMoveBehavior.cacheDuringOverlayProperty);
                    child.RenderTransform = new TranslateTransform(-10000, -10000);
                    canvas.RenderTransform = new TranslateTransform(10000, 10000);

                    child = rectangle;
                }

                Rect parentRect = newTagData.ParentRect;
                Storyboard transitionStoryboard = CreateTransitionStoryboard(child, usingBeforeLoaded, ref parentRect, ref currentRect);

                transitionStoryboardDictionary.Add(tag, transitionStoryboard);

                transitionStoryboard.Completed += delegate (object sender, EventArgs e)
                {
                    Storyboard currentlyRunningStoryboard;
                    if (transitionStoryboardDictionary.TryGetValue(tag, out currentlyRunningStoryboard) && currentlyRunningStoryboard == transitionStoryboard)
                    {
                        transitionStoryboardDictionary.Remove(tag);
                        transitionStoryboard.Stop();
                        RemoveTransform(child);
                        child.InvalidateMeasure();

                        if (overlay != null)
                        {
                            System.Windows.Documents.AdornerLayer.GetAdornerLayer(root).Remove((AdornerContainer)overlay);
                            TransferLocalValue(originalChild, FluidMoveBehavior.cacheDuringOverlayProperty, FrameworkElement.RenderTransformProperty);
                            SetOverlay(originalChild, null);
                        }
                    }
                };

                transitionStoryboard.Begin();
            }

            tagData.ParentRect = newTagData.ParentRect;
            tagData.AppRect = newTagData.AppRect;
            tagData.Parent = newTagData.Parent;
            tagData.Child = newTagData.Child;
            tagData.Timestamp = newTagData.Timestamp;
        }

        private Storyboard CreateTransitionStoryboard(FrameworkElement child, bool usingBeforeLoaded, ref Rect layoutRect, ref Rect currentRect)
        {
            Duration duration = this.Duration;
            Storyboard transitionStoryboard = new Storyboard();
            transitionStoryboard.Duration = duration;

            double xScaleFrom = (!usingBeforeLoaded || layoutRect.Width == 0.0) ? 1.0 : (currentRect.Width / layoutRect.Width);
            double yScaleFrom = (!usingBeforeLoaded || layoutRect.Height == 0.0) ? 1.0 : (currentRect.Height / layoutRect.Height);
            double xFrom = currentRect.Left - layoutRect.Left;
            double yFrom = currentRect.Top - layoutRect.Top;

            TransformGroup transform = new TransformGroup();
            transform.Children.Add(new ScaleTransform() { ScaleX = xScaleFrom, ScaleY = yScaleFrom });
            transform.Children.Add(new TranslateTransform() { X = xFrom, Y = yFrom });
            AddTransform(child, transform);

            string prefix = "(FrameworkElement.RenderTransform).";

            TransformGroup transformGroup = child.RenderTransform as TransformGroup;
            if (transformGroup != null && GetHasTransformWrapper(child))
            {
                prefix += "(TransformGroup.Children)[" + (transformGroup.Children.Count - 1) + "].";
            }

            if (usingBeforeLoaded)
            {
                if (xScaleFrom != 1.0)
                {
                    DoubleAnimation xScaleAnimation = new DoubleAnimation() { Duration = duration, From = xScaleFrom, To = 1.0 };
                    Storyboard.SetTarget(xScaleAnimation, child);
                    Storyboard.SetTargetProperty(xScaleAnimation, new PropertyPath(prefix + "(TransformGroup.Children)[0].(ScaleTransform.ScaleX)", new object[0]));
                    xScaleAnimation.EasingFunction = this.EaseX;
                    transitionStoryboard.Children.Add(xScaleAnimation);
                }

                if (yScaleFrom != 1.0)
                {
                    DoubleAnimation yScaleAnimation = new DoubleAnimation() { Duration = duration, From = yScaleFrom, To = 1.0 };
                    Storyboard.SetTarget(yScaleAnimation, child);
                    Storyboard.SetTargetProperty(yScaleAnimation, new PropertyPath(prefix + "(TransformGroup.Children)[0].(ScaleTransform.ScaleY)", new object[0]));
                    yScaleAnimation.EasingFunction = this.EaseY;
                    transitionStoryboard.Children.Add(yScaleAnimation);
                }
            }

            if (xFrom != 0.0)
            {
                DoubleAnimation xAnimation = new DoubleAnimation() { Duration = duration, From = xFrom, To = 0.0 };
                Storyboard.SetTarget(xAnimation, child);
                Storyboard.SetTargetProperty(xAnimation, new PropertyPath(prefix + "(TransformGroup.Children)[1].(TranslateTransform.X)", new object[0]));
                xAnimation.EasingFunction = this.EaseX;
                transitionStoryboard.Children.Add(xAnimation);
            }

            if (yFrom != 0.0)
            {
                DoubleAnimation yAnimation = new DoubleAnimation() { Duration = duration, From = yFrom, To = 0.0 };
                Storyboard.SetTarget(yAnimation, child);
                Storyboard.SetTargetProperty(yAnimation, new PropertyPath(prefix + "(TransformGroup.Children)[1].(TranslateTransform.Y)", new object[0]));
                yAnimation.EasingFunction = this.EaseY;
                transitionStoryboard.Children.Add(yAnimation);
            }

            return transitionStoryboard;
        }

        private static void AddTransform(FrameworkElement child, Transform transform)
        {
            TransformGroup transformGroup = child.RenderTransform as TransformGroup;

            if (transformGroup == null)
            {
                transformGroup = new TransformGroup();
                transformGroup.Children.Add(child.RenderTransform);
                child.RenderTransform = transformGroup;
                SetHasTransformWrapper(child, true);
            }

            transformGroup.Children.Add(transform);
        }

        private static Transform GetTransform(FrameworkElement child)
        {
            TransformGroup transformGroup = child.RenderTransform as TransformGroup;
            if (transformGroup != null && transformGroup.Children.Count > 0)
            {
                return transformGroup.Children[transformGroup.Children.Count - 1];
            }
            else
            {
                return new TranslateTransform();
            }
        }

        private static void RemoveTransform(FrameworkElement child)
        {
            TransformGroup transformGroup = child.RenderTransform as TransformGroup;

            if (transformGroup != null)
            {
                if (GetHasTransformWrapper(child))
                {
                    child.RenderTransform = transformGroup.Children[0];
                    SetHasTransformWrapper(child, false);
                }
                else
                {
                    transformGroup.Children.RemoveAt(transformGroup.Children.Count - 1);
                }
            }
        }

        private static void TransferLocalValue(FrameworkElement element, DependencyProperty source, DependencyProperty dest)
        {
            object value = element.ReadLocalValue(source);

            BindingExpressionBase bindingExpressionBase = value as BindingExpressionBase;
            if (bindingExpressionBase != null)
            {
                element.SetBinding(dest, bindingExpressionBase.ParentBindingBase);
            }
            else if (value == DependencyProperty.UnsetValue)
            {
                element.ClearValue(dest);
            }
            else
            {
                element.SetValue(dest, element.GetAnimationBaseValue(source));
            }

            element.ClearValue(source);
        }

        private static bool IsClose(double a, double b)
        {
            return (Math.Abs((double)(a - b)) < 1E-07);
        }

        private static bool IsEmptyRect(Rect rect)
        {
            return ((rect.IsEmpty || double.IsNaN(rect.Left)) || double.IsNaN(rect.Top));
        }
    }
}
