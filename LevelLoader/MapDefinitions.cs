using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace PadZex.LevelLoader
{
    /// <summary>
    /// Definitions are the files types that can be used in the game.
    /// Stores all paths and files that can be loaded.
    ///
    /// TODO : This code is a mess and should be cleaned probably. It uses 2 dictionaries
    /// and that shouldn't be the case.
    /// </summary>
    internal static class MapDefinitions
    {
        private const string DEFINITION_FILE = "tiledefinitions";
        private const string ENTITY_DEFINITION_FILE = "entitydefinitions";

        private static readonly Type[] tileTypes = new Type[] {
            typeof(FloorType), typeof(WallType), typeof(EntityType)
        };

        internal static IReadOnlyDictionary<Color, TileDefinition<FloorType>> FloorTileDefinition => floorTileDefinitions;
        internal static IReadOnlyDictionary<Color, TileDefinition<WallType>> WallTileDefinition => wallTileDefinitions;
        internal static IReadOnlyDictionary<Color, EntityType> EntityTypeDefinitions => entityDefinitions;

        private static Dictionary<Color, TileDefinition<FloorType>> floorTileDefinitions;
        private static Dictionary<Color, TileDefinition<WallType>> wallTileDefinitions;
        private static Dictionary<Color, EntityType> entityDefinitions;

        /// <summary>
        /// Get a specific, and generic, tile definition
        /// </summary>
        internal static TileDefinition GetDefinition(Color color)
        {
            if(floorTileDefinitions.TryGetValue(color, out var floorTile))
                return floorTile;
            else if(WallTileDefinition.TryGetValue(color, out var wallTile))
                return wallTile;

            return null;
        }

        internal static EntityType? GetEntityDefinition(Color color)
        {
            if (EntityTypeDefinitions.TryGetValue(color, out EntityType entityType)) return entityType;
            return null;
        }

        internal static void LoadTiles()
        {
            floorTileDefinitions = new Dictionary<Color, TileDefinition<FloorType>>();
            wallTileDefinitions = new();
            entityDefinitions = new();

            string[] definitions = File.ReadAllLines(Path.Combine(LevelLoader.LEVEL_PATH, DEFINITION_FILE));
            string[] entityDefs = File.ReadAllLines(Path.Combine(LevelLoader.LEVEL_PATH, ENTITY_DEFINITION_FILE));

            foreach(string definition in definitions)
            {
                LoadDefinition(definition);
            }

            foreach(string entityDefinition in entityDefs)
            {
                LoadEntityDefinition(entityDefinition);
            }
        }

        private static void LoadDefinition(string definition)
        {
            string[] split = definition.Split(" ");

            (Type enumType, int enumValue) = GetDefinitionType(split[0]);
            Color color = ColorUtils.FromHex(split[1]);
            string path = split[2];
            string[] files = new string[split.Length - 2 - 1];

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = split[3 + i];
            }

            AddDefinition(enumType, enumValue, color, path, files);
        }

        private static void LoadEntityDefinition(string entityDefinition)
        {
            string[] split = entityDefinition.Split(" ");

            (Type enumType, int enumValue) = GetDefinitionType(split[0]);
            Color color = ColorUtils.FromHex(split[1]);

            AddEntityDefinition(color, (EntityType)enumValue);
        }

        private static void AddDefinition(Type enumType, int enumValue, Color color, string path, string[] files)
        {
            if(enumType == typeof(FloorType))
            {
                var floorTypeDefinition = new TileDefinition<FloorType>(path, files, (FloorType)enumValue);
                floorTileDefinitions.Add(color, floorTypeDefinition);
            }
            else if(enumType == typeof(WallType))
            {
                var wallTypeDefinition = new TileDefinition<WallType>(path, files, (WallType)enumValue);
                wallTileDefinitions.Add(color, wallTypeDefinition);
            }
        }

        private static void AddEntityDefinition(Color color, EntityType entityType)
        {
            entityDefinitions.Add(color, entityType);
        }

        /// <summary>
        /// Retrieves the enum and the enum value of the enum definition.
        /// </summary>
        private static (Type, int) GetDefinitionType(string enumType)
        {
            string[] split = enumType.Split('.');
            string enumTypeName = split[0];
            string value = split[1];

            Type enumVal = GetEnumType(enumTypeName);
            string[] enumNames = Enum.GetNames(enumVal);

            for (int i = 0; i < enumNames.Length; i++)
            {
                if(enumNames[i].Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    return (enumVal, i);
                }
            }

            return (null, -1);
        }

        private static Type GetEnumType(string enumTypeName)
        {
            foreach(Type tileType in tileTypes)
            {
                if (tileType.Name.Equals(enumTypeName, StringComparison.OrdinalIgnoreCase))
                {
                    return tileType;
                } 
            }

            return null;
        }
    }
}
