using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using PadZex.Core;
using PadZex.Weapons;
using Microsoft.Xna.Framework;

namespace PadZex.Entities.Level
{
    public class RandomWeaponSpawn : Entity
    {
        private Type[] weaponTypes = new Type[]
        {
            typeof(Dagger), typeof(Sword), typeof(Potion), typeof(Hammer), typeof(Brick)
        };

        private Weapon spawnedWeapon;
        private int randomWeaponIndex;

        public RandomWeaponSpawn(LevelLoader.Level level, Point gridPos)
        {
            randomWeaponIndex = CoreUtils.Random.Next(weaponTypes.Length);
        }

        public override void Draw(SpriteBatch spriteBatch, Time time)
        {

        }

        public override void Initialize(ContentManager content)
        {
            float randomAngle = (float)CoreUtils.Random.NextDouble() * 100f - 50f;
            Type weaponType = weaponTypes[randomWeaponIndex];

            spawnedWeapon = (Weapon)Activator.CreateInstance(weaponType);
            spawnedWeapon.Position = Position;
            spawnedWeapon.Position.X += 265 / 2;
            spawnedWeapon.Position.Y += 265 / 2;
            spawnedWeapon.Angle = randomAngle;

            Scene.MainScene.AddEntity(spawnedWeapon);
        }

        public override void Update(Time time)
        {
            
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Scene.MainScene.DeleteEntity(spawnedWeapon);
        }
    }
}
