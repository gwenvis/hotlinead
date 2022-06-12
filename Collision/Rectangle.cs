using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PadZex.Collision
{
    /// <summary>
    /// Rectangle shape for the <see cref="CollisionField"/>.
    /// </summary>
    public class Rectangle : Shape
    {
        public float X
        {
            get => Position.X;
            set => Position.X = value;
        }

        public float Y
        {
            get => Position.Y;
            set => Position.Y = value;
        }

        public float Width
        {
            get => Size.X;
            set => Size.X = value;
        }

        public float Height
        {
            get => Size.Y;
            set => Size.Y = value;
        }

        public float WorldX => X + Owner.Position.X;
        public float WorldY => Y + Owner.Position.Y;
        public float WorldWidth => Width * Owner.Scale;
        public float WorldHeight => Height * Owner.Scale;
        public Vector2 WorldPosition => Position + Owner.Position;
        public Vector2 WorldSize => new Vector2(WorldWidth, WorldHeight);

        public Vector2 Position;
        public Vector2 Size;

        public Rectangle(PadZex.Core.Entity owner, Vector2 position, Vector2 size) : base(owner)
        {
            Position = position;
            Size = size;
        }

        public Rectangle(PadZex.Core.Entity owner, float x, float y, float width, float height)
            : this(owner, new Vector2(x, y), new Vector2(width, height))
        {

        }

        public override bool CollideWithRect(Rectangle rect) => CollisionUtils.RectangleWithRectangle(this, rect);
        public override bool CollideWithCircle(Circle circle) => CollisionUtils.CircleWithRectangle(circle, this);
    }
}
