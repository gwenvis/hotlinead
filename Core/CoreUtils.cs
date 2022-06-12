using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PadZex.Core
{
    public static class CoreUtils
    { 
        public static System.Random Random { get; set; } 
        public static GraphicsDevice GraphicsDevice { get; set; }

        public static Point ScreenSize { get; set; }

        static CoreUtils()
        {
            Random = new System.Random();
        }

        public static bool PointRectangleCollision(Vector2 point, Rectangle rect)
        {
            return point.X > rect.X && point.X < rect.X + rect.Width
                && point.Y > rect.Y && point.Y < rect.Y + rect.Height;
        }
    }
}
