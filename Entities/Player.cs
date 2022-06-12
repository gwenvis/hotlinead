using PadZex.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PadZex.Collision;
using PadZex.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using PadZex.Scripts.Particle;

namespace PadZex
{
	/// <summary>
	///  Player with static sprite and hp bar.
	///  WASD : Move
	/// </summary>
	public class Player : Entity ,IDamagable
	{
		private const int PARTICLE_AMOUNT = 250;
		
		public event Action DeadEvent;
		
		public Vector2 Middle => Position + SpriteSize / 2f;
		public Vector2 SpriteSize => new Vector2(playerSprite.Width, playerSprite.Height);
		public bool Dead { get; private set; }
		public bool HoldingWeapon { get; set; }
		
		private const float ACCEL_SPEED = 256 * 15f; 
		private const float MAX_SPEED = 256 * 5f;
		
		private Texture2D playerSprite;
		private Texture2D deathSprite;
		private Texture2D healthTexture;

		private Color color = Color.White;
		private float speed;
		private Entity sound;

		private Health health;
		private HealthBar healthBar;


		public Player()
		{

		}

		public override void Initialize(ContentManager content)
		{
			playerSprite = content.Load<Texture2D>("sprites/player");
			deathSprite = content.Load<Texture2D>("sprites/deadPlayer");
			healthTexture = content.Load<Texture2D>("RedPixel");
			health = new Health(3);
			healthBar = new HealthBar(healthTexture, health.HP, new Vector2(50 * Scale, -10), 10);

			health.HealthChangedEvent += healthBar.SetHealh;
			health.HasDiedEvent += Die;

			speed = 0;
			Depth = 5;
			Scale = 1f / (float)playerSprite.Width * 200f;
			
			AddTag("Player");
		}

		public override void Draw(SpriteBatch spriteBatch, Time time)
		{
			Texture2D sprite = Dead ? deathSprite : playerSprite;
			Draw(spriteBatch, sprite);
			healthBar.Draw(spriteBatch);
		}

		public override void Update(Time time)
		{
			if (Dead)
			{
				healthBar.UpdatePosition(Position);
				return;
			}
			
			float horizontal = -Convert.ToSingle(Input.KeyPressed(Keys.A)) + Convert.ToSingle(Input.KeyPressed(Keys.D));
			float vertical = -Convert.ToSingle(Input.KeyPressed(Keys.W)) + Convert.ToSingle(Input.KeyPressed(Keys.S));

			Vector2 move = new Vector2(horizontal, vertical);

			if (move != new Vector2(0, 0))
			{
				speed += ACCEL_SPEED * time.deltaTime;
				move = Vector2.Normalize(move);
			}
			else
			{
				speed = 0f;
			}

			speed = Math.Clamp(speed, 0, MAX_SPEED);
			float xVelocity = move.X * speed * time.deltaTime;
			float yVelocity = move.Y * speed * time.deltaTime;

			Position.X += xVelocity;
			CheckHorizontalCollision(xVelocity);

			Position.Y += yVelocity;
			CheckVerticalCollision(yVelocity);

			healthBar.UpdatePosition(Position);

			if (Input.KeyPressed(Keys.A))
			{
				FlipSprite();
			}
			else if (Input.KeyPressed(Keys.D))
			{
				UnFlipSprite();
			}
		}

		private void CheckHorizontalCollision(float velocity)
        {
            (bool collided, IEnumerable<Shape> shapes) = Scene.MainScene.TestAllCollision(Shape);
			if (collided)
			{
				var walls = shapes.Where(x => x.Owner.Tags.Contains("wall")).Cast<Collision.Rectangle>();
				var wall = walls.FirstOrDefault();
				if (wall != null)
				{
					if (velocity < 0) Position.X = wall.WorldX + wall.WorldWidth;
					else Position.X = wall.WorldX - ((Collision.Rectangle)Shape).WorldWidth;
				}
			}
        }
	
		private void CheckVerticalCollision(float velocity)
        {
            (bool collided, IEnumerable<Shape> shapes) = Scene.MainScene.TestAllCollision(Shape);
			if(collided)
            {
				var walls = shapes.Where(x  =>  x.Owner.Tags.Contains("wall")).Cast<Collision.Rectangle>();
				var wall = walls.FirstOrDefault();
				if  (wall == null) return;
				if (velocity < 0) Position.Y = wall.WorldY + wall.WorldHeight;
				else Position.Y = wall.WorldY - ((Collision.Rectangle)Shape).WorldHeight;
            }
        }

		public override Shape CreateShape()
		{
			var shape = new Collision.Rectangle(this, Vector2.Zero, new Vector2(playerSprite.Width, playerSprite.Height));
			shape.ShapeEnteredEvent += OnShapeEnteredEvent;
			return shape;
		}

		public override void OnDestroy()
		{
			Shape.ShapeEnteredEvent -= OnShapeEnteredEvent;
		}

		private void OnShapeEnteredEvent(Entity shape)
		{
			if (!shape.Tags.Contains("enemy")) return;
			Damage(shape, 1);
		}

		public void Damage(Entity entity, float damage = 0)
        {
			health.Hit(1);
			Sound.SoundPlayer.PlaySound(Sound.Sounds.PLAYER_HURT, this);
		}

		public void Reset()
		{
			health.Reset();
			Dead = false;
			HoldingWeapon = false;
		}

		private void Die()
		{
			if (Dead) return;
			
			DeadEvent?.Invoke();
			Dead = true;
			SpawnBlood();
		}
		
		private void SpawnBlood() 
		{
			for (int i = 0; i < PARTICLE_AMOUNT; i++)
			{
				BloodParticle particles = new(Middle, false, true);
				Scene.MainScene.AddEntity(particles);
			}
		}
    }
}
		
