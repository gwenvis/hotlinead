using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PadZex
{
    public static class Input
    {
        private static KeyboardState currentState;
        private static KeyboardState oldState;
        private static MouseState currentMouseState;
        private static MouseState oldMouseState;

        public static Point MousePosition => currentMouseState.Position;
        public static bool MouseLeftPressed => currentMouseState.LeftButton == ButtonState.Pressed;
        public static bool MouseRightPressed => currentMouseState.RightButton == ButtonState.Pressed;
        public static bool MouseLeftFramePressed => oldMouseState.LeftButton != ButtonState.Pressed &&
                                                    currentMouseState.LeftButton == ButtonState.Pressed;

        public static void UpdateInput()
        {
            oldState = currentState;
            oldMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            currentState = Keyboard.GetState();
        }

        /// <summary>
        /// Returns if the specified keyboard key is pressed
        /// </summary>
        /// <param name="key">Key to check if it's pressed</param>
        /// <returns>Whether the key is pressed or not</returns>
        public static bool KeyPressed(Keys key) => currentState.IsKeyDown(key);

        /// <summary>
        /// Returns true if the key was released previous frame and pressed on this frame
        /// </summary>
        public static bool KeyFramePressed(Keys key) => !oldState.IsKeyDown(key) && currentState.IsKeyDown(key);
    }
}