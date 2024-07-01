using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;
using static SDL2.SDL;

namespace OriNoco
{
    public class InputManager
    {
        public Control control { get; private set; }
        public List<KeyInputState> keyStates = new List<KeyInputState>();
        private List<KeyInputState> queueKeyStates = new List<KeyInputState>();
        public List<MouseInputState> mouseStates = new List<MouseInputState>();
        private List<MouseInputState> queueMouseStates = new List<MouseInputState>();

        public InputManager(Control control)
        {
            this.control = control;
            control.MouseDown += OnMouseDown;
            control.MouseUp += OnMouseUp;

            var parent = control.Parent;
            parent.PreviewKeyDown += OnKeyDown;
            parent.KeyUp += OnKeyUp;
        }

        public void Update()
        {
            foreach(var state in keyStates)
            {
                if (state.state == InputStateMode.Down)
                {
                    state.state = InputStateMode.Hold;
                }
                else if (state.destroyAfter)
                {
                    state.state = InputStateMode.Up;
                    state.destroyAfter = false;
                }
                else if (state.state == InputStateMode.Up)
                {
                    state.state = InputStateMode.None;
                    queueKeyStates.Add(state);
                }
            }

            foreach(var queue in queueKeyStates)
                keyStates.Remove(queue);

            foreach(var state in mouseStates)
            {
                if (state.state == InputStateMode.Down)
                    state.state = InputStateMode.Hold;

                if (state.state == InputStateMode.Up)
                {
                    state.state = InputStateMode.None;
                    queueMouseStates.Add(state);
                }
            }

            foreach (var queue in queueMouseStates)
                mouseStates.Remove(queue);
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            var mouse = mouseStates.Find(val => val.button == e.Button);
            if (mouse == null)
            {
                mouse = new MouseInputState(e.Button);
                mouseStates.Add(mouse);
            }
            mouse.state = InputStateMode.Up;
            mouse.isPressed = false;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            var mouse = mouseStates.Find(val => val.button == e.Button);
            if (mouse == null)
            {
                mouseStates.Add(new MouseInputState(e.Button));
            }
            else
            {
                mouse.state = InputStateMode.Down;
                mouse.isPressed = true;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            var key = keyStates.Find(val => val.key == e.KeyCode);
            if (key == null)
            {
                key = new KeyInputState(e.KeyCode);
                keyStates.Add(key);
            }
            key.state = InputStateMode.Up;
            key.isPressed = false;
        }

        private void OnKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            var key = keyStates.Find(val => val.key == e.KeyCode);
            if (key != null)
                key.destroyAfter = true;
            keyStates.Add(new KeyInputState(e.KeyCode));
        }

        public bool GetKeyDown(Keys key) => keyStates.Exists(val => val.key == key && val.state == InputStateMode.Down);
        public bool GetKey(Keys key) => keyStates.Exists(val => val.key == key && val.isPressed);
        public bool GetKeyUp(Keys key) => keyStates.Exists(val => val.key == key && val.state == InputStateMode.Up);
    }

    public class KeyInputState
    {
        public InputStateMode state = InputStateMode.Down;
        public Keys key = Keys.None;
        public bool isPressed = true;
        public bool destroyAfter;

        public KeyInputState() { }
        public KeyInputState(Keys key) { this.key = key; }
    }

    public class MouseInputState
    {
        public InputStateMode state = InputStateMode.Down;
        public MouseButtons button = MouseButtons.None;
        public bool isPressed = true;

        public MouseInputState() { }
        public MouseInputState(MouseButtons button) { this.button = button; }
    }

    [Flags]
    public enum InputStateMode
    {
        None,
        PendingDown,
        Down,
        Hold,
        Up
    }
}
