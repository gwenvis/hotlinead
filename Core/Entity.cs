using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PadZex.Core
{
    /// <summary>
    /// Base functionality for any entity that is drawn on screen.
    /// </summary>
    public abstract class Entity
    {
        public Vector2 Position = Vector2.Zero;
        public Vector2 Origin = default;
        public Color Color = Microsoft.Xna.Framework.Color.White;
        public float Angle = .0f;
        public float Scale = 1.0f;
		public float Alpha = 1;
        public int Depth = 0;
        
        public IEnumerable<string> Tags => tags;

        /// <summary>
        /// Shape assigned to this Entity. Can be null.
        /// </summary>
        public Collision.Shape Shape { get; private set; }

        protected facing oldFacing = facing.left;

        private SpriteEffects effect = SpriteEffects.None;
        private List<string> tags = new List<string>();


        public abstract void Initialize(ContentManager content);
        public abstract void Update(Time time);
        public abstract void Draw(SpriteBatch spriteBatch, Time time);
        public virtual void OnDestroy() { }

        /// <summary>
        /// <summary>
        /// Look for an entity in the Active scene and return it
        /// </summary>
        /// <param name="tag">Tag to look for</param>
        /// <returns>an Entity in the scene. Null if not found.</returns>
        protected static Entity FindEntity(string tag) => Scene.MainScene?.FindEntity(tag);

        /// <summary>
        /// <summary>
        /// Look for an entity in the Active scene and return it
        /// </summary>
        /// <param name="tag">Tag to look for</param>
        /// <returns>an Entity in the scene. Null if not found.</returns>
        protected static T FindEntity<T>(string tag) where T : Entity => Scene.MainScene?.FindEntity(tag) as T;

        protected static T FindEntity<T>() where T : Entity => Scene.MainScene?.FindEntity<T>();

        /// Create an entity and apply tags on it.
        /// </summary>
        /// <param name="tags">Tags to apply on the entity.</param>
        public Entity(params string[] tags)
        {
            this.tags.AddRange(tags);
        }
        public Entity() { }
        public Entity(Texture2D texture) { }

        /// <summary>
        /// Draw the texture to the screen with the entities attributes.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, Position, null,
                Color * Alpha, Angle, Origin,
                Scale, effect, Depth);
        }
        /// Deletes an entity the next frame in the active scene.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        public static void DeleteEntity(Entity entity)
        {
            Scene.MainScene?.DeleteEntity(entity);
        }

        /// <summary>
        /// Extend this to add a shape to this entity.
        /// Return it so it's added to the collision field properly.
        /// </summary>
        /// <returns>a newly created shape or null.
        /// A <see cref="Scene"/> will add this to its collision field</returns>
        public virtual Collision.Shape CreateShape() { return null; }

        public void FlipSprite()
        {
            effect = SpriteEffects.FlipHorizontally;
        }

        public void UnFlipSprite()
        {
            effect = SpriteEffects.None;
        }

        /// <summary>
        /// Add a tag to the Entity
        /// </summary>
        public void AddTag(string tag)
        {
            tags.Add(tag);
        }

        /// <summary>
        /// Remove an existing tag from the entity.
        /// </summary>
        public void RemoveTag(string tag)
        {
            tags.Remove(tag);
        }

        public PadZex.Collision.Shape InitializeShape()
        {
            Shape = CreateShape();
            return Shape;
        }
    }
}

public enum facing
{
    left,
    right
}