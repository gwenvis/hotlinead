using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PadZex.Interfaces;
using PadZex.Collision;
using PadZex.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Rectangle = PadZex.Collision.Rectangle;
using PadZex.Sound;
using PadZex.Entities;

namespace PadZex.Weapons
{
	/// <summary>
	/// Base class for all weapons used by the player
	/// </summary>
	public class Weapon : Entity
	{
		/// <summary>
		/// collision detection will do nothing in this timeframe
		/// </summary>
		private const float THROW_TIMEFRAME = 0.05f;
		private const bool DRAW_SHAPE = false;
		private const float WEAPON_FRICTION = 0.3f;
		
		/// <summary>
		/// Weapon settings set in the sub classes
		/// </summary>

		public int WeaponDamage { get; set; }
		public float WeaponSpeed { get; set; }
		public float RotationSpeed { get; set; }
		public string SpriteLocation { get; set; }
		public bool Rotating { get; set; }
		public bool isFlipped { get; set; }
		public Vector2 Offset { get; set; }

		public bool throwing;
		public Vector2 velocity = Vector2.Zero;
		private Vector2 direction;
		public bool pickedUp, collidingWithPlayer = false;
		private Texture2D weaponSprite;
		private Player player;
		private Camera camera;
		
		// throw timing
		private float throwTime = 0;

		public override void Initialize(ContentManager content)
		{
			weaponSprite = content.Load<Texture2D>(SpriteLocation);
			player = (Player)FindEntity("Player");
			camera = FindEntity<Camera>("Camera");
			Origin = new Vector2(weaponSprite.Width / 2, weaponSprite.Height / 2);

			if (isFlipped)
			{
				FlipSprite();
			}

		}

		public override Shape CreateShape()
		{
			var shape = new Collision.Circle(this, new Vector2((float)-weaponSprite.Width / 2), (float)weaponSprite.Width / 2);
			shape.ShapeEnteredEvent += CollisionEnter;
			shape.ShapeExitedEvent += CollisionExit;
			return shape;
		}

		/// <summary>
		/// Funtion to throw your weapon to the mouse position
		/// </summary>
		public virtual void ThrowWeapon(Time time)
		{
			velocity = new Vector2(1, 1);
			Vector2 mousePos = camera.MousePosition;
			direction = mousePos - player.Position;
			Angle = VectorToAngle(direction);
			direction.Normalize();
			throwing = true;
			pickedUp = false;
			player.HoldingWeapon = false;
			throwTime = time.timeSinceStart;
			SoundPlayer.PlaySound(Sounds.THROW, this);
		}

		public override void Draw(SpriteBatch spriteBatch, Time time)
		{
			Draw(spriteBatch, weaponSprite);
			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if(DRAW_SHAPE) Shape?.Draw(spriteBatch);
		}

		public override void Update(Time time)
		{
			//Weapon is attached to player if picked up
			if (!throwing && pickedUp)
			{
				Position = player.Middle;// + Offset;
			}

			//If set, rotates the weapon and moves it to the destination
			if (throwing)
			{
				float velocityLength = velocity.Length();
				if (Rotating && velocityLength > 0)
				{
					Angle += RotationSpeed * velocityLength * time.deltaTime;
				} else
				{
					//Angle = VectorToAngle(direction);
				}

				if (velocity.LengthSquared() > 0)
				{
					velocity.X -= time.deltaTime;
					velocity.X = velocity.X < 0 ? 0 : velocity.X;
					velocity.Y -= time.deltaTime;
					velocity.Y = velocity.Y < 0 ? 0 : velocity.Y;
					
					Position.X += direction.X * WeaponSpeed * velocity.X * time.deltaTime;
					bool collided = HorizontalCollisionDetection(direction.X, velocity.X, time.timeSinceStart);
					Position.Y += direction.Y * WeaponSpeed * velocity.Y * time.deltaTime;
					collided = collided || VerticalCollisionDetection(direction.Y, velocity.Y, time.timeSinceStart);

					if (collided)
					{
						velocity.X -= WEAPON_FRICTION ;
						velocity.Y -= WEAPON_FRICTION ;
						if (velocity.X < 0) velocity.X = 0.0f;
						if (velocity.Y < 0) velocity.Y = 0.0f;
					}
				}

				if (velocity.Length() <= 0.001f && this is not Potion)
				{
					throwing = false;
				}

			}

			//Throws the weapon
			if (Input.MouseLeftFramePressed && pickedUp)
			{
				ThrowWeapon(time);
			}

			//Picks op weapon if colliding with 'F'
			if (collidingWithPlayer && !throwing)
			{
				PickUp();
			}
		}

