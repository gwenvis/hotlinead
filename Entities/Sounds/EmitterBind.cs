using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PadZex.Core;

namespace PadZex.Entities.Sounds
{
    public class EmitterBind : Core.Entity
    {
        private const float DISTANCE = 2000f;
        private const float MIN_AUDIO = 0.1f;
        private const float PAN_DISTANCE = 2200f;
        
        public bool Playing { get; set; }

        private Core.Entity target;
        private SoundEffectInstance soundEffect;
        private AudioEmitter audioEmitter;
        private AudioListener audioListener;

        public EmitterBind(Core.Entity target, SoundEffect soundEffect, AudioListener audioListener)
        {
            this.target = target;
            this.audioListener = audioListener;
            audioEmitter = new AudioEmitter();
            Position = target.Position;
            audioEmitter.Position = new Vector3(Position, 0);
            
            this.soundEffect = soundEffect.CreateInstance();
        }

        public void Play()
        {
            //this.soundEffect.Apply3D(audioListener, audioEmitter);
            this.soundEffect.Play();
        }

        public override void Initialize(ContentManager content)
        {
            Play();
        }

        public override void Update(Time time)
        {
            Vector2 velocity = target.Position - Position;
            velocity.Normalize();
            Position = target.Position;
            audioEmitter.Position = new Vector3(Position, 0);

            float distance = (audioListener.Position - audioEmitter.Position).LengthSquared();
            float volume = 1.0f - Math.Clamp(distance / (DISTANCE * DISTANCE), MIN_AUDIO, 1f);
            float abs = Math.Sign(audioListener.Position.X - audioEmitter.Position.X);
            float pan = Math.Clamp(distance / (PAN_DISTANCE * PAN_DISTANCE), 0f, 1f) * -abs;

            soundEffect.Volume = volume;
            if (pan is < 0.15f and > -0.15f) pan = 0f;
            soundEffect.Pan = Math.Clamp(pan, -1f, 1f);

            if (soundEffect.State == SoundState.Stopped)
            {
                soundEffect.Dispose();
                Entity.DeleteEntity(this);
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Time time) { }
    }
}