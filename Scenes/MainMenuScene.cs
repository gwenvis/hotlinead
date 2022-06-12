using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using PadZex.Core;
using PadZex.Entities;
using PadZex.Entities.MainMenu;

namespace PadZex.Scenes
{
    public class MainMenuScene : Scene
    {
        private const float SONG_VOLUME = 0.4f;
        private const int REFERENCE_WIDTH = 1920;
        private const int REFERENCE_HEIGHT = 1080;
        private const float FADE_SPEED = 0.7f; // in seconds
        private const float VOLUME_FADE_SPEED = 0.2f; // in seconds
        
        private Song song; 
        private SpriteEntity fade;
        private bool fading;

        public MainMenuScene(ContentManager content) : base(content)
        {
            var viewport = CoreUtils.GraphicsDevice.Viewport;
            Camera = new Camera(CoreUtils.GraphicsDevice.Viewport)
            {
                Zoom = (float)CoreUtils.ScreenSize.X / REFERENCE_WIDTH,
                X = 0,
                Y= 0,
            };
            Camera.AddTag("Camera");
            Camera.Update(default);
            AddEntityImmediate(Camera);
            
            song = content.Load<Song>("backgroundMusic/mainmenu1");

            SpriteEntity background = new("sprites/mainmenu/background", content)
            {
                Depth = -1,
            };
            background.Scale = (float)REFERENCE_WIDTH / background.Texture.Width;
            AddEntityImmediate(background);

            Color playNormalColor = new Color(0.0f, 1f, 0.0f);
            Color playHoverColor = new Color(0.0f, 0.5f, 0.0f);
            Button playButton = new("sprites/mainmenu/play", content, playNormalColor, playHoverColor,
                FadeToGameState);
            playButton.Center();
            playButton.Position = new Vector2((float) REFERENCE_WIDTH / 2, (float) REFERENCE_HEIGHT / 2);
            AddEntityImmediate(playButton);

            Color exitNormalColor = Color.Red;
            Color exitHoverColor = new Color(0.7f, .0f, .0f);
            Button exitbutton = new("sprites/mainmenu/exit", content, exitNormalColor, exitHoverColor,
                SceneManager.Quit);
            exitbutton.Origin = new Vector2(exitbutton.Texture.Width, exitbutton.Texture.Height);
            exitbutton.Position = new Vector2(REFERENCE_WIDTH - 10, REFERENCE_HEIGHT - 10);
            exitbutton.Scale = 0.4f;
            AddEntityImmediate(exitbutton);
            
            SpriteEntity logo = new("sprites/mainmenu/logo", content);
            logo.Center();
            logo.Scale = 0.7f / Camera.Zoom;
            logo.Position = new Vector2((float)REFERENCE_WIDTH / 2, (float)logo.Texture.Height / 2 + 2f);
            AddEntityImmediate(logo);
            AddEntityImmediate(new MouseEntity() { Scale = 2.0f });

            Texture2D singlePixelTexture = new(CoreUtils.GraphicsDevice, 1, 1);
            singlePixelTexture .SetData(new Color[] { Color.Black });
            fade = new SpriteEntity(singlePixelTexture)
            {
                Scale = 9000,
                Alpha = 0.0f,
            };
            AddEntityImmediate(fade);
        }

        public override void Initialize()
        {
            MediaPlayer.Volume = SONG_VOLUME;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
            base.Initialize();
        }

        public void FadeToGameState()
        {
            fading = true;
        }

        public override void Update(Time time)
        {
            if (fading)
            {
                fade.Alpha += FADE_SPEED * time.deltaTime;
                MediaPlayer.Volume -= VOLUME_FADE_SPEED * time.deltaTime;
                if (fade.Alpha > 0.999f)
                {
                    SceneManager.ChangeScene(SceneName.Game);
                }
                return;
            }
            
            base.Update(time);
        }
    }
}