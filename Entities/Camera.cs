using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Specialized;
using PadZex.Core;

namespace PadZex
{
    public class Camera : Entity
    {
        
        public Matrix Transform
        {
            get { return transform; }
        }

		public Vector2 offset;

        private Matrix transform;
        
        private Viewport viewport;

        private float zoom = 0.42f;
        private Entity target;

        public float X
        {
            get => Position.X;
            set => Position.X = value;
        }

        public float Y
        {
            get => Position.Y;
            set => Position.Y = value;
        }

        public float Zoom
        {
            get => zoom;
            set
            {
                zoom = value;
                if (zoom < 0.1f)
                    zoom = 0.1f;
            }
        }

        public float Rotation
        {
            get => Angle;
            set => Angle = value;
        }

        public Vector2 MousePosition => Input.MousePosition.ToVector2() / Zoom + Position;
		public Vector2 GlobalPosition => Position;
		
		public Camera(Viewport newViewport)
        {
            viewport = newViewport;
        }

        public void SelectTarget(String targetString, Scene scene, Vector2 origin = default)
        {
            target = scene.FindEntity(targetString);
            Origin = CoreUtils.ScreenSize.ToVector2() / 2 + origin;
        }

        public override void Initialize(ContentManager content)
        {
            AddTag("Camera");
        }

        public override void Update(Time time)
        {
			if (target != null) Position = new Vector2(target.Position.X, target.Position.Y) + offset - Origin / Zoom;
 			transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
													Matrix.CreateRotationZ(Rotation) *
													Matrix.CreateScale(new Vector3(Zoom, Zoom, 0));
        }

        public override void Draw(SpriteBatch spriteBatch, Time time)
        {

        }

		public bool IsInScreen(Entity obj) =>
            obj.Position.X < GlobalPosition.X + CoreUtils.ScreenSize.X / Zoom &&
            obj.Position.X > GlobalPosition.X &&
            obj.Position.Y < GlobalPosition.Y + CoreUtils.ScreenSize.Y / Zoom &&
            obj.Position.Y > GlobalPosition.Y;
    }
}
