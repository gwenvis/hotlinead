using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PadZex.LevelLoader
{
    /// <summary>
    /// A tile is a single piece in the level with a position, a texture and an optional shape.
    /// </summary>
    public record Tile(Point GridPosition, Texture2D Texture, Collision.Shape Shape = null);
}
