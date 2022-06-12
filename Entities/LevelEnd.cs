using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PadZex.Collision;
using PadZex.Core;
using PadZex.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadZex.Entities
{
    public class LevelEnd : Entity
    {
        public enum Direction { Horizontal, Vertical };
        private Texture2D texture;
        private PlayScene playScene;
		private Player player;

        public LevelEnd(Direction direction)
        {
            AddTag("wall");
			player = (Player)FindEntity("Player");
		}

        public override void Draw(SpriteBatch spriteBatch, Time time)
        {
            Draw(spriteBatch, texture);
        }

        public override Shape CreateShape()
        {
            Shape shape = new Collision.Rectangle(this, Vector2.Zero, new Vector2(texture.Width, texture.Height));
            shape.ShapeEnteredEvent += OnShapeEnteredEvent;
            return shape;
        }

        private void OnShapeEnteredEvent(Entity shape)
        {
            if (Tags.Contains("wall")) return;

            if (shape.Tags.Contains("Player"))
            {
                PlayScene playScene = Scene.MainScene as PlayScene;
                if (playScene == null) return;
				player.HoldingWeapon = false;
                playScene.LoadNextLevel();
            }
        }

        public override void Initialize(ContentManager content)
        {
            texture = content.Load<Texture2D>("sprites/door/LevelEndState0");
            playScene = Scene.MainScene as PlayScene;
        }

        public override void OnDestroy()
        {
            if(Shape != null) Shape.ShapeEnteredEvent -= OnShapeEnteredEvent;
        }

        public override void Update(Time time) 
        {
            if (playScene.EnemyCount == 0 && Tags.Contains("wall")) RemoveTag("wall");
        }
    }
}
