using Microsoft.Xna.Framework;

namespace PadZex.LevelLoader
{
    public record LevelEntity(System.Type EntityType, Point GridPosition);
}
