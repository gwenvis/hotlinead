using System;

namespace PadZex
{
    public static class HitStun
    {
        /// <summary>
        /// The duration of a hit stun frame.
        /// </summary>
        private const float FRAME_DURATION = 0.16f / 6;

        /// <summary>
        /// The duration of a pause frame where the game continues before the next freeze.
        /// </summary>
        private const float PAUSE_FRAME_TIME = 0.16f / 4;

        private static int hitStunAmount = 0;
        private static float currentHitStunTime = FRAME_DURATION;
        private static bool isPauseFrame = false;

        /// <summary>
        /// Add Hitstun time.
        /// </summary>
        /// <param name="time">Time to add. Any values below 1 will be ignored.</param>
        public static void Add()
        {
            hitStunAmount ++ ;
        }

        /// <summary>
        /// Updates the hitstun timing.
        /// </summary>
        /// <param name="deltaTime">Time since last frame</param>
        /// <returns>returns false if there is no stun, returns true if there is a stun.</returns>
        public static bool UpdateStun(float deltaTime)
        {
            if (hitStunAmount == 0) return false;

            if (isPauseFrame)
            {
                currentHitStunTime -= deltaTime;
                if (currentHitStunTime <= 0)
                {
                    currentHitStunTime = FRAME_DURATION;
                    isPauseFrame = false;
                }
                return true;
            }

            currentHitStunTime -= deltaTime;
            
            if (currentHitStunTime <= 0)
            {
                hitStunAmount--;
                isPauseFrame = true;
                currentHitStunTime = PAUSE_FRAME_TIME;
                return false;
            }

            return true;
        }
    }
}