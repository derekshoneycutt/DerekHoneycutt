using System.Runtime.InteropServices;

namespace VagabondLib.Win32
{
    /// <summary>
    /// Class representing a Rectangle; Used appropriately for some Win32 API Calls
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        /// <summary>
        /// Gets or Sets the Left side of the RECT
        /// </summary>
        public int Left;
        /// <summary>
        /// Gets or Sets the Top of the RECT
        /// </summary>
        public int Top;
        /// <summary>
        /// Gets or Sets the Right side of the RECT
        /// </summary>
        public int Right;
        /// <summary>
        /// Gets or Sets the Bottom of the RECT
        /// </summary>
        public int Bottom;

        /// <summary>
        /// Initiate a new RECT object with given dimensions
        /// </summary>
        /// <param name="left">Left dimension</param>
        /// <param name="top">Top dimension</param>
        /// <param name="right">Right dimension</param>
        /// <param name="bottom">Bottom dimension</param>
        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        /// <summary>
        /// Initiate a new RECT object according to a Rectangle object
        /// </summary>
        /// <param name="r">Rectangle object to copy into the RECT</param>
        public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

        /// <summary>
        /// Gets or Sets the X position of the RECT
        /// </summary>
        public int X
        {
            get { return Left; }
            set { Right -= (Left - value); Left = value; }
        }

        /// <summary>
        /// Gets or Sets the Y position of the RECT
        /// </summary>
        public int Y
        {
            get { return Top; }
            set { Bottom -= (Top - value); Top = value; }
        }

        /// <summary>
        /// Gets or Sets the Height of the RECT
        /// </summary>
        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = value + Top; }
        }

        /// <summary>
        /// Gets or SEts the Width of the RECT
        /// </summary>
        public int Width
        {
            get { return Right - Left; }
            set { Right = value + Left; }
        }

        /// <summary>
        /// Gets or Sets the X and Y Positions of the RECT
        /// </summary>
        public System.Drawing.Point Location
        {
            get { return new System.Drawing.Point(Left, Top); }
            set { X = value.X; Y = value.Y; }
        }

        /// <summary>
        /// Gets or Sets the dimensions of the RECT
        /// </summary>
        public System.Drawing.Size Size
        {
            get { return new System.Drawing.Size(Width, Height); }
            set { Width = value.Width; Height = value.Height; }
        }

        /// <summary>
        /// Implicit conversion to a System.Drawing.Rectangle object
        /// </summary>
        /// <param name="r">RECT object to convert</param>
        /// <returns>new System.Drawing.Rectangle that is a copy of the RECT</returns>
        public static implicit operator System.Drawing.Rectangle(RECT r)
        {
            return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
        }

        /// <summary>
        /// Implic conversion of System.Drawing.Rectangle to RECT
        /// </summary>
        /// <param name="r">System.Drawing.Rectangle objec to convert</param>
        /// <returns>new RECT object copied from the Rectangle</returns>
        public static implicit operator RECT(System.Drawing.Rectangle r)
        {
            return new RECT(r);
        }

        /// <summary>
        /// Compare the equality of 2 RECT objects
        /// </summary>
        public static bool operator ==(RECT r1, RECT r2)
        {
            return r1.Equals(r2);
        }

        /// <summary>
        /// Compare the inequality of 2 RECT objects
        /// </summary>
        public static bool operator !=(RECT r1, RECT r2)
        {
            return !r1.Equals(r2);
        }

        /// <summary>
        /// Compare to another RECT for equality
        /// </summary>
        public bool Equals(RECT r)
        {
            return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
        }

        /// <summary>
        /// Compare to another RECT or System.Drawing.Rectangle for equality
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is RECT)
                return Equals((RECT)obj);
            else if (obj is System.Drawing.Rectangle)
                return Equals(new RECT((System.Drawing.Rectangle)obj));
            return false;
        }

        /// <summary>
        /// Get the Hash Code for the object
        /// </summary>
        public override int GetHashCode()
        {
            return ((System.Drawing.Rectangle)this).GetHashCode();
        }

        /// <summary>
        /// Get a String representation of the RECT
        /// </summary>
        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
        }
    }
}
