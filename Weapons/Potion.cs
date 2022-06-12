using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PadZex.Collision;
using PadZex.Scripts.Particle;
using PadZex.Core;
using PadZex.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace PadZex.Weapons
{
	class Potion : Weapon
	{
		private const float RADIUS = 750.0f;
		private const int PARTICLE_AMOUNT = 150;
		
		private bool exploded;
		private Entity sound;

		public Potion()
		{
			WeaponDamage = 0;
			WeaponSpeed = 1200;
			RotationSpeed = 15f;
			Scale = 0.5f;
			Rotating = true;
			Offset = new Vector2(150, 100);
			AddTag("Potion");
			SpriteLocation = "sprites/weapons/potion";
		}

		public override void Update(Time time)
		{
			base.Update(time);
			KeyboardState state = Keyboard.GetState();

			float velocityLength = velocity.Length();
			if (throwing && velocityLength > 0.5f)
			{
				Scale += time.deltaTime;
			}
			else if (throwing && velocityLength < 0.5f && velocityLength > 0)
			{
				Scale -= time.deltaTime;
			}

			if (throwing && velocityLength <= 0 && !exploded)
			{
				Explode();
			}
		}

		private void Explode()
		{
			WeaponDamage = 3;

			(bool collided, IEnumerable<Shape> shapes) = Scene.MainScene.TestAllCollision(new Circle(this, new Vector2(-RADIUS / Scale * 0.5f), RADIUS / Scale));

			foreach (var shape in shapes)
			{
				if (collided && shape.Owner is IDamagable damagable)
				{
					damagable.Damage(shape.Owner, WeaponDamage);
				}

			}

			var particles = new PotionParticle[PARTICLE_AMOUNT];

			for (int i = 0; i < PARTICLE_AMOUNT; i++)
			{
				particles[i] = new PotionParticle(Position);
				Scene.MainScene.AddEntity(particles[i]);
			}

			Sound.SoundPlayer.PlaySound(Sound.Sounds.POTION_IMPACT, this);
			exploded = true;
			Scene.MainScene.DeleteEntity(this);
		}
	}
}
