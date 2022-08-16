using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FavobeanGames.Components.Input
{
    /// <summary>
    ///     InputManager handles input for Keyboard, GamePad, and Mouse controls.
    /// </summary>
    public class InputManager
    {
        protected GamePadState currentGamePadState;
        protected KeyboardState currentKeyboardState;
        protected MouseState currentMouseState;

        protected GamePadState prevGamePadState;

        // Control states
        protected KeyboardState prevKeyboardState;

        protected MouseState prevMouseState;

        public InputManager()
        {
        }

        public InputManager(int playerIndex)
        {
            PlayerIndex = playerIndex;
        }

        // PlayerIndex is the port of the player. Defaults to 1
        public int PlayerIndex { get; set; }

        public void Update(GameTime gameTime)
        {
            prevKeyboardState = currentKeyboardState;
            prevGamePadState = currentGamePadState;
            prevMouseState = currentMouseState;


            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex);
            currentMouseState = Mouse.GetState();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }

        private void UpdateMouse()
        {
        }

        #region Control Functions

        /// <summary>
        ///     Checks to see if the control is pressed.
        ///     This only returns true if the button/key is not being held
        /// </summary>
        /// <param name="control">InputControl object</param>
        /// <returns></returns>
        public bool IsControlPressed(InputControl control)
        {
            if (prevGamePadState.IsButtonDown(control.GamepadButton) &&
                !currentGamePadState.IsButtonDown(control.GamepadButton)) return true;

            if (prevKeyboardState.IsKeyDown(control.KeyboardKey) &&
                !currentKeyboardState.IsKeyDown(control.KeyboardKey)) return true;

            return false;
        }

        /// <summary>
        ///     Checks to see if the control is held down.
        /// </summary>
        /// <param name="control">InputControl object</param>
        /// <returns></returns>
        public bool IsControlHeld(InputControl control)
        {
            if (prevGamePadState.IsButtonDown(control.GamepadButton) &&
                currentGamePadState.IsButtonDown(control.GamepadButton)) return true;

            if (prevKeyboardState.IsKeyDown(control.KeyboardKey) &&
                currentKeyboardState.IsKeyDown(control.KeyboardKey)) return true;

            return false;
        }

        #endregion

        #region Mouse Functions

        public bool MouseLeftButtonPressed()
        {
            return prevMouseState.LeftButton == ButtonState.Pressed &&
                   currentMouseState.LeftButton == ButtonState.Released;
        }

        public bool MouseLeftButtonHeld()
        {
            return prevMouseState.LeftButton == ButtonState.Pressed &&
                   currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool MouseRightButtonPressed()
        {
            return prevMouseState.RightButton == ButtonState.Pressed &&
                   currentMouseState.RightButton == ButtonState.Released;
        }

        public bool MouseRightButtonHeld()
        {
            return prevMouseState.RightButton == ButtonState.Pressed &&
                   currentMouseState.RightButton == ButtonState.Pressed;
        }

        public Vector2 GetMousePosition()
        {
            return currentMouseState.Position.ToVector2();
        }

        public void SetMousePosition(Vector2 position)
        {
            Mouse.SetPosition((int) position.X, (int) position.Y);
        }

        #endregion
    }
}