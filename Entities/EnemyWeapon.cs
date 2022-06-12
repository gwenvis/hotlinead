using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PadZex.Interfaces;
using System.Collections.Generic;
using System;
using System.Text;
using PadZex.Core;
using PadZex.Collision;
using PadZex.Scripts.Particle;
using PadZex.Scenes;
using System.Linq;
using Rectangle = PadZex.Collision.Rectangle;

namespace PadZex
{
    class EnemyWeapon : Entity
    {

        private const float WEAPON_FRICTION = 0.3f;
        private const float MAX_SPEED = 3000f;
        private const float MIN_DAMAGE_VELOCITY = 0.8f;
        private const float ROTATION_SPEED = 10f;

        public bool IsFlying { get; private set; }

        private Vector2 target;
        private Vector2 velocity;
        private Vector2 direction;
        private Entity player;
        private Texture2D weaponSprite;

        private float timeAlive;
        private int weaponDamage = 1;

        public EnemyWeapon() 
        {
            player = FindEntity("Player");
        }

        public void Reset(Vector2 startPosition)
        {
            Position = startPosition;
            target = player.Position;
            direction = target - startPosition;
            Angle = VectorToAngle(direction);
            direction.Normalize();
            IsFlying = true;
            timeAlive = 0;
            velocity = new Vector2(1, 1);
        }

        public override void Initialize(ContentManager content)
        {
            weaponSprite = content.Load<Texture2D>("sprites/enemyAttack");
            AddTag("EnemySword");
            
            velocity = new Vector2(1, 1);
            Scale = 0.3f;
        }

        public override Shape CreateShape()
        {
            var shape = new Collision.Circle(this, new Vector2((float)-weaponSprite.Width / 2), (float)weaponSprite.Width / 2);
            shape.ShapeEnteredEvent += CollisionEnter;
            return shape;
        }

        public override void Draw(SpriteBatch spriteBatch, Time time)
        {
            if (IsFlying) Draw(spriteBatch, weaponSprite);
        }  

        public override void Update(Time time)
        {
            if (velocity.LengthSquared() > 0)
            {
                velocity.X -= time.deltaTime;
                velocity.X = velocity.X < 0 ? 0 : velocity.X;
                velocity.Y -= time.deltaTime;
                velocity.Y = velocity.Y < 0 ? 0 : velocity.Y;

                Position.X += direction.X * MAX_SPEED * velocity.X * time.deltaTime;
                bool collided = HorizontalCollisionDetection(direction.X, velocity.X, time.timeSinceStart);
                Position.Y += direction.Y * MAX_SPEED * velocity.Y * time.deltaTime;
                collided = collided || VerticalCollisionDetection(direction.Y, velocity.Y, time.timeSinceStart);

                Angle += ROTATION_SPEED * time.deltaTime;
                if (collided)
                {
                    velocity.X -= WEAPON_FRICTION;
                    velocity.Y -= WEAPON_FRICTION;
                    if (velocity.X < 0) velocity.X = 0.0f;
                    if (velocity.Y < 0) velocity.Y = 0.0f;
                }                              
            }

            timeAlive += time.deltaTime;
            if (timeAlive > 2)
            {
                IsFlying = false;
            }
        }

        private float VectorToAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        private bool VerticalCollisionDetection(float direction, float velocity, float timeSinceStart)
        {
            if (velocity < float.Epsilon) return false;

            (bool collided, var shapes) = Scene.MainScene.TestAllCollision(Shape);

            foreach (var shape in shapes)
            {
                if (!shape.Owner.Tags.Contains("wall")) continue;
                var wall = (Rectangle)shape;

                if (Position.X < wall.WorldPosition.X ||
                    Position.X > wall.WorldPosition.X + wall.WorldWidth) continue;

                if (direction < 0) Position.Y = wall.WorldY + wall.WorldHeight + ((Collision.Circle)Shape).WorldRadius;
                else Position.Y = wall.WorldY - ((Collision.Circle)Shape).WorldRadius;
                this.velocity.Y = 0;
                return true;
            }
            return false;
        }

        private bool HorizontalCollisionDetection(float direction, float velocity, float timeSinceStart)
        {
            if (velocity < float.Epsilon) return false;

            (bool collided, var shapes) = Scene.MainScene.TestAllCollision(Shape);

            foreach (var shape in shapes)
            {
                if (!shape.Owner.Tags.Contains("wall")) continue;
                var wall = (Rectangle)shape;

                if (Position.Y < wall.WorldPosition.Y ||
                    Position.Y > wall.WorldPosition.Y + wall.WorldHeight) continue;

                if (direction < 0) Position.X = wall.WorldX + wall.WorldWidth + ((Collision.Circle)Shape).WorldRadius + 0.01f;
                else Position.X = wall.WorldX - ((Collision.Circle)Shape).WorldRadius;
                this.velocity.X = 0;
                return true;
            }
            return false;
        }

        private void CollisionEnter(Entity entity)
        {
            if (IsFlying && entity is Player or Door && velocity.Length() > MIN_DAMAGE_VELOCITY * MIN_DAMAGE_VELOCITY)
            {
                ((IDamagable) entity)?.Damage(this, weaponDamage);
            }
        }
    }
}
