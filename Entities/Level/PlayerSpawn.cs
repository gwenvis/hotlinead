using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using PadZex.Core;
using Microsoft.Xna.Framework;

namespace PadZex.Entities.Level
{
    public class PlayerSpawn : Entity
    {
        public PlayerSpawn(LevelLoader.Level level, Point gridPos){ }

        public override void Draw(SpriteBatch spriteBatch, Time time)
        {

        }

        public override void Initialize(ContentManager content)
        {
            FindEntity("Player").Position = Position;
        }

        public override void Update(Time time)
        {

        }
    }
}
