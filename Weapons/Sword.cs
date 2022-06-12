using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadZex.Weapons
{
    public class Sword : Weapon
    {
        public Sword() 
        {
            WeaponDamage = 3;
            WeaponSpeed = 3500;
            RotationSpeed = 35;
            Scale = 0.5f;
            Rotating = true;
            Offset = new Vector2(190, 70);
            AddTag("Sword");
            SpriteLocation = "sprites/weapons/sword";
        }
    }
}
