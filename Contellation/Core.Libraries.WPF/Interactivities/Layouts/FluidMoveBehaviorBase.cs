using Core.Libraries.WPF.Interactivities.Enums;
using Core.Libraries.WPF.Interactivities.Extensions;
using System.Windows.Controls;
using System.Windows.Data;

namespace Core.Libraries.WPF.Interactivities.Layouts
{
    public abstract class FluidMoveBehaviorBase : Behavior<FrameworkElement>
    {
        public FluidMoveScope AppliesTo
        {
            get { return (FluidMoveScope)this.GetValue(AppliesToProperty); }
            set { this.SetValue(AppliesToProperty, value); }
        }
        public static readonly DependencyProperty AppliesToProperty = DependencyProperty.Register(nameof(AppliesTo), typeof(FluidMoveScope), 
            typeof(FluidMoveBehaviorBase), new PropertyMetadata(FluidMoveScope.Self));

        public bool IsActive
        {
            get { return (bool)this.GetValue(IsActiveProperty); }
            set { this.SetValue(IsActiveProperty, value); }
        }
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof(IsActive), typeof(bool), 
            typeof(FluidMoveBehaviorBase), new PropertyMetadata(true));

        public TagType Tag
        {
            get { return (TagType)this.GetValue(TagProperty); }
            set { this.SetValue(TagProperty, value); }
        }
        public static readonly DependencyProperty TagProperty = DependencyProperty.Register(nameof(Tag), typeof(TagType), 
            typeof(FluidMoveBehaviorBase), new PropertyMetadata(TagType.Element));

        public string TagPath
        {
            get { return (string)this.GetValue(TagPathProperty); }
            set { this.SetValue(TagPathProperty, value); }
        }
        public static readonly DependencyProperty TagPathProperty = DependencyProperty.Register(nameof(TagPath), typeof(string), 
            typeof(FluidMoveBehaviorBase), new PropertyMetadata(String.Empty));

        protected static readonly DependencyProperty IdentityTagProperty = DependencyProperty.RegisterAttached("IdentityTag", typeof(object), 
            typeof(FluidMoveBehaviorBase), new PropertyMetadata(null));
        protected static object GetIdentityTag(DependencyObject obj) { return obj.GetValue(IdentityTagProperty); }
        protected static void SetIdentityTag(DependencyObject obj, object value) { obj.SetValue(IdentityTagProperty, value); }

        internal class TagData
        {
            public FrameworkElement Child { get; set; }
            public FrameworkElement Parent { get; set; }
            public Rect ParentRect { get; set; }
            public Rect AppRect { get; set; }
            public DateTime Timestamp { get; set; }
            public object InitialTag { get; set; }
        }

        internal static Dictionary<object, TagData> TagDictionary = new Dictionary<object, TagData>();

        private static DateTime nextToLastPurgeTick = DateTime.MinValue;
        private static DateTime lastPurgeTick = DateTime.MinValue;
        private static TimeSpan minTickDelta = TimeSpan.FromSeconds(0.5);

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.LayoutUpdated += this.AssociatedObject_LayoutUpdated;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.LayoutUpdated -= this.AssociatedObject_LayoutUpdated;
        }

        private void AssociatedObject_LayoutUpdated(object sender, EventArgs e)
        {
            if (!this.IsActive)
            {
                return;
            }

            if (DateTime.Now - lastPurgeTick >= minTickDelta)
            {
                List<object> deadTags = null;

                foreach (KeyValuePair<object, TagData> pair in TagDictionary)
                {
                    if (pair.Value.Timestamp < nextToLastPurgeTick)
                    {
                        if (deadTags == null)
                        {
                            deadTags = new List<object>();
                        }
                        deadTags.Add(pair.Key);
                    }
                }

                if (deadTags != null)
                {
                    foreach (object tag in deadTags)
                    {
                        TagDictionary.Remove(tag);
                    }
                }

                nextToLastPurgeTick = lastPurgeTick;
                lastPurgeTick = DateTime.Now;
            }

            if (this.AppliesTo == FluidMoveScope.Self)
            {
                this.UpdateLayoutTransition(this.AssociatedObject);
            }
            else
            {
                Panel panel = this.AssociatedObject as Panel;
                if (panel != null)
                {
                    foreach (FrameworkElement child in panel.Children)
                    {
                        this.UpdateLayoutTransition(child);
                    }
                }
            }
        }

        private void UpdateLayoutTransition(FrameworkElement child)
        {
            if (child.Visibility == Visibility.Collapsed || !child.IsLoaded)
            {
                if (this.ShouldSkipInitialLayout)
                {
                    return;
                }
            }

            FrameworkElement root = GetVisualRoot(child);

            TagData newTagData = new TagData();
            newTagData.Parent = VisualTreeHelper.GetParent(child) as FrameworkElement;
            newTagData.ParentRect = ExtendedVisualStateManager.GetLayoutRect(child);
            newTagData.Child = child;
            newTagData.Timestamp = DateTime.Now;

            try
            {
                newTagData.AppRect = TranslateRect(newTagData.ParentRect, newTagData.Parent, root);
            }
            catch (System.ArgumentException)
            {
                if (this.ShouldSkipInitialLayout)
                {
                    return;
                }
            }

            this.EnsureTags(child);

            object tag = GetIdentityTag(child);
            if (tag == null)
            {
                tag = child;
            }

            this.UpdateLayoutTransitionCore(child, root, tag, newTagData);
        }

        protected virtual bool ShouldSkipInitialLayout
        {
            get { return (this.Tag == TagType.DataContext); }
        }

        internal abstract void UpdateLayoutTransitionCore(FrameworkElement child, FrameworkElement root, object tag, TagData newTagData);

        protected virtual void EnsureTags(FrameworkElement child)
        {
            if (this.Tag == TagType.DataContext)
            {
                object tagValue = child.ReadLocalValue(IdentityTagProperty);
                if (!(tagValue is BindingExpression))
                {
                    child.SetBinding(IdentityTagProperty, new Binding(this.TagPath));
                }
            }
        }

        private static FrameworkElement GetVisualRoot(FrameworkElement child)
        {
            while (true)
            {
                FrameworkElement parent = VisualTreeHelper.GetParent(child) as FrameworkElement;
                if (parent == null)
                {
                    return child;
                }
                if (System.Windows.Documents.AdornerLayer.GetAdornerLayer(parent) == null)
                {
                    return child;
                }
                child = parent;
            }
        }

        internal static Rect TranslateRect(Rect rect, FrameworkElement from, FrameworkElement to)
        {
            if (from == null || to == null)
            {
                return rect;
            }

            Point point = new Point(rect.Left, rect.Top);
            point = from.TransformToVisual(to).Transform(point);
            return new Rect(point.X, point.Y, rect.Width, rect.Height);
        }
    }
}
