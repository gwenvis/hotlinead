using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PadZex.Core;

namespace PadZex.Entities.MainMenu
{
    public class Button : SpriteEntity
    {
        private Action pressCallback;

        private Camera camera;
        private Color normalColor;
        private Color hoverColor;

        public Button(string spriteName, ContentManager content, Color normalColor, Color hoverColor, Action pressCallback) : base (spriteName, content)
        {
            this.pressCallback = pressCallback;
            this.normalColor = normalColor;
            this.hoverColor = hoverColor;
        }
        
        public override void Initialize(ContentManager content)
        {
            camera = FindEntity<Camera>("Camera");
            base.Initialize(content);
        }

        public override void Update(Time time)
        {
            camera ??= FindEntity<Camera>("Camera");
            Rectangle rect = new(Position.ToPoint() - (Origin * Scale).ToPoint(), new Vector2(Texture.Width * Scale, Texture.Height * Scale).ToPoint());

            if (CoreUtils.PointRectangleCollision(camera.MousePosition, rect))
            {
                Color = hoverColor;

                if (Input.MouseLeftPressed)
                {
                    pressCallback();
                }
            }
            else Color = normalColor;
            
            base.Update(time);
        }
    }
}