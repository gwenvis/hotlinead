using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using PadZex.Core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PadZex
{
    public class BackgroundMusic : Entity
    {
        private const float VOLUME_CHANGE_RATE = 0.1f;
        private const float START_VOLUME = 0.3f; 
        private const Keys MUTE_KEY = Keys.M;
        
        private Song[] songs = new Song[3];
        private int currentSong;
        private int lastSong;
        private bool muted;
        private float volume;
        
        public BackgroundMusic()
        {
            AddTag("backgroundMusic");
        }

        public override void Draw(SpriteBatch spriteBatch, Time time)
        {
            
        }

        public override void Initialize(ContentManager content)
        {
            this.songs[0] = content.Load<Song>("backgroundMusic/music1");
            this.songs[1] = content.Load<Song>("backgroundMusic/music2");
            this.songs[2] = content.Load<Song>("backgroundMusic/music3");

        }

        /// <summary>
        /// sets up the music player
        /// </summary>
        public void Start()
        {
            volume = START_VOLUME;
            MediaPlayer.Volume = volume;
            MediaPlayer.IsRepeating = false;
            ShuffleSong();
            MediaPlayer.MediaStateChanged += OnMediaStateChanged;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            MediaPlayer.MediaStateChanged -= OnMediaStateChanged;
        }

        private void OnMediaStateChanged(object? sender, EventArgs e)
        {
            if (MediaPlayer.IsRepeating) return;
            ShuffleSong();
        }

        public override void Update(Time time)
        {
            volume += (Input.KeyFramePressed(Keys.Down) ? -VOLUME_CHANGE_RATE :
                Input.KeyFramePressed((Keys.Up)) ? VOLUME_CHANGE_RATE : 0);
            if (Input.KeyFramePressed(MUTE_KEY)) muted = !muted;
            ChangeVolume(volume);
        }

        public void PlaySong(int songIndex, bool repeating = false)
        {
            if (songIndex == currentSong || songIndex < 0 || songIndex >= songs.Length) return;
            MediaPlayer.Play(songs[songIndex]);
            MediaPlayer.IsRepeating = repeating;
            currentSong = songIndex;
        }

        public void ShuffleSong()
        {
            int tries = 5;
            do
            {
                int randomSongIndex = CoreUtils.Random.Next(songs.Length);

                if (randomSongIndex != currentSong)
                {
                    PlaySong(randomSongIndex);
                }
                
                tries--;
            } while (tries > 0);

            // just play the same song again as compromise
            PlaySong(currentSong);
        }

        private void ChangeVolume(float volumeAmount)
        {
            if (muted)
            {
                MediaPlayer.Volume = 0;
                return;
            }
            MediaPlayer.Volume = volumeAmount;
            MediaPlayer.Volume = MediaPlayer.Volume switch
            {
                < 0 => 0,
                > 1 => 1,
                _ => MediaPlayer.Volume
            };
        }
    }
}
