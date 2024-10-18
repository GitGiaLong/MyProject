using Core.Libraries.WPF.Interactivities.Extensions;
using System.Diagnostics;
using System.Windows.Input;

namespace Core.Libraries.WPF.Interactivities.Layouts
{
    public class MouseDragElementBehavior : Behavior<FrameworkElement>
    {
        #region Fields

        private bool settingPosition;
        private Point relativePosition;
        private Transform cachedRenderTransform;

        #endregion

        #region Events

        public event MouseEventHandler DragBegun;

        public event MouseEventHandler Dragging;

        public event MouseEventHandler DragFinished;

        #endregion

        #region Dependency properties

        public double X
        {
            get { return (double)this.GetValue(XProperty); }
            set { this.SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(nameof(X), typeof(double), 
            typeof(MouseDragElementBehavior), new PropertyMetadata(double.NaN, new PropertyChangedCallback(OnXChanged)));

        public double Y
        {
            get { return (double)this.GetValue(YProperty); }
            set { this.SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty = DependencyProperty.Register(nameof(Y), typeof(double), 
            typeof(MouseDragElementBehavior), new PropertyMetadata(double.NaN, new PropertyChangedCallback(OnYChanged)));

        public bool ConstrainToParentBounds
        {
            get { return (bool)this.GetValue(ConstrainToParentBoundsProperty); }
            set { this.SetValue(ConstrainToParentBoundsProperty, value); }
        }
        public static readonly DependencyProperty ConstrainToParentBoundsProperty = DependencyProperty.Register(nameof(ConstrainToParentBounds), 
            typeof(bool), typeof(MouseDragElementBehavior), new PropertyMetadata(false, new PropertyChangedCallback(OnConstrainToParentBoundsChanged)));

        #endregion

        #region PropertyChangedHandlers

        private static void OnXChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            MouseDragElementBehavior dragBehavior = (MouseDragElementBehavior)sender;
            dragBehavior.UpdatePosition(new Point((double)args.NewValue, dragBehavior.Y));
        }

        private static void OnYChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            MouseDragElementBehavior dragBehavior = (MouseDragElementBehavior)sender;
            dragBehavior.UpdatePosition(new Point(dragBehavior.X, (double)args.NewValue));
        }

        private static void OnConstrainToParentBoundsChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            MouseDragElementBehavior b = (MouseDragElementBehavior)sender;
            b.UpdatePosition(new Point(b.X, b.Y));
        }

        #endregion

        #region Private properties

        private Point ActualPosition
        {
            get
            {
                GeneralTransform elementToRoot = this.AssociatedObject.TransformToVisual(this.RootElement);
                Point translation = MouseDragElementBehavior.GetTransformOffset(elementToRoot);
                return new Point(translation.X, translation.Y);
            }
        }

        private Rect ElementBounds
        {
            get
            {
                Rect layoutRect = ExtendedVisualStateManager.GetLayoutRect(this.AssociatedObject);
                return new Rect(new Point(0, 0), new Size(layoutRect.Width, layoutRect.Height));
            }
        }

        private FrameworkElement ParentElement
        {
            get
            {
                return this.AssociatedObject.Parent as FrameworkElement;
            }
        }

        private UIElement RootElement
        {
            get
            {
                DependencyObject child = this.AssociatedObject;
                DependencyObject parent = child;
                while (parent != null)
                {
                    child = parent;
                    parent = VisualTreeHelper.GetParent(child);
                }
                return child as UIElement;
            }
        }

        private Transform RenderTransform
        {
            get
            {
                if (this.cachedRenderTransform == null || !object.ReferenceEquals(cachedRenderTransform, this.AssociatedObject.RenderTransform))
                {
                    Transform clonedTransform = MouseDragElementBehavior.CloneTransform(this.AssociatedObject.RenderTransform);
                    this.RenderTransform = clonedTransform;
                }
                return cachedRenderTransform;
            }
            set
            {
                if (this.cachedRenderTransform != value)
                {
                    this.cachedRenderTransform = value;
                    this.AssociatedObject.RenderTransform = value;
                }
            }
        }

        #endregion

        #region Private methods

        private void UpdatePosition(Point point)
        {
            if (!this.settingPosition && this.AssociatedObject != null)
            {
                GeneralTransform elementToRoot = this.AssociatedObject.TransformToVisual(this.RootElement);
                Point translation = MouseDragElementBehavior.GetTransformOffset(elementToRoot);
                double xChange = double.IsNaN(point.X) ? 0 : point.X - translation.X;
                double yChange = double.IsNaN(point.Y) ? 0 : point.Y - translation.Y;
                this.ApplyTranslation(xChange, yChange);
            }
        }

        private void ApplyTranslation(double x, double y)
        {
            if (this.ParentElement != null)
            {
                GeneralTransform rootToParent = this.RootElement.TransformToVisual(this.ParentElement);
                Point transformedPoint = MouseDragElementBehavior.TransformAsVector(rootToParent, x, y);
                x = transformedPoint.X;
                y = transformedPoint.Y;

                if (this.ConstrainToParentBounds)
                {
                    FrameworkElement parentElement = this.ParentElement;
                    Rect parentBounds = new Rect(0, 0, parentElement.ActualWidth, parentElement.ActualHeight);

                    GeneralTransform objectToParent = this.AssociatedObject.TransformToVisual(parentElement);
                    Rect objectBoundingBox = this.ElementBounds;
                    objectBoundingBox = objectToParent.TransformBounds(objectBoundingBox);

                    Rect endPosition = objectBoundingBox;
                    endPosition.X += x;
                    endPosition.Y += y;

                    if (!MouseDragElementBehavior.RectContainsRect(parentBounds, endPosition))
                    {
                        if (endPosition.X < parentBounds.Left)
                        {
                            double diff = endPosition.X - parentBounds.Left;
                            x -= diff;
                        }
                        else if (endPosition.Right > parentBounds.Right)
                        {
                            double diff = endPosition.Right - parentBounds.Right;
                            x -= diff;
                        }

                        if (endPosition.Y < parentBounds.Top)
                        {
                            double diff = endPosition.Y - parentBounds.Top;
                            y -= diff;
                        }
                        else if (endPosition.Bottom > parentBounds.Bottom)
                        {
                            double diff = endPosition.Bottom - parentBounds.Bottom;
                            y -= diff;
                        }
                    }
                }

                this.ApplyTranslationTransform(x, y);
            }
        }

        internal void ApplyTranslationTransform(double x, double y)
        {
            Transform renderTransform = this.RenderTransform;
            TranslateTransform translateTransform = renderTransform as TranslateTransform;

            if (translateTransform == null)
            {
                TransformGroup renderTransformGroup = renderTransform as TransformGroup;
                MatrixTransform renderMatrixTransform = renderTransform as MatrixTransform;
                if (renderTransformGroup != null)
                {
                    if (renderTransformGroup.Children.Count > 0)
                    {
                        translateTransform = renderTransformGroup.Children[renderTransformGroup.Children.Count - 1] as TranslateTransform;
                    }
                    if (translateTransform == null)
                    {
                        translateTransform = new TranslateTransform();
                        renderTransformGroup.Children.Add(translateTransform);
                    }
                }
                else if (renderMatrixTransform != null)
                {
                    Matrix matrix = renderMatrixTransform.Matrix;
                    matrix.OffsetX += x;
                    matrix.OffsetY += y;
                    MatrixTransform matrixTransform = new MatrixTransform();
                    matrixTransform.Matrix = matrix;
                    this.RenderTransform = matrixTransform;
                    return;
                }
                else
                {
                    TransformGroup transformGroup = new TransformGroup();
                    translateTransform = new TranslateTransform();
                    if (renderTransform != null)
                    {
                        transformGroup.Children.Add(renderTransform);
                    }
                    transformGroup.Children.Add(translateTransform);
                    this.RenderTransform = transformGroup;
                }
            }

            Debug.Assert(translateTransform != null, "TranslateTransform should not be null by this point.");
            translateTransform.X += x;
            translateTransform.Y += y;
        }

        internal static Transform CloneTransform(Transform transform)
        {
            ScaleTransform scaleTransform = null;
            RotateTransform rotateTransform = null;
            SkewTransform skewTransform = null;
            TranslateTransform translateTransform = null;
            MatrixTransform matrixTransform = null;
            TransformGroup transformGroup = null;

            if (transform == null)
            {
                return null;
            }

            Type transformType = transform.GetType();
            if ((scaleTransform = transform as ScaleTransform) != null)
            {
                return new ScaleTransform()
                {
                    CenterX = scaleTransform.CenterX,
                    CenterY = scaleTransform.CenterY,
                    ScaleX = scaleTransform.ScaleX,
                    ScaleY = scaleTransform.ScaleY,
                };
            }
            else if ((rotateTransform = transform as RotateTransform) != null)
            {
                return new RotateTransform()
                {
                    Angle = rotateTransform.Angle,
                    CenterX = rotateTransform.CenterX,
                    CenterY = rotateTransform.CenterY,
                };
            }
            else if ((skewTransform = transform as SkewTransform) != null)
            {
                return new SkewTransform()
                {
                    AngleX = skewTransform.AngleX,
                    AngleY = skewTransform.AngleY,
                    CenterX = skewTransform.CenterX,
                    CenterY = skewTransform.CenterY,
                };
            }
            else if ((translateTransform = transform as TranslateTransform) != null)
            {
                return new TranslateTransform()
                {
                    X = translateTransform.X,
                    Y = translateTransform.Y,
                };
            }
            else if ((matrixTransform = transform as MatrixTransform) != null)
            {
                return new MatrixTransform()
                {
                    Matrix = matrixTransform.Matrix,
                };
            }
            else if ((transformGroup = transform as TransformGroup) != null)
            {
                TransformGroup group = new TransformGroup();
                foreach (Transform childTransform in transformGroup.Children)
                {
                    group.Children.Add(CloneTransform(childTransform));
                }
                return group;
            }

            Debug.Assert(false, "Unexpected Transform type encountered");
            return null;
        }

        private void UpdatePosition()
        {
            GeneralTransform elementToRoot = this.AssociatedObject.TransformToVisual(this.RootElement);
            Point translation = MouseDragElementBehavior.GetTransformOffset(elementToRoot);
            this.X = translation.X;
            this.Y = translation.Y;
        }

        internal void StartDrag(Point positionInElementCoordinates)
        {
            this.relativePosition = positionInElementCoordinates;

            this.AssociatedObject.CaptureMouse();

            this.AssociatedObject.MouseMove += this.OnMouseMove;
            this.AssociatedObject.LostMouseCapture += this.OnLostMouseCapture;
            this.AssociatedObject.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.OnMouseLeftButtonUp), false /* handledEventsToo */);
        }

