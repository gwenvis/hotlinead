using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PadZex.Collision;
using PadZex.Interfaces;
using PadZex.Core;
using System.Linq;

namespace PadZex
{
    class Door : Entity, IDamagable
    {
        private short doorState, startState;
        private int health;
        private bool opening;
        private Texture2D[] doorStateSprites = new Texture2D[3];
        private Color color = Color.White;
        private Entity sound;

        public Door(short startState)
        {
            this.startState = startState;
        }

        public override void Initialize(ContentManager content)
        {
            AddTag("Door");
            doorState = startState;
            doorStateSprites[0] = content.Load<Texture2D>("sprites/door/doorState1"); //Doorstate left/right closed
            doorStateSprites[1] = content.Load<Texture2D>("sprites/door/doorState2"); //Doorstate in between (currently not in use)
            doorStateSprites[2] = content.Load<Texture2D>("sprites/door/doorState3"); //Doorstate down/up closed
            opening = startState == 0;
            Scale = 1;

            health = 1;
        }

        public override void Draw(SpriteBatch spriteBatch, Time time)
        {
            spriteBatch.Draw(doorStateSprites[doorState], Position, null, color, Angle, Origin, Scale, SpriteEffects.None, Depth);
        }

        public override void Update(Time time)
        {

        }
        public override Shape CreateShape()
        {
            var shape = new Collision.Rectangle(this, Vector2.Zero, new Vector2(doorStateSprites[doorState].Width, doorStateSprites[doorState].Height));
            shape.ShapeEnteredEvent += OnShapeEnteredEvent;
            return shape;
        }

        private void OnShapeEnteredEvent(Entity shape)
        {
            //Old way of opening, has interaction regardless of where the player is compared to the door (below/above it vs. left/right of it)
            /*if (opening)
              {
                  if (doorState < 2) doorState+=2;
                  if (doorState == 2) opening = false;
              }
              else 
              {
                  if (doorState > 0) doorState-=2;
                  if (doorState == 0) opening = true;
              }*/

            if (!shape.Tags.Contains("Player")) return;
            
            //Only changes doorstate if the player is on the right side of the door
            if((shape.Position.X < Position.X || shape.Position.X > Position.X + doorStateSprites[0].Width) && !opening) 
            { 
                doorState -= 2;
                opening = true;
            } 
            else if ((shape.Position.Y < Position.Y || shape.Position.Y > Position.Y + doorStateSprites[2].Height) && opening)
            {
                doorState += 2;
                opening = false;
            }
        }

        public void Damage(Entity entity, float damage = 10)
        {
            Sound.SoundPlayer.PlaySound(Sound.Sounds.DOOR_BREAK, this);
            health -= (int)damage;
            if (health <= 0) Entity.DeleteEntity(this);
        }
    }
}
