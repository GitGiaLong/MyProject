using Core.WPF.Interactivities.Layouts;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Core.WPF.Interactivities.Inputs
{
    public class TranslateZoomRotateBehavior : Behavior<FrameworkElement>
    {
        #region Fields

        private Transform cachedRenderTransform;

        private bool isDragging = false;
        private bool isAdjustingTransform = false;
        private Point lastMousePoint;

        private double lastScaleX = 1.0;
        private double lastScaleY = 1.0;
        private const double HardMinimumScale = 1e-6;
        #endregion

        #region Dependency properties

        public static readonly DependencyProperty SupportedGesturesProperty =
            DependencyProperty.Register("SupportedGestures", typeof(ManipulationModes), typeof(TranslateZoomRotateBehavior), new PropertyMetadata(ManipulationModes.All));

        public static readonly DependencyProperty TranslateFrictionProperty =
            DependencyProperty.Register("TranslateFriction", typeof(double), typeof(TranslateZoomRotateBehavior), new PropertyMetadata(0.0, frictionChanged, coerceFriction));

        public static readonly DependencyProperty RotationalFrictionProperty =
            DependencyProperty.Register("RotationalFriction", typeof(double), typeof(TranslateZoomRotateBehavior), new PropertyMetadata(0.0, frictionChanged, coerceFriction));

        public static readonly DependencyProperty ConstrainToParentBoundsProperty =
            DependencyProperty.Register("ConstrainToParentBounds", typeof(bool), typeof(TranslateZoomRotateBehavior), new PropertyMetadata(false));

        public static readonly DependencyProperty MinimumScaleProperty =
            DependencyProperty.Register("MinimumScale", typeof(double), typeof(TranslateZoomRotateBehavior), new PropertyMetadata(0.1));

        public static readonly DependencyProperty MaximumScaleProperty =
            DependencyProperty.Register("MaximumScale", typeof(double), typeof(TranslateZoomRotateBehavior), new PropertyMetadata(10.0));

        #endregion

        #region Public properties
        public ManipulationModes SupportedGestures
        {
            get { return (ManipulationModes)this.GetValue(TranslateZoomRotateBehavior.SupportedGesturesProperty); }
            set { this.SetValue(TranslateZoomRotateBehavior.SupportedGesturesProperty, value); }
        }

        public double TranslateFriction
        {
            get { return (double)this.GetValue(TranslateZoomRotateBehavior.TranslateFrictionProperty); }
            set { this.SetValue(TranslateZoomRotateBehavior.TranslateFrictionProperty, value); }
        }

        public double RotationalFriction
        {
            get { return (double)this.GetValue(TranslateZoomRotateBehavior.RotationalFrictionProperty); }
            set { this.SetValue(TranslateZoomRotateBehavior.RotationalFrictionProperty, value); }
        }

        public bool ConstrainToParentBounds
        {
            get { return (bool)this.GetValue(TranslateZoomRotateBehavior.ConstrainToParentBoundsProperty); }
            set { this.SetValue(TranslateZoomRotateBehavior.ConstrainToParentBoundsProperty, value); }
        }

        public double MinimumScale
        {
            get { return (double)this.GetValue(TranslateZoomRotateBehavior.MinimumScaleProperty); }
            set { this.SetValue(TranslateZoomRotateBehavior.MinimumScaleProperty, value); }
        }

        public double MaximumScale
        {
            get { return (double)this.GetValue(TranslateZoomRotateBehavior.MaximumScaleProperty); }
            set { this.SetValue(TranslateZoomRotateBehavior.MaximumScaleProperty, value); }
        }

        #endregion

        #region PropertyChangedHandlers

        private static void frictionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
        }

        private static object coerceFriction(DependencyObject sender, object value)
        {
            double friction = (double)value;
            return Math.Max(0, Math.Min(1, friction));
        }


        #endregion

        #region Private properties

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

        private Point RenderTransformOriginInElementCoordinates
        {
            get
            {
                return new Point(this.AssociatedObject.RenderTransformOrigin.X * this.AssociatedObject.ActualWidth,
                                this.AssociatedObject.RenderTransformOrigin.Y * this.AssociatedObject.ActualHeight);

            }
        }

        private Matrix FullTransformValue
        {
            get
            {
                Point center = this.RenderTransformOriginInElementCoordinates;
                Matrix matrix = this.RenderTransform.Value;
                matrix.TranslatePrepend(-center.X, -center.Y);
                matrix.Translate(center.X, center.Y);
                return matrix;
            }
        }

        private MatrixTransform MatrixTransform
        {
            get
            {
                this.EnsureTransform();
                return (MatrixTransform)this.RenderTransform;
            }
        }

        private FrameworkElement ParentElement
        {
            get
            {
                return this.AssociatedObject.Parent as FrameworkElement;
            }
        }

        #endregion

        #region Private methods

        internal void EnsureTransform()
        {
            MatrixTransform transform = this.RenderTransform as MatrixTransform;
            if (transform == null || transform.IsFrozen)
            {
                if (this.RenderTransform != null)
                {
                    transform = new MatrixTransform(this.FullTransformValue);
                }
                else
                {
                    transform = new MatrixTransform(Matrix.Identity);
                }
                this.RenderTransform = transform;
            }
            this.AssociatedObject.RenderTransformOrigin = new Point(0, 0);
        }

        internal void ApplyRotationTransform(double angle, Point rotationPoint)
        {
            Matrix matrix = this.MatrixTransform.Matrix;
            matrix.RotateAt(angle, rotationPoint.X, rotationPoint.Y);
            this.MatrixTransform.Matrix = matrix;
        }

        internal void ApplyScaleTransform(double scaleX, double scaleY, Point scalePoint)
        {
            double newScaleX = scaleX * this.lastScaleX;
            newScaleX = Math.Min(Math.Max(Math.Max(TranslateZoomRotateBehavior.HardMinimumScale, this.MinimumScale), newScaleX), this.MaximumScale);
            scaleX = newScaleX / this.lastScaleX;
            this.lastScaleX = scaleX * this.lastScaleX;

            double newScaleY = scaleY * this.lastScaleY;
            newScaleY = Math.Min(Math.Max(Math.Max(TranslateZoomRotateBehavior.HardMinimumScale, this.MinimumScale), newScaleY), this.MaximumScale);
            scaleY = newScaleY / this.lastScaleY;
            this.lastScaleY = scaleY * this.lastScaleY;

            Matrix matrix = this.MatrixTransform.Matrix;
            matrix.ScaleAt(scaleX, scaleY, scalePoint.X, scalePoint.Y);
            this.MatrixTransform.Matrix = matrix;
        }

        internal void ApplyTranslateTransform(double x, double y)
        {
            Matrix matrix = this.MatrixTransform.Matrix;
            matrix.Translate(x, y);
            this.MatrixTransform.Matrix = matrix;
        }

        private void ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            FrameworkElement manipulationContainer = this.ParentElement;
            if (manipulationContainer == null || !manipulationContainer.IsAncestorOf(this.AssociatedObject))
            {
                manipulationContainer = this.AssociatedObject;
            }
            e.ManipulationContainer = manipulationContainer;
            e.Mode = this.SupportedGestures;
            e.Handled = true;
        }

        private void ManipulationInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            double translateFactor = this.TranslateFriction == 1 ? 1.0 : -.00666 * Math.Log(1 - this.TranslateFriction);
            double translateDeceleration = e.InitialVelocities.LinearVelocity.Length * translateFactor;

            e.TranslationBehavior = new InertiaTranslationBehavior()
            {
                InitialVelocity = e.InitialVelocities.LinearVelocity,
                DesiredDeceleration = Math.Max(translateDeceleration, 0)
            };

            double rotateFactor = this.RotationalFriction == 1 ? 1.0 : -.00666 * Math.Log(1 - this.RotationalFriction);
            double rotateDeceleration = Math.Abs(e.InitialVelocities.AngularVelocity) * rotateFactor;

            e.RotationBehavior = new InertiaRotationBehavior()
            {
                InitialVelocity = e.InitialVelocities.AngularVelocity,
                DesiredDeceleration = Math.Max(rotateDeceleration, 0)
            };

            e.Handled = true;
        }

        private void ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            this.EnsureTransform();
            ManipulationDelta currentDelta = e.DeltaManipulation;

            Point origin = new Point(this.AssociatedObject.ActualWidth / 2, this.AssociatedObject.ActualHeight / 2);

            Point center = this.FullTransformValue.Transform(origin);

            this.ApplyScaleTransform(currentDelta.Scale.X, currentDelta.Scale.Y, center);
            this.ApplyRotationTransform(currentDelta.Rotation, center);
            this.ApplyTranslateTransform(currentDelta.Translation.X, currentDelta.Translation.Y);

            FrameworkElement container = (FrameworkElement)e.ManipulationContainer;
            Rect parentBounds = new Rect(container.RenderSize);

            Rect childBounds = this.AssociatedObject.TransformToVisual(container).TransformBounds(new Rect(this.AssociatedObject.RenderSize));

            if (e.IsInertial && this.ConstrainToParentBounds && !parentBounds.Contains(childBounds))
            {
                e.Complete();
            }

            e.Handled = true;
        }

        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.AssociatedObject.CaptureMouse();
            this.AssociatedObject.MouseMove += this.AssociatedObject_MouseMove;
            this.AssociatedObject.LostMouseCapture += this.AssociatedObject_LostMouseCapture;
            e.Handled = true;
            this.lastMousePoint = e.GetPosition(this.AssociatedObject);
            this.isDragging = true;
        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.AssociatedObject.ReleaseMouseCapture();
            e.Handled = true;
        }

        private void AssociatedObject_LostMouseCapture(object sender, MouseEventArgs e)
        {
            this.isDragging = false;
            this.AssociatedObject.MouseMove -= this.AssociatedObject_MouseMove;
            this.AssociatedObject.LostMouseCapture -= this.AssociatedObject_LostMouseCapture;
        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isDragging && !this.isAdjustingTransform)
            {
                this.isAdjustingTransform = true;
                Point newPoint = e.GetPosition(this.AssociatedObject);
                Vector delta = newPoint - this.lastMousePoint;
                if ((this.SupportedGestures & ManipulationModes.TranslateX) == 0)
                {
                    delta.X = 0;
                }
                if ((this.SupportedGestures & ManipulationModes.TranslateY) == 0)
                {
                    delta.Y = 0;
                }

                // Transform mouse movement into element space, taking the element's transform into account.
                Vector transformedDelta = this.FullTransformValue.Transform(delta);
                this.ApplyTranslateTransform(transformedDelta.X, transformedDelta.Y);
                // Need to get the position again, as it probably changed when updating the transform.
                this.lastMousePoint = e.GetPosition(this.AssociatedObject);
                this.isAdjustingTransform = false;
            }
        }

        #endregion

        protected override void OnAttached()
        {
            this.AssociatedObject.AddHandler(UIElement.ManipulationStartingEvent, new EventHandler<ManipulationStartingEventArgs>(this.ManipulationStarting), false /* handledEventsToo */);
            this.AssociatedObject.AddHandler(UIElement.ManipulationInertiaStartingEvent, new EventHandler<ManipulationInertiaStartingEventArgs>(this.ManipulationInertiaStarting), false /* handledEventsToo */);
            this.AssociatedObject.AddHandler(UIElement.ManipulationDeltaEvent, new EventHandler<ManipulationDeltaEventArgs>(this.ManipulationDelta), false /* handledEventsToo */);
            this.AssociatedObject.IsManipulationEnabled = true;

            this.AssociatedObject.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.MouseLeftButtonDown), false /* handledEventsToo */);
            this.AssociatedObject.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.MouseLeftButtonUp), false /* handledEventsToo */);
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.RemoveHandler(UIElement.ManipulationStartingEvent, new EventHandler<ManipulationStartingEventArgs>(this.ManipulationStarting));
            this.AssociatedObject.RemoveHandler(UIElement.ManipulationInertiaStartingEvent, new EventHandler<ManipulationInertiaStartingEventArgs>(this.ManipulationInertiaStarting));
            this.AssociatedObject.RemoveHandler(UIElement.ManipulationDeltaEvent, new EventHandler<ManipulationDeltaEventArgs>(this.ManipulationDelta));
            this.AssociatedObject.IsManipulationEnabled = false;

            this.AssociatedObject.AddHandler(UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.MouseLeftButtonDown), false /* handledEventsToo */);
            this.AssociatedObject.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.MouseLeftButtonUp), false /* handledEventsToo */);
        }
    }
}
