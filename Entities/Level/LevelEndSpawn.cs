using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using PadZex.Core;
using Microsoft.Xna.Framework;
using System.Linq;

namespace PadZex.Entities.Level
{
    public class LevelEndSpawn : Entity
    {
        private LevelEnd.Direction direction;
        private LevelEnd entity;

        public LevelEndSpawn(LevelLoader.Level level, Point gridPos)
        {
            var tiles = level.Tiles.ToList();

            direction = SpawnerUtils.IsSolid(tiles, level.Size, new Point(gridPos.X - 1, gridPos.Y), new Point(gridPos.X + 1, gridPos.Y)) ?
                LevelEnd .Direction.Horizontal : LevelEnd.Direction.Vertical; 
        }

        public override void Draw(SpriteBatch spriteBatch, Time time)
        {
        }

        public override void Initialize(ContentManager content)
        {
            entity = new LevelEnd(direction)
            {
                Position = Position
            };
            Scene.MainScene.AddEntity(entity);
        }

        public override void Update(Time time)
        {
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            DeleteEntity(entity);
        }
    }
}
