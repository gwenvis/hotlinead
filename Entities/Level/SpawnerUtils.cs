using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace PadZex.Entities.Level
{
    public static class SpawnerUtils
    {
        /// <summary>
        /// returns ture if all point positions are solid
        /// </summary>
        public static bool IsSolid(List<LevelLoader.Tile> tiles, Point levelSize, params Point[] positions)
        {
            int GetIndex(Point point) => point.X + point.Y * levelSize.X;
            bool isSolid = true;

            foreach (var position in positions)
            {
                int point = GetIndex(position);
                if (point < 0 || point >= tiles.Count) continue;
                isSolid = isSolid && tiles[point].Shape != null;
            }

            return isSolid;
        }
    }
}