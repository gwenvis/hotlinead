using Microsoft.Xna.Framework;

namespace PadZex.Collision
{
    /// <summary>
    /// Circle shape for the <see cref="CollisionField"/>.
    /// </summary>
    public class Circle : Shape
    {
        public float X
        {
            get => Center.X;
            set => Center.X = value;
        }

        public float Y
        {
            get => Center.Y;
            set => Center.Y = value;
        }

        public float WorldX => X * Owner.Scale + Radius * Owner.Scale + Owner.Position.X;
        public float WorldY => Y * Owner.Scale + Radius * Owner.Scale + Owner.Position.Y;
        public float WorldRadius => Radius * Owner.Scale;

        public Vector2 Center;
        public float Radius;

        public Circle(PadZex.Core.Entity owner, Vector2 center, float radius) : base(owner)
        {
            Center = center;
            Radius = radius;
        }

        public override bool CollideWithRect(Rectangle rect) => CollisionUtils.CircleWithRectangle(this, rect);
        public override bool CollideWithCircle(Circle circle) => CollisionUtils.CircleWithCircle(this, circle);
    }
}
