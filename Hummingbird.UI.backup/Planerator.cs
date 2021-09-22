using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows.Interactivity;

namespace Hummingbird.UI
{
    /// <summary>
    /// Based on Greg Schechter's Planeator
    /// http://blogs.msdn.com/b/greg_schechter/archive/2007/10/26/enter-the-planerator-dead-simple-3d-in-wpf-with-a-stupid-name.aspx
    /// </summary>


    [ContentProperty("Child")]
    public class Planerator : FrameworkElement
    {
        #region Public API

        #region Dependency Properties

        /// <summary>
        /// The rotation x property
        /// </summary>
        public static readonly DependencyProperty RotationXProperty =
            DependencyProperty.Register("RotationX", typeof(double), typeof(Planerator), new UIPropertyMetadata(0.0, (d, args) => ((Planerator)d).UpdateRotation()));
        /// <summary>
        /// The rotation y property
        /// </summary>
        public static readonly DependencyProperty RotationYProperty =
            DependencyProperty.Register("RotationY", typeof(double), typeof(Planerator), new UIPropertyMetadata(0.0, (d, args) => ((Planerator)d).UpdateRotation()));
        /// <summary>
        /// The rotation z property
        /// </summary>
        public static readonly DependencyProperty RotationZProperty =
            DependencyProperty.Register("RotationZ", typeof(double), typeof(Planerator), new UIPropertyMetadata(0.0, (d, args) => ((Planerator)d).UpdateRotation()));
        /// <summary>
        /// The field of view property
        /// </summary>
        public static readonly DependencyProperty FieldOfViewProperty =
            DependencyProperty.Register("FieldOfView", typeof(double), typeof(Planerator), new UIPropertyMetadata(45.0, (d, args) => ((Planerator)d).Update3D(),
                (d, val) => Math.Min(Math.Max((double)val, 0.5), 179.9))); // clamp to a meaningful range


        /// <summary>
        /// Gets or sets the rotation x.
        /// </summary>
        /// <value>
        /// The rotation x.
        /// </value>
        public double RotationX
        {
            get { return (double)GetValue(RotationXProperty); }
            set { SetValue(RotationXProperty, value); }
        }
        /// <summary>
        /// Gets or sets the rotation y.
        /// </summary>
        /// <value>
        /// The rotation y.
        /// </value>
        public double RotationY
        {
            get { return (double)GetValue(RotationYProperty); }
            set { SetValue(RotationYProperty, value); }
        }
        /// <summary>
        /// Gets or sets the rotation z.
        /// </summary>
        /// <value>
        /// The rotation z.
        /// </value>
        public double RotationZ
        {
            get { return (double)GetValue(RotationZProperty); }
            set { SetValue(RotationZProperty, value); }
        }
        /// <summary>
        /// Gets or sets the field of view.
        /// </summary>
        /// <value>
        /// The field of view.
        /// </value>
        public double FieldOfView
        {
            get { return (double)GetValue(FieldOfViewProperty); }
            set { SetValue(FieldOfViewProperty, value); }
        }

        #endregion

        /// <summary>
        /// Gets or sets the child.
        /// </summary>
        /// <value>
        /// The child.
        /// </value>
        public FrameworkElement Child
        {
            get
            {
                return _originalChild;
            }
            set
            {
                if (_originalChild != value)
                {
                    this.RemoveVisualChild(_visualChild);
                    this.RemoveLogicalChild(_logicalChild);

                    // Wrap child with special decorator that catches layout invalidations. 
                    _originalChild = value;
                    _logicalChild = new LayoutInvalidationCatcher() { Child = _originalChild };
                    _visualChild = CreateVisualChild();

                    this.AddVisualChild(_visualChild);

                    // Need to use a logical child here to make sure databinding operations get down to it,
                    // since otherwise the child appears only as the Visual to a Viewport2DVisual3D, which 
                    // doesn't have databinding operations pass into it from above.
                    this.AddLogicalChild(_logicalChild);
                    this.InvalidateMeasure();
                }
            }
        }

