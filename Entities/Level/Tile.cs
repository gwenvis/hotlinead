using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PadZex.Collision;
using PadZex.Core;

namespace PadZex.Entities.Level
{
    public class Tile : Entity
    {
        private const float TILE_SCALE = 1.0f;
        private LevelLoader.Tile tile;

        public Tile(LevelLoader.Tile tile)
        {
            this.tile = tile;
            Scale = TILE_SCALE;
            Position = new Microsoft.Xna.Framework.Vector2(
                tile.GridPosition.X * Scale * tile.Texture.Width, 
                tile.GridPosition.Y * Scale * tile.Texture.Width - (tile.Texture.Height - tile.Texture.Width));
            Depth = -1000 + tile.GridPosition.Y;
        }

        public override void Draw(SpriteBatch spriteBatch, Time time)
        {
            Draw(spriteBatch, tile.Texture);
        }

        public override void Initialize(ContentManager content)
        {

        }

        public override Shape CreateShape()
        {
            Rectangle rect = tile.Shape as Rectangle;

            if (rect == null) return base.CreateShape();

            AddTag("wall");
            return new Rectangle(this, rect.Position, rect.Size);
        }

        public override void Update(Time time) { }
    }
}
