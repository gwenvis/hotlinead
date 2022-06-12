using System;
using System.Collections.Generic;
using System.Text;

namespace PadZex.LevelLoader
{
    internal static class EntityTypeGlobals
    {
        internal static Dictionary<EntityType, Type> EntityTypes = new Dictionary<EntityType, Type>()
        {
            { EntityType.PlayerSpawn, typeof(Entities.Level.PlayerSpawn) },
            { EntityType.RandomWeapon, typeof(Entities.Level.RandomWeaponSpawn) },
            { EntityType.EnemySpawn, typeof(Entities.Level.EnemySpawn) },
            { EntityType.LevelEnd, typeof(Entities.Level.LevelEndSpawn) },
            { EntityType.Door, typeof(Entities.Level.DoorSpawn) }
        };

        internal static Type GetEntityType(EntityType @type) => EntityTypes[@type];
    }
}
