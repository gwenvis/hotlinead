using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadZex.Entities
{
	public static class ScreenShake
	{
		private static float shakeTime = 0;
		private static int shakeAmount = 25;

		public static void Shake()
		{
			shakeTime = 0.1f;
		}

		public static Vector2 UpdateShake(float deltaTime)
		{
			if (shakeTime <= 0) return Vector2.Zero;

			Random r = Core.CoreUtils.Random;
			Vector2 shake = new Vector2(r.Next(-shakeAmount, shakeAmount), r.Next(-shakeAmount, shakeAmount));
			shakeTime -= deltaTime;

			return shake;
			
		}

	}
}
