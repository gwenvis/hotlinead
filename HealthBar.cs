using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Universele healthbar class. 
namespace PadZex
{
    public class HealthBar
    {
        private const int WIDTH = 100;
    
        private int health;
        private readonly int startingHealth;
        private Vector2 position;

        private Rectangle healthBar;

        private Texture2D texture;
        private Vector2 offset;
        private int thickness;

        public HealthBar(Texture2D texture, int startingHealth, Vector2 offset, int thickness)
        {
            this.texture = texture;
            this.startingHealth = startingHealth;
            this.health = startingHealth;
            this.offset = offset;
            this.thickness = thickness;

            healthBar = new Rectangle(0, 0, 0, 0);
        }

        public void SetHealh(int health)
        {
            this.health = health;
        }

        public void UpdatePosition(Vector2 position)
        {
            this.position.X = position.X;
            this.position.Y = position.Y;

            Update();
        }

        private void Update()
        {
            int width = WIDTH;
            int healthDif = startingHealth - health;
            if (healthDif is > 0) width = WIDTH - (WIDTH / startingHealth * (startingHealth - health));
            healthBar = new Rectangle((int)position.X - startingHealth + (int)offset.X, (int)position.Y + (int)offset.Y, width, thickness);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, healthBar, Color.White);
        }
    }
}
