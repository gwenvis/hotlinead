using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadZex.Weapons
{
	class Hammer : Weapon
	{
		public Hammer()
		{
			WeaponDamage = 3;
			WeaponSpeed = 1500;
			RotationSpeed = 30;
			Scale = 0.4f;
			Rotating = true;
			Offset = new Vector2(140, 30);
			AddTag("Hammer");
			SpriteLocation = "sprites/weapons/Hammer";
		}
	}
}
