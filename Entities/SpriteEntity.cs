using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PadZex.Core;

namespace PadZex.Entities
{
    public class SpriteEntity : Entity
    {
        public Texture2D Texture { get; private set; }
        private readonly string sprite;
        
        public SpriteEntity(string spritePath)
        {
            this.sprite = spritePath;
        }

        public SpriteEntity(string spritePath, ContentManager content)
        {
            this.sprite = spritePath;
            this.Initialize(content);
        }

        public SpriteEntity(Texture2D texture)
        {
            this.Texture = texture;
        }
        
        public override void Initialize(ContentManager content)
        {
            if(Texture == null) Texture = content.Load<Texture2D>(sprite);
        }

        public override void Update(Time time) { }

        public override void Draw(SpriteBatch spriteBatch, Time time)
        {
            Draw(spriteBatch, Texture);
        }

        /// <summary>
        /// Centers the origin of the entity to be in the middle.
        /// </summary>
        public void Center()
        {
            Origin = new Vector2((float)Texture.Width / 2, (float)Texture.Height / 2);
        }
    }
}