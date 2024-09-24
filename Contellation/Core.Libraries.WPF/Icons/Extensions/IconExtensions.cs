using Core.Entities.Enums.Placements;
using System.Drawing;

namespace Core.Libraries.WPF.Icons.Extensions
{
    internal static class IconExtensions
    {
        /// <summary>
        /// Rotate a graphics around its center for the specified angle in degrees.
        /// </summary>
        /// <param Name="graphics">The graphics to rotate</param>
        /// <param Name="rotation">Angle of rotation in degrees</param>
        /// <param Name="width">The graphics width in pixels (for center calculation)</param>
        /// <param Name="height">The graphics height in pixels (for center calculation)</param>
        public static void Rotate(this Graphics graphics, double rotation, int width, int height)
        {
            if (Math.Abs(rotation) < 0.5) return;
            float mx = .5f * width, my = .5f * height;
            graphics.TranslateTransform(mx, my);
            graphics.RotateTransform((float)rotation);
            graphics.TranslateTransform(-mx, -my);
        }

        /// <summary>
        /// Flip a graphics
        /// </summary>
        /// <param Name="graphics">The graphics to flip</param>
        /// <param Name="flip">The flip value</param>
        /// <param Name="width">The graphics width in pixels (for center calculation)</param>
        /// <param Name="height">The graphics height in pixels (for center calculation)</param>
        public static void Flip(this Graphics graphics, OrientationPlacement flip, int width, int height)
        {
            switch (flip)
            {
                case OrientationPlacement.Horizontal:
                    graphics.ScaleTransform(-1f, 1f);
                    graphics.TranslateTransform(-width, 0f);
                    break;

                case OrientationPlacement.Vertical:
                    graphics.ScaleTransform(1f, -1f);
                    graphics.TranslateTransform(0f, -height);
                    break;
                case OrientationPlacement.Normal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(flip), flip, null);
            }
        }

        /// <summary>
        /// Flips an image
        /// </summary>
        /// <param Name="image">The image to flip</param>
        /// <param Name="flip">The flip value</param>
        public static void Flip(this System.Drawing.Image image, OrientationPlacement flip)
        {
            var rotateFlip = flip.ToRotateFlip();
            if (rotateFlip == RotateFlipType.RotateNoneFlipNone) return;
            image.RotateFlip(rotateFlip);
        }

        private static RotateFlipType ToRotateFlip(this OrientationPlacement flip)
        {
            return flip switch
            {
                OrientationPlacement.Horizontal => RotateFlipType.RotateNoneFlipX,
                OrientationPlacement.Vertical => RotateFlipType.RotateNoneFlipY,
                _ => RotateFlipType.RotateNoneFlipNone
            };
        }
    }
}