        internal void HandleDrag(Point newPositionInElementCoordinates)
        {
            double relativeXDiff = newPositionInElementCoordinates.X - this.relativePosition.X;
            double relativeYDiff = newPositionInElementCoordinates.Y - this.relativePosition.Y;

            GeneralTransform elementToRoot = this.AssociatedObject.TransformToVisual(this.RootElement);
            Point relativeDifferenceInRootCoordinates = TransformAsVector(elementToRoot, relativeXDiff, relativeYDiff);

            this.settingPosition = true;
            this.ApplyTranslation(relativeDifferenceInRootCoordinates.X, relativeDifferenceInRootCoordinates.Y);
            this.UpdatePosition();
            this.settingPosition = false;
        }

        internal void EndDrag()
        {
            this.AssociatedObject.MouseMove -= this.OnMouseMove;
            this.AssociatedObject.LostMouseCapture -= this.OnLostMouseCapture;
            this.AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.OnMouseLeftButtonUp));
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.StartDrag(e.GetPosition(this.AssociatedObject));

            if (this.DragBegun != null)
            {
                this.DragBegun(this, e);
            }
        }

        private void OnLostMouseCapture(object sender, MouseEventArgs e)
        {
            this.EndDrag();

            if (this.DragFinished != null)
            {
                this.DragFinished(this, e);
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.AssociatedObject.ReleaseMouseCapture();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            this.HandleDrag(e.GetPosition(this.AssociatedObject));

            if (this.Dragging != null)
            {
                this.Dragging(this, e);
            }
        }

        #endregion

        #region Linear algebra helper methods

        private static bool RectContainsRect(Rect rect1, Rect rect2)
        {
            if (rect1.IsEmpty || rect2.IsEmpty)
            {
                return false;
            }
            return ((((rect1.X <= rect2.X) && (rect1.Y <= rect2.Y)) && ((rect1.X + rect1.Width) >= (rect2.X + rect2.Width))) && ((rect1.Y + rect1.Height) >= (rect2.Y + rect2.Height)));
        }

        private static Point TransformAsVector(GeneralTransform transform, double x, double y)
        {
            Point origin = transform.Transform(new Point(0, 0));
            Point transformedPoint = transform.Transform(new Point(x, y));
            return new Point(transformedPoint.X - origin.X, transformedPoint.Y - origin.Y);
        }

        private static Point GetTransformOffset(GeneralTransform transform)
        {
            return transform.Transform(new Point(0, 0));
        }

        #endregion

        protected override void OnAttached()
        {
            this.AssociatedObject.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.OnMouseLeftButtonDown), false /* handledEventsToo */);
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.RemoveHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.OnMouseLeftButtonDown));
        }
    }
}