		private bool VerticalCollisionDetection(float direction, float velocity, float timeSinceStart)
		{
			bool InTimeFrame() => timeSinceStart < throwTime + THROW_TIMEFRAME;

			if (velocity < float.Epsilon) return false;
			
			(bool collided, var shapes) = Scene.MainScene.TestAllCollision(Shape);
			if (!collided || this is Potion || InTimeFrame()) return false;

			foreach (var shape in shapes)
			{
				if (!shape.Owner.Tags.Contains("wall")) continue;
				var wall = (Rectangle)shape;

				if (Position.X < wall.WorldPosition.X ||
				    Position.X > wall.WorldPosition.X + wall.WorldWidth) continue;

				if (direction < 0) Position.Y = wall.WorldY + wall.WorldHeight + ((Collision.Circle) Shape).WorldRadius;
				else Position.Y = wall.WorldY - ((Collision.Circle) Shape).WorldRadius;
				this.velocity.Y = 0;
				return true;
			}
			return false;
		}
		
		private bool HorizontalCollisionDetection(float direction, float velocity, float timeSinceStart)
		{
			bool InTimeFrame() => timeSinceStart < throwTime + THROW_TIMEFRAME;
			
			if (velocity < float.Epsilon) return false;
			
			(bool collided, var shapes) = Scene.MainScene.TestAllCollision(Shape);
			if (!collided || this is Potion || InTimeFrame()) return false;

			foreach (var shape in shapes)
			{
				if (!shape.Owner.Tags.Contains("wall")) continue;
				var wall = (Rectangle)shape;
				
				if (Position.Y < wall.WorldPosition.Y ||
				    Position.Y > wall.WorldPosition.Y + wall.WorldHeight) continue;

				if (direction < 0) Position.X = wall.WorldX + wall.WorldWidth + ((Collision.Circle)Shape).WorldRadius + 0.01f;
				else Position.X = wall.WorldX - ((Collision.Circle) Shape).WorldRadius;
				this.velocity.X = 0;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Converts a vector to angle used for throwing the weapon in a straight line
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		private float VectorToAngle(Vector2 vector)
		{
			return (float)Math.Atan2(vector.Y, vector.X);
		}

		/// <summary>
		/// Picks up the weapon
		/// </summary>
		public void PickUp()
		{
			if (!player.HoldingWeapon)
			{
				Angle = 0;
				pickedUp = true;
				throwing = false;
				player.HoldingWeapon = true;
			}

		}

		/// <summary>
		/// Triggers on collision enter event
		/// Damages any IDamagable and tracks if player makes collison for PickUp()
		/// </summary>
		/// <param name="entity"></param>
		private void CollisionEnter(Entity entity)
		{
			if (throwing)
			{
				if (entity is IDamagable damagable)
				{
					damagable.Damage(this, WeaponDamage);
					HitStun.Add();
					ScreenShake.Shake();
				}
			}

			if (entity is Player) collidingWithPlayer = true;

		}

		/// <summary>
		/// Triggers on colision exit event
		/// Sets player collision to false
		/// </summary>
		/// <param name="entity"></param>
		private void CollisionExit(Entity entity)
		{
			if (entity is Player) collidingWithPlayer = false;
		}

	}
}
