using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using PadZex.Core;

namespace PadZex.Scripts.Particle
{
	class BloodParticle : Entity
	{
		private const float LIFE_TIME = 5.0f;
		
		private float particleSpeed;
		private float velocity;
		private Vector2 direction;
		private bool isTrail;
		private Texture2D particleSprite;
		private float lifeSpan = 0.0f;
		private bool infinite;
		
		public BloodParticle(Vector2 startPos, bool trail, bool infinite = false)
		{
			Position = startPos;
			isTrail = trail;
			this.infinite = infinite;
		}
		
		public override void Draw(SpriteBatch spriteBatch, Time time)
		{
			if (velocity > 0)
			{
				velocity -= time.deltaTime * 4;
				Position += direction * particleSpeed * velocity * time.deltaTime;

				if (!isTrail)
				{
					BloodParticle trail = new BloodParticle(Position, true, infinite);
					Scene.MainScene.AddEntity(trail);
				}

			}
			
			if (Scene.MainScene.Camera.IsInScreen(this)) {
				Draw(spriteBatch, particleSprite);
			}

			lifeSpan += time.deltaTime;
			if (!infinite && lifeSpan > LIFE_TIME)
			{
				DeleteEntity(this);
			}
		}

		public override void Initialize(ContentManager content)
		{
			Scale = 0.15f;
			particleSprite = content.Load<Texture2D>("sprites/weapons/blood_particle");
			Angle = CoreUtils.Random.Next(0, 1000);
			Alpha = (float)CoreUtils.Random.NextDouble();
			direction = new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle));

			if (!isTrail)
			{
				velocity = (float)CoreUtils.Random.NextDouble() + 0.2f;
				particleSpeed = CoreUtils.Random.Next(0, 800);			
				
			} else
			{
				velocity = 0.3f;
				particleSpeed = 50;
			}
		}

		public override void Update(Time time) { }
	}
}
