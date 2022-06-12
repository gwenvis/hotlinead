using Microsoft.Xna.Framework;
using PadZex.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PadZex.Weapons
{
    class Dagger : Weapon
    {
        public Dagger()
        {
            WeaponDamage = 1;
            WeaponSpeed = 5000;
            RotationSpeed = 0;
            Scale = 0.2f;
            Rotating = false;
            Offset = new Vector2(200, 150);
            isFlipped = true;
			AddTag("Dagger");
            SpriteLocation = "sprites/weapons/machete";
        }

	}
}
