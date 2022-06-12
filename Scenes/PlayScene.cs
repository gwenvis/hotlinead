using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using PadZex.LevelLoader;
using PadZex.Core;
using System.Linq;
using Microsoft.Xna.Framework;
using PadZex.Entities;

namespace PadZex.Scenes
{
	public class PlayScene : Scene
	{
		private const int REFERENCE_WIDTH = 1920;
		private const float CAMERA_ZOOM = 0.5f;
		private const float DEAD_CAMERA_ZOOM = 1.5f;
		private const float ZOOM_SPEED = 0.8f;

		public int EnemyCount { get; set; } = 0;

		private Level loadedLevel;
		private List<Entity> spawnedEntities;
		private List<Entity> protectedEntities = new();
		private readonly BackgroundMusic backgroundMusic;
		private readonly Player player;

		private int currentLevel = 1;
		private bool LevelLoaded { get; set; }
		private SpriteEntity deathOverlay;

		public PlayScene(ContentManager content) : base(content)
		{
			player = new Player();
			AddProtectedEntityImmediate(player);
			AddProtectedEntityImmediate((backgroundMusic = new BackgroundMusic()));
			AddProtectedEntityImmediate((Camera = new Camera(CoreUtils.GraphicsDevice.Viewport)));
			AddProtectedEntityImmediate(new MouseEntity());

			Camera.SelectTarget("Player", this, -player.SpriteSize * player.Scale / 4);
			Camera.Zoom = CAMERA_ZOOM;
			Camera.Zoom *= CoreUtils.ScreenSize.X / (float)REFERENCE_WIDTH;

			deathOverlay = new SpriteEntity("sprites/deathOverlay")
			{
				Depth = 9999,
				Alpha = 0
			};
			AddProtectedEntity(deathOverlay);

			player.DeadEvent += () =>
			{
				deathOverlay.Alpha = 1.0f;
				deathOverlay.Position = Camera.Position;
				deathOverlay.Scale = (float)CoreUtils.ScreenSize.X / REFERENCE_WIDTH / Camera.Zoom;
			};

			var level = LevelLoader.LevelLoader.LoadLevel(CoreUtils.GraphicsDevice, "level1");
			LoadLevel(level);
		}

		public void AddProtectedEntityImmediate(Entity entity)
		{
			protectedEntities.Add(entity);
			AddEntityImmediate(entity);
		}

		public void AddProtectedEntity(Entity entity)
		{
			protectedEntities.Add(entity);
			AddEntity(entity);
		}

		public override void Initialize()
		{
			backgroundMusic.Start();
			base.Initialize();
		}

		public void LoadLevel(Level level)
		{
			if (LevelLoaded)
			{
				UnloadLevel();
			}

			EnemyCount = 0;
			loadedLevel = level;
			spawnedEntities = new List<Entity>();
			int width = level.Tiles.First().Texture.Width;

			foreach (var tile in level.Tiles)
			{
				var tileEntity = new Entities.Level.Tile(tile);
				AddEntity(tileEntity);
				spawnedEntities.Add(tileEntity);
			}

			foreach (var entityType in level.Entities)
			{
				var entityTypeInstance = Activator.CreateInstance(entityType.EntityType, level, entityType.GridPosition);
				if (entityTypeInstance == null) continue;
				Entity entity = (Entity)entityTypeInstance;
				entity.Position = new Microsoft.Xna.Framework.Vector2(
					entityType.GridPosition.X * width,
					entityType.GridPosition.Y * width);

				spawnedEntities.Add(entity);
				AddEntity(entity);
			}

			player.Reset();
			deathOverlay.Alpha = 0.0f;
			Camera.Zoom = CAMERA_ZOOM;
			Camera.Zoom *= CoreUtils.ScreenSize.X / (float)REFERENCE_WIDTH;
			Camera.Angle = 0;
			LevelLoaded = true;
		}

		public void ReloadLevel() => LoadLevel(loadedLevel);

		public void UnloadLevel()
		{
			foreach (var entity in entities)
			{
				if (protectedEntities.Contains(entity)) continue;
				DeleteEntity(entity);
			}

			spawnedEntities.Clear();
			LevelLoaded = false;
		}

		public override void Update(Time time)
		{
			if (player.Dead)
			{
				float zoom = MathHelper.Lerp(Camera.Zoom, DEAD_CAMERA_ZOOM, time.deltaTime * ZOOM_SPEED);

				deathOverlay.Position = Camera.Position;
				deathOverlay.Scale = (float)CoreUtils.ScreenSize.X / REFERENCE_WIDTH / Camera.Zoom;
				Camera.Zoom = zoom;

				if (Input.MouseLeftPressed)
				{
					ReloadLevel();
				}
			}

			if (!HitStun.UpdateStun(time.deltaTime))
			{
				base.Update(time);
			} else {
				Camera.Update(time);
			}

			Camera.offset = ScreenShake.UpdateShake(time.deltaTime);
		}

		public void LoadNextLevel()
		{
			if (LevelLoaded)
			{
				UnloadLevel();
			}

			currentLevel++;

			if (!LevelLoader.LevelLoader.DoesLevelExist("level" + currentLevel))
			{
				// TODO : switch to game win state instead.
				return;
			}

			var level = LevelLoader.LevelLoader.LoadLevel(Core.CoreUtils.GraphicsDevice, "level" + currentLevel);
			LoadLevel(level);
		}
	}
}
