using System;
using System.Collections.Generic;

namespace PadZex.LevelLoader
{
    internal record TileDefinition<T>(string Path, IEnumerable<string> Tiles, T TileType) : TileDefinition(Path, Tiles) where T : Enum { }
    internal record TileDefinition(string Path, IEnumerable<string> Tiles) { }
}
