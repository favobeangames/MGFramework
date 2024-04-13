using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace FavobeanGames.MGFramework.Input
{
    /// <summary>
    /// Object containing the information relating to the input.
    /// This stores the keyboard and gamepad
    /// to be used by the input manager
    /// </summary>
    public class InputControl
    {
        public InputControl(Buttons gamepadButton, Keys keyboardKey)
        {
            GamepadButton = gamepadButton;
            KeyboardKey = keyboardKey;
        }

        public Buttons GamepadButton;
        public Keys KeyboardKey;
    }

    /// <summary>
    /// Mapping for the controls
    /// </summary>
    public class InputControlMapping
    {
        private Dictionary<string, InputControl> controls;

        public InputControlMapping()
        {
            controls = new Dictionary<string, InputControl>();
        }

        public InputControl GetControl(string key)
        {
            return controls.TryGetValue(key, out var control) ? control : null;
        }
        public void AddControl(string key, InputControl control)
        {
            controls.Add(key, control);
        }

        public void RemoveControl(string key)
        {
            controls.Remove(key);
        }
    }
}