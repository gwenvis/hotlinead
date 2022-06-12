using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PadZex.Core;

namespace PadZex.Entities.Sounds
{
    public class AudioListenerBind : Entity
    {
        private readonly Entity target;
        private readonly AudioListener audioListener;

        public AudioListenerBind(Entity entity, AudioListener audioListener)
        {
            target = entity;
            audioListener.Position = new Vector3(Position, 0);
            this.audioListener = audioListener;
        }

        
        public override void Update(Time time)
        {
            Vector2 velocity = target.Position - Position;
            
            Position = target.Position;
            audioListener.Position = new Vector3(Position, 0);
            audioListener.Velocity = new Vector3(velocity, 0);
        }
        
        public override void Initialize(ContentManager content) { }

        public override void Draw(SpriteBatch spriteBatch, Time time) { }
    }
}