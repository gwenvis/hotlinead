using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using PadZex.Core;

namespace PadZex.Sound
{
    [System.Serializable]
    public class Sound
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("sounds")]
        public List<string> SoundNames { get; set; }

        public IReadOnlyList<SoundEffect> Sounds;

        [JsonPropertyName("volume")]
        public float Volume { get; set; }

        public void Load(ContentManager content) => Sounds = SoundNames.Select(content.Load<SoundEffect>).ToList();
        public SoundEffect GetRandomSound() => Sounds[CoreUtils.Random.Next(Sounds.Count)];
        public SoundEffect GetSound(int index) => Sounds[index];
    }
}