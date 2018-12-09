using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Super_Platformer.Code.Core.Input
{
    /// <summary>
    /// Keyboard input class.
    /// </summary>
    public class KeyboardInput : IMonoUpdateable
    {
        /// <summary> Current state. </summary>
        private KeyboardState _state;

        /// <summary> Previous state. </summary>
        private KeyboardState _previousState;

        /// <summary>
        /// Update the previous state to the current state.
        /// </summary>
        public void EndFrame()
        {
            _previousState = _state;
        }

        /// <summary>
        /// Determine if a key was not pressed in this frame.
        /// </summary>
        /// <param name="key"> The key to check.</param>
        /// <returns>Returns true if the key is released.</returns>
        public bool KeyUp(Keys key)
        {
            return _state.IsKeyUp(key);
        }

        /// <summary>
        /// Determine if a key was pressed in this frame.
        /// </summary>
        /// <param name="key"> The key to check.</param>
        /// <returns>Returns true if the key is pressed.</returns>
        public bool KeyDown(Keys key)
        {
            return _state.IsKeyDown(key) && _previousState.IsKeyUp(key);
        }

        /// <summary>
        /// Determine if a key is held in this frame.
        /// </summary>
        /// <param name="key"> The key to check.</param>
        /// <returns>Returns true if the key is held down.</returns>
        public bool KeyHeld(Keys key)
        {
            return _state.IsKeyDown(key);
        }

        /// <summary>
        /// Update function (IMonoUpdatable).
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        public void Update(GameTime gameTime)
        {
            _state = Keyboard.GetState();
        }
    }
}
