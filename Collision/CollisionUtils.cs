using System;
using System.Collections.Generic;
using System.Text;

namespace PadZex.Collision
{
    /// <summary>
    /// Useful collision functions
    /// </summary>
    public static class CollisionUtils
    {
        public static bool CircleWithRectangle(Circle circle, Rectangle rect)
        {
            float testX = circle.WorldX;
            float testY = circle.WorldY;

            if (circle.WorldX < rect.WorldX)
                testX = rect.WorldX;
            else if (circle.WorldX > rect.WorldX + rect.WorldWidth)
                testX = rect.WorldX + rect.WorldWidth;
            if (circle.WorldY < rect.WorldY)
                testY = rect.WorldY;
            else if (circle.WorldY > rect.WorldY + rect.WorldHeight)
                testY = rect.WorldY + rect.WorldHeight;

            float distX = circle.WorldX - testX;
            float distY = circle.WorldY - testY;
            float distance = distX * distX + distY * distY;

            return distance <= circle.WorldRadius * circle.WorldRadius;
        }

        public static bool RectangleWithRectangle(Rectangle rect1, Rectangle rect2)
        {
            return rect1.WorldX < rect2.WorldX + rect2.WorldWidth
                && rect1.WorldX + rect1.WorldWidth > rect2.WorldX
                && rect1.WorldY < rect2.WorldY + rect2.WorldHeight
                && rect1.WorldY + rect1.WorldHeight > rect2.WorldY;
        }

        public static bool CircleWithCircle(Circle circle1, Circle circle2)
        {
            float x = circle2.WorldX - circle1.WorldX;
            float y = circle2.WorldY - circle1.WorldY;
            float r = circle2.WorldRadius + circle1.WorldRadius;

            return x * x + y * y <= r * r;
        }
    }
}
