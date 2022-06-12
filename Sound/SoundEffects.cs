using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using PadZex.Core;
using PadZex.Entities.Sounds;
using PadZex.Scenes;

namespace PadZex.Sound
{
    public static class SoundPlayer
    {
        private const string SOUND_PATH = "data/sounds.data";
        
        private static Dictionary<string, Sound> sounds = new();
        private static AudioListener audioListener;
        private static Player player;

        public static void Load(ContentManager content)
        {
            string json = System.IO.File.ReadAllText(SOUND_PATH);
            var soundsJson = JsonSerializer.Deserialize<Sound[]>(json);

            if (soundsJson == null) return;
            foreach (var sound in soundsJson)
            {
                sound.Load(content);
                sounds.Add(sound.Name, sound);
            }
        }

        /// <summary>
        /// Play a sound and bind it to the entity
        /// If the player cannot be found the sound will not be played.
        /// </summary>
        public static void PlaySound(string sound, Entity emitter)
        {
            if (player == null && !FindPlayer()) return;

            var soundEffect = sounds[sound].GetRandomSound();
            EmitterBind emitterBind = new(emitter, soundEffect, audioListener); // TODO : cache this so it can be reused in the future.
            Scene.MainScene.AddEntity(emitterBind);
        }

        private static bool FindPlayer()
        {
            var playerEntity = Scene.MainScene.FindEntity<Player>("Player");
            if (playerEntity == null) return false;

            player = playerEntity;
            audioListener = new AudioListener();
            AudioListenerBind bind = new(playerEntity, audioListener);
            ((PlayScene)Scene.MainScene).AddProtectedEntity(bind);
            return true;
        }
    }
}
