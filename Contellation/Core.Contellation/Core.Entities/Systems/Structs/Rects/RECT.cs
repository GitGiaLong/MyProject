﻿using System.Runtime.InteropServices;
using Core.Entities.Systems.Structs.Points;

namespace Core.Entities.Systems.Structs.Rects
{
    /// <summary>
    /// The RECT structure defines a rectangle by the coordinates of its upper-left and lower-right corners.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        private int _left;
        private int _top;
        private int _right;
        private int _bottom;

        /// <summary>
        /// Gets or sets the x-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int Left
        {
            readonly get { return _left; }
            set { _left = value; }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int Right
        {
            readonly get { return _right; }
            set { _right = value; }
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int Top
        {
            readonly get { return _top; }
            set { _top = value; }
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int Bottom
        {
            readonly get { return _bottom; }
            set { _bottom = value; }
        }

        /// <summary>
        /// Gets the width of the rectangle.
        /// </summary>
        public readonly int Width
        {
            get { return _right - _left; }
        }

        /// <summary>
        /// Gets the height of the rectangle.
        /// </summary>
        public readonly int Height
        {
            get { return _bottom - _top; }
        }

        /// <summary>
        /// Gets the position of the rectangle.
        /// </summary>
        public POINT Position
        {
            get { return new POINT { x = _left, y = _top }; }
        }

        /// <summary>
        /// Gets the size of the rectangle.
        /// </summary>
        public SIZE Size
        {
            get { return new SIZE { cx = Width, cy = Height }; }
        }

        /// <summary>
        /// Sets offset of the rectangle.
        /// </summary>
        public void Offset(int dx, int dy)
        {
            _left += dx;
            _top += dy;
            _right += dx;
            _bottom += dy;
        }

        /// <summary>
        /// Combines two RECTs.
        /// </summary>
        public static RECT Union(RECT rect1, RECT rect2)
        {
            return new RECT
            {
                Left = Math.Min(rect1.Left, rect2.Left),
                Top = Math.Min(rect1.Top, rect2.Top),
                Right = Math.Max(rect1.Right, rect2.Right),
                Bottom = Math.Max(rect1.Bottom, rect2.Bottom),
            };
        }

        /// <inheritdoc />
        public override readonly bool Equals(object? obj)
        {
            if (obj is not RECT)
            {
                return false;
            }

            try
            {
                var rc = (RECT)obj;

                return rc._bottom == _bottom && rc._left == _left && rc._right == _right && rc._top == _top;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public override readonly int GetHashCode()
        {
            return _top.GetHashCode() ^ _bottom.GetHashCode() ^ _left.GetHashCode() ^ _right.GetHashCode();
        }

        public static bool operator ==(RECT left, RECT right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RECT left, RECT right)
        {
            return !(left == right);
        }
    }
}
