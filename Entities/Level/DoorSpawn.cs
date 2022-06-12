using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PadZex.Core;
using Microsoft.Xna.Framework;
using System.Linq;

namespace PadZex.Entities.Level
{
    public class DoorSpawn : Entity
    {
        private enum DoorDirection { Horizontal = 0, Vertical = 2 }
        private DoorDirection doorDirection;
        private Door door;

        public DoorSpawn(LevelLoader.Level level, Point gridPos)
        {
            var tiles = level.Tiles.ToList();
            doorDirection =
                SpawnerUtils.IsSolid(tiles, level.Size, new Point(gridPos.X - 1, gridPos.Y),
                    new Point(gridPos.X + 1, gridPos.Y))
                    ? DoorDirection.Horizontal
                    : DoorDirection.Vertical;
        }

        public override void Draw(SpriteBatch spriteBatch, Time time)
        {
            
        }

        public override void Initialize(ContentManager content)
        {
            door = new Door((short)doorDirection)
            {
                Position = Position
            };
            Scene.MainScene.AddEntity(door);
        }

        public override void Update(Time time)
        {

        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Scene.MainScene.DeleteEntity(door);
        }
    }
}
