using Microsoft.Xna.Framework;
using System;

namespace PadZex.LevelLoader
{
    public static class ColorUtils
    {
        public static Color FromHex(string hex)
        {
            byte r = Convert.ToByte(hex.Substring(0,2), 16);
            byte g = Convert.ToByte(hex.Substring(2,2), 16);
            byte b = Convert.ToByte(hex.Substring(4,2), 16);
            return new Color(r, g, b);
        }
    }
}
