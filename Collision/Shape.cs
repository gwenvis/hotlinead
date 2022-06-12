using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PadZex.Core;
using System;
using System.Collections.Generic;

namespace PadZex.Collision
{
    /// <summary>
    /// <para>	/// Base shape class to be used with the QuadTree.
    /// Extend this class to make a new shape type.
    /// </para>
    ///
    /// See: <see cref="Rectangle"/> & <see cref="Circle"/>
    /// </summary>
    public abstract class Shape
    {
        private static Texture2D circleTexture;
        private static Texture2D rectangleTexture;

        public delegate void ShapeCollisionDelegate(PadZex.Core.Entity shape);

        /// <summary>
        /// Event fired when a shape enters this shape.
        /// </summary>
        public event ShapeCollisionDelegate ShapeEnteredEvent;

        /// <summary>
        /// Event fired when a shape exits this shape and was previously in it.
        /// </summary>
        public event ShapeCollisionDelegate ShapeExitedEvent;

        /// <summary>
        /// The entity that owns this shape.
        /// </summary>
        public PadZex.Core.Entity Owner { get; }

        /// <summary>
        /// The shapes that this shape is currently collided with.
        /// </summary>
        public IReadOnlyList<Shape> CollidedShapes => collidedShapes;

        private List<Shape> collidedShapes;

        protected Shape(PadZex.Core.Entity owner)
        {
            this.Owner = owner;
            collidedShapes = new List<Shape>();
        }

        public abstract bool CollideWithRect(Rectangle rect);
        public abstract bool CollideWithCircle(Circle circle);

        private void InvokeEnterCollision(PadZex.Core.Entity entity) => ShapeEnteredEvent?.Invoke(entity);
        private void InvokeExitCollision(PadZex.Core.Entity entity) => ShapeExitedEvent?.Invoke(entity);

        private void ShapeCollided(Shape shape)
        {
            collidedShapes.Add(shape);
            InvokeEnterCollision(shape.Owner);
        }

        private void ShapeExited(Shape shape)
        {
            collidedShapes.Remove(shape);
            InvokeExitCollision(shape.Owner);
        }

        private bool Collided(Shape shape) => shape switch
        {
            Rectangle rect => CollideWithRect(rect),
            Circle circle => CollideWithCircle(circle),
            _ => throw new NotImplementedException()
        };

        /// <summary>
        /// Collision of another shape against this shape and invokes events accordingly.
        /// </summary>
        /// <param name="shape">Shape to test against</param>
        internal void CheckCollision(Shape shape)
        {
            bool collided = Collided(shape);

            if (collided && !collidedShapes.Contains(shape))
            {
                ShapeCollided(shape);
            }
            else if (!collided && collidedShapes.Contains(shape))
            {
                ShapeExited(shape);
            }
        }

        public bool IsColliding(Shape shape) => Collided(shape);

        /// <summary>
        /// Loads the debug textures
        /// </summary>
        public static void LoadTextures(ContentManager contentManager)
        {
            circleTexture = contentManager.Load<Texture2D>("sprites/collision/circle");
            rectangleTexture = contentManager.Load<Texture2D>("sprites/collision/rectangle");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this is Rectangle re)
            {
                float width = 1f / rectangleTexture.Width * re.WorldWidth;
                float height = 1f / rectangleTexture.Height * re.WorldHeight;
                Vector2 rectScale = new Vector2(width, height);
                spriteBatch.Draw(rectangleTexture, re.WorldPosition, null, new Color(1f, 1f, 1f, 0.3f), 0, Vector2.Zero, rectScale, SpriteEffects.None, 100);
            }
            else if (this is Circle cir)
            {
                float scale = cir.Owner.Scale;
                float size = 1f / circleTexture.Width * cir.WorldRadius * 2;
                float x = cir.WorldX - cir.WorldRadius;
                float y = cir.WorldY - cir.WorldRadius;
                spriteBatch.Draw(circleTexture, new Vector2(x, y), null, new Color(1f, 1f, 1f, 0.3f), 0, Vector2.Zero, new Vector2(size), SpriteEffects.None, 100);
            }
        }
    }
}