        #endregion

        #region Layout Stuff

        /// <summary>
        /// When overridden in a derived class, measures the size in layout required for child elements and determines a size for the <see cref="T:System.Windows.FrameworkElement" />-derived class.
        /// </summary>
        /// <param name="availableSize">The available size that this element can give to child elements. Infinity can be specified as a value to indicate that the element will size to whatever content is available.</param>
        /// <returns>
        /// The size that this element determines it needs during layout, based on its calculations of child element sizes.
        /// </returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            Size result;
            if (_logicalChild != null)
            {
                // Measure based on the size of the logical child, since we want to align with it.
                _logicalChild.Measure(availableSize);
                result = _logicalChild.DesiredSize;
                _visualChild.Measure(result);
            }
            else
            {
                result = new Size(0, 0);
            }
            return result;
        }

        /// <summary>
        /// When overridden in a derived class, positions child elements and determines a size for a <see cref="T:System.Windows.FrameworkElement" /> derived class.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
        /// <returns>
        /// The actual size used.
        /// </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_logicalChild != null)
            {
                _logicalChild.Arrange(new Rect(finalSize));
                _visualChild.Arrange(new Rect(finalSize));
                Update3D();
            }
            return base.ArrangeOverride(finalSize);
        }

        /// <summary>
        /// Overrides <see cref="M:System.Windows.Media.Visual.GetVisualChild(System.Int32)" />, and returns a child at the specified index from a collection of child elements.
        /// </summary>
        /// <param name="index">The zero-based index of the requested child element in the collection.</param>
        /// <returns>
        /// The requested child element. This should not return null; if the provided index is out of range, an exception is thrown.
        /// </returns>
        protected override Visual GetVisualChild(int index)
        {
            return _visualChild;

        }

        /// <summary>
        /// Gets the number of visual child elements within this element.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get
            {
                return _visualChild == null ? 0 : 1;
            }
        }

        #endregion

        #region 3D Stuff

        private FrameworkElement CreateVisualChild()
        {
            MeshGeometry3D simpleQuad = new MeshGeometry3D()
            {
                Positions = new Point3DCollection(_mesh),
                TextureCoordinates = new PointCollection(_texCoords),
                TriangleIndices = new Int32Collection(_indices)
            };

            // Front material is interactive, back material is not.
            Material frontMaterial = new DiffuseMaterial(Brushes.White);
            frontMaterial.SetValue(Viewport2DVisual3D.IsVisualHostMaterialProperty, true);

            VisualBrush vb = new VisualBrush(_logicalChild);
            SetCachingForObject(vb);  // big perf wins by caching!!
            Material backMaterial = new DiffuseMaterial(vb);

            _rotationTransform.Rotation = _quaternionRotation;
            var xfGroup = new Transform3DGroup() { Children = { _scaleTransform, _rotationTransform } };

            GeometryModel3D backModel = new GeometryModel3D() { Geometry = simpleQuad, Transform = xfGroup, BackMaterial = backMaterial };
            Model3DGroup m3dGroup = new Model3DGroup()
            {
                Children = { new DirectionalLight(Colors.White, new Vector3D(0, 0, -1)),
                                 new DirectionalLight(Colors.White, new Vector3D(0.1, -0.1, 1)),
                                 backModel }
            };

            // Non-interactive Visual3D consisting of the backside, and two lights.
            ModelVisual3D mv3d = new ModelVisual3D() { Content = m3dGroup };

            // Interactive frontside Visual3D
            Viewport2DVisual3D frontModel = new Viewport2DVisual3D() { Geometry = simpleQuad, Visual = _logicalChild, Material = frontMaterial, Transform = xfGroup };

            // Cache the brush in the VP2V3 by setting caching on it.  Big perf wins.
            SetCachingForObject(frontModel);

            // Scene consists of both the above Visual3D's.
            _viewport3d = new Viewport3D() { ClipToBounds = false, Children = { mv3d, frontModel } };

            UpdateRotation();

            return _viewport3d;
        }

        private void SetCachingForObject(DependencyObject d)
        {
            RenderOptions.SetCachingHint(d, CachingHint.Cache);
            RenderOptions.SetCacheInvalidationThresholdMinimum(d, 0.5);
            RenderOptions.SetCacheInvalidationThresholdMaximum(d, 2.0);
        }

        private void UpdateRotation()
        {
            Quaternion qx = new Quaternion(_xAxis, RotationX);
            Quaternion qy = new Quaternion(_yAxis, RotationY);
            Quaternion qz = new Quaternion(_zAxis, RotationZ);

            _quaternionRotation.Quaternion = qx * qy * qz;
        }

        private void Update3D()
        {
            // Use GetDescendantBounds for sizing and centering since DesiredSize includes layout whitespace, whereas GetDescendantBounds 
            // is tighter
            Rect logicalBounds = VisualTreeHelper.GetDescendantBounds(_logicalChild);
            double w = logicalBounds.Width;
            double h = logicalBounds.Height;

            // Create a camera that looks down -Z, with up as Y, and positioned right halfway in X and Y on the element, 
            // and back along Z the right distance based on the field-of-view is the same projected size as the 2D content
            // that it's looking at.  See http://blogs.msdn.com/greg_schechter/archive/2007/04/03/camera-construction-in-parallaxui.aspx
            // for derivation of this camera.
            double fovInRadians = FieldOfView * (Math.PI / 180);
            double zValue = w / Math.Tan(fovInRadians / 2) / 2;
            _viewport3d.Camera = new PerspectiveCamera(new Point3D(w / 2, h / 2, zValue),
                                                       -_zAxis,
                                                       _yAxis,
                                                       FieldOfView);


            _scaleTransform.ScaleX = w;
            _scaleTransform.ScaleY = h;
            _rotationTransform.CenterX = w / 2;
            _rotationTransform.CenterY = h / 2;
        }

        #endregion

        #region Private Classes

        /// <summary>
        /// Wrap this around a class that we want to catch the measure and arrange 
        /// processes occuring on, and propagate to the parent Planerator, if any.
        /// Do this because layout invalidations don't flow up out of a 
        /// Viewport2DVisual3D object.
        /// </summary>
        public class LayoutInvalidationCatcher : Decorator
        {
            /// <summary>
            /// Gets the Planerator parent.
            /// </summary>
            /// <value>
            /// The pla parent.
            /// </value>
            public Planerator PlaParent { get { return this.Parent as Planerator; } }

            /// <summary>
            /// Measures the child element of a <see cref="T:System.Windows.Controls.Decorator" /> to prepare for arranging it during the <see cref="M:System.Windows.Controls.Decorator.ArrangeOverride(System.Windows.Size)" /> pass.
            /// </summary>
            /// <param name="constraint">An upper limit <see cref="T:System.Windows.Size" /> that should not be exceeded.</param>
            /// <returns>
            /// The target <see cref="T:System.Windows.Size" /> of the element.
            /// </returns>
            protected override Size MeasureOverride(Size constraint)
            {
                Planerator pl = PlaParent;
                if (pl != null)
                {
                    pl.InvalidateMeasure();
                }
                return base.MeasureOverride(constraint);
            }

            /// <summary>
            /// Arranges the content of a <see cref="T:System.Windows.Controls.Decorator" /> element.
            /// </summary>
            /// <param name="arrangeSize">The <see cref="T:System.Windows.Size" /> this element uses to arrange its child content.</param>
            /// <returns>
            /// The <see cref="T:System.Windows.Size" /> that represents the arranged size of this <see cref="T:System.Windows.Controls.Decorator" /> element and its child.
            /// </returns>
            protected override Size ArrangeOverride(Size arrangeSize)
            {
                Planerator pl = PlaParent;
                if (pl != null)
                {
                    pl.InvalidateArrange();
                }
                return base.ArrangeOverride(arrangeSize);
            }
        }

        #endregion

        #region Private data

        // Instance data
        private FrameworkElement _logicalChild;
        private FrameworkElement _visualChild;
        private FrameworkElement _originalChild;

        private QuaternionRotation3D _quaternionRotation = new QuaternionRotation3D();
        private RotateTransform3D _rotationTransform = new RotateTransform3D();
        private Viewport3D _viewport3d;
        private ScaleTransform3D _scaleTransform = new ScaleTransform3D();

        // Static data
        static private readonly Point3D[] _mesh = new Point3D[] { new Point3D(0, 0, 0), new Point3D(0, 1, 0), new Point3D(1, 1, 0), new Point3D(1, 0, 0) };
        static private readonly Point[] _texCoords = new Point[] { new Point(0, 1), new Point(0, 0), new Point(1, 0), new Point(1, 1) };
        static private readonly int[] _indices = new int[] { 0, 2, 1, 0, 3, 2 };
        static private readonly Vector3D _xAxis = new Vector3D(1, 0, 0);
        static private readonly Vector3D _yAxis = new Vector3D(0, 1, 0);
        static private readonly Vector3D _zAxis = new Vector3D(0, 0, 1);

        #endregion
    }

    /// <summary>
    /// TiltBehavior which can be attached on FrameworkElements
    /// </summary>
    public class TiltBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        /// The keep dragging property
        /// </summary>
        public static readonly DependencyProperty KeepDraggingProperty =
            DependencyProperty.Register("KeepDragging", typeof(bool), typeof(TiltBehavior), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether to keep dragging.
        /// </summary>
        /// <value>
        ///   <c>true</c> if keep dragging is set; otherwise, <c>false</c>.
        /// </value>
        public bool KeepDragging
        {
            get { return (bool)GetValue(KeepDraggingProperty); }
            set { SetValue(KeepDraggingProperty, value); }
        }


        /// <summary>
        /// The tilt factor property
        /// </summary>
        public static readonly DependencyProperty TiltFactorProperty =
            DependencyProperty.Register("TiltFactor", typeof(Int32), typeof(TiltBehavior), new PropertyMetadata(20));


        /// <summary>
        /// Gets or sets the tilt factor.
        /// </summary>
        /// <value>
        /// The tilt factor.
        /// </value>
        public Int32 TiltFactor
        {
            get { return (Int32)GetValue(TiltFactorProperty); }
            set { SetValue(TiltFactorProperty, value); }
        }

        private FrameworkElement attachedElement;
        private Panel OriginalPanel;
        private Thickness OriginalMargin;
        private Size OriginalSize;
        /// <summary>
        /// Called when this behavior is attached.
        /// </summary>
        protected override void OnAttached()
        {
            attachedElement = this.AssociatedObject;

            if (attachedElement as Panel != null)
            {
                (attachedElement as Panel).Loaded += (sl, el) =>
                {
                    List<UIElement> elements = new List<UIElement>();

                    foreach (UIElement ui in (attachedElement as Panel).Children)
                    {
                        elements.Add(ui);
                    }
                    elements.ForEach((element) => Interaction.GetBehaviors(element).Add(new TiltBehavior() { KeepDragging = this.KeepDragging, TiltFactor = this.TiltFactor }));
                };

                return;
            }

            OriginalPanel = attachedElement.Parent as Panel;
            OriginalMargin = attachedElement.Margin;
            OriginalSize = new Size(attachedElement.Width, attachedElement.Height);
            double left = Canvas.GetLeft(attachedElement);
            double right = Canvas.GetRight(attachedElement);
            double top = Canvas.GetTop(attachedElement);
            double bottom = Canvas.GetBottom(attachedElement);
            int z = Canvas.GetZIndex(attachedElement);
            VerticalAlignment va = attachedElement.VerticalAlignment;
            HorizontalAlignment ha = attachedElement.HorizontalAlignment;

            #region Setting Container Properties
            RotatorParent = new Planerator();
            RotatorParent.Margin = OriginalMargin;
            RotatorParent.Width = OriginalSize.Width;
            RotatorParent.Height = OriginalSize.Height;
            RotatorParent.VerticalAlignment = va;
            RotatorParent.HorizontalAlignment = ha;
            RotatorParent.SetValue(Canvas.LeftProperty, left);
            RotatorParent.SetValue(Canvas.RightProperty, right);
            RotatorParent.SetValue(Canvas.TopProperty, top);
            RotatorParent.SetValue(Canvas.BottomProperty, bottom);
            RotatorParent.SetValue(Canvas.ZIndexProperty, z);
            #endregion

            #region Removing Child Properties
            OriginalPanel.Children.Remove(attachedElement);
            attachedElement.Margin = new Thickness();
            attachedElement.Width = double.NaN;
            attachedElement.Height = double.NaN;
            #endregion

            OriginalPanel.Children.Add(RotatorParent);
            RotatorParent.Child = attachedElement;

            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        /// <summary>
        /// Gets or sets the rotator parent.
        /// </summary>
        /// <value>
        /// The rotator parent.
        /// </value>
        public Planerator RotatorParent { get; set; }


        Point current = new Point(-99, -99);
        bool IsPressed = false;
        Int32 times = -1;
        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (KeepDragging)
            {
                current = Mouse.GetPosition(RotatorParent.Child);
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    if (current.X > 0 && current.X < (attachedElement as FrameworkElement).ActualWidth && current.Y > 0 && current.Y < (attachedElement as FrameworkElement).ActualHeight)
                    {
                        RotatorParent.RotationY = -1 * TiltFactor + current.X * 2 * TiltFactor / (attachedElement as FrameworkElement).ActualWidth;
                        RotatorParent.RotationX = -1 * TiltFactor + current.Y * 2 * TiltFactor / (attachedElement as FrameworkElement).ActualHeight;
                    }
                }
                else
                {
                    RotatorParent.RotationY = RotatorParent.RotationY - 5 < 0 ? 0 : RotatorParent.RotationY - 5;
                    RotatorParent.RotationX = RotatorParent.RotationX - 5 < 0 ? 0 : RotatorParent.RotationX - 5;
                }
            }
            else
            {

                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {

                    if (!IsPressed)
                    {
                        current = Mouse.GetPosition(RotatorParent.Child);
                        if (current.X > 0 && current.X < (attachedElement as FrameworkElement).ActualWidth && current.Y > 0 && current.Y < (attachedElement as FrameworkElement).ActualHeight)
                        {
                            RotatorParent.RotationY = -1 * TiltFactor + current.X * 2 * TiltFactor / (attachedElement as FrameworkElement).ActualWidth;
                            RotatorParent.RotationX = -1 * TiltFactor + current.Y * 2 * TiltFactor / (attachedElement as FrameworkElement).ActualHeight;
                        }
                        IsPressed = true;
                    }


                    if (IsPressed && times == 7)
                    {
                        RotatorParent.RotationY = RotatorParent.RotationY - 5 < 0 ? 0 : RotatorParent.RotationY - 5;
                        RotatorParent.RotationX = RotatorParent.RotationX - 5 < 0 ? 0 : RotatorParent.RotationX - 5;
                    }
                    else if (IsPressed && times < 7)
                    {
                        times++;
                    }
                }
                else
                {
                    IsPressed = false;
                    times = -1;
                    RotatorParent.RotationY = RotatorParent.RotationY - 5 < 0 ? 0 : RotatorParent.RotationY - 5;
                    RotatorParent.RotationX = RotatorParent.RotationX - 5 < 0 ? 0 : RotatorParent.RotationX - 5;
                }

            }
        }
    }
}
