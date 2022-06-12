using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using PadZex.Core;
using Microsoft.Xna.Framework;
using PadZex.Scenes;

namespace PadZex.Entities.Level
{
    public class EnemySpawn : Entity
    {
        private Enemy enemy;

        public EnemySpawn(LevelLoader.Level level, Point gridPos) { }

        public override void Draw(SpriteBatch spriteBatch, Time time)
        {

        }

        public override void Initialize(ContentManager content)
        {
            enemy = new Enemy
            {
                Position = Position
            };
            Scene.MainScene.AddEntity(enemy);

            var playScene = Scene.MainScene as PlayScene;
            playScene.EnemyCount++;
        }

        public override void Update(Time time)
        {

        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Scene.MainScene.DeleteEntity(enemy);
        }
    }
}
