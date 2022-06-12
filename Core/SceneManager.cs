using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using PadZex.Scenes;

namespace PadZex.Core
{
    public static class SceneManager
    {
        public static event Action QuitEvent;
        
        /// <summary>
        /// Scene used when none specified.
        /// </summary>
        public const SceneName STARTING_SCENE = SceneName.MainMenu;
        
        /// <summary>
        /// The scene that is currently used
        /// </summary>
        public static SceneName CurrentScene { get; private set; }

        private static Dictionary<SceneName, Scene> scenes; 

        /// <summary>
        /// Initialize scene with <see cref="STARTING_SCENE"/> as the boot scene.
        /// </summary>
        public static void InitializeScenes(ContentManager content) => InitializeScenes(content, STARTING_SCENE);

        /// <summary>
        /// Initialize all scenes and start the scene at <paramref name="scene"/>
        /// </summary>
        public static void InitializeScenes(ContentManager content, SceneName scene)
        {
            scenes = new Dictionary<SceneName, Scene>() {
                {SceneName.MainMenu, new MainMenuScene(content)},
                {SceneName.Game, new PlayScene(content)}
            };

            scenes[scene].SetAsMainScene();
            Scene.MainScene.Initialize();
        }

        /// <summary>
        /// Changes the main scene to another scene.
        /// </summary>
        public static void ChangeScene(SceneName scene)
        {
            if (CurrentScene == scene) return;
            
            CurrentScene = scene;
            scenes[CurrentScene].SetAsMainScene();
            Scene.MainScene.Initialize();
        }

        public static void Quit()
        {
            QuitEvent?.Invoke();
        }
    }
}