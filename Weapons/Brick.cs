using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadZex.Core;

namespace PadZex.Weapons
{
	class Brick : Weapon
	{
		public Brick()
		{
			WeaponDamage = 3;
			WeaponSpeed = 2300;
			RotationSpeed = 0;
			Scale = 0.3f;
			Rotating = true;
			Offset = new Vector2(150, 150);
			AddTag("Brick");
			SpriteLocation = "sprites/weapons/brick";
		}

		public override void ThrowWeapon(Time time)
		{
			RotationSpeed = Core.CoreUtils.Random.Next(-5, 5);
			base.ThrowWeapon(time);
		}
	}
}
