using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Vector
{
    class InputManager
    {
        public bool LeftPressed()
        {
            return Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left);
        }

        public bool LeftReleased() {
            return Keyboard.GetState().IsKeyUp(Keys.A) && Keyboard.GetState().IsKeyUp(Keys.Left);
        }

        public bool RightPressed()
        {
            return Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right);
        }

        public bool RightReleased() {
             return Keyboard.GetState().IsKeyUp(Keys.D) && Keyboard.GetState().IsKeyUp(Keys.Right);
        }

        public bool Jump()
        {
            return Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up);
        }

        public bool PausePressed()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Space);
        }

        public bool PauseReleased()
        {
            return Keyboard.GetState().IsKeyUp(Keys.Space);
        }

        public bool Exit()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Escape);
        }

        public bool MouseDown()
        {
            return Mouse.GetState().LeftButton == ButtonState.Pressed;
        }

        public bool MouseUp()
        {
            return Mouse.GetState().LeftButton == ButtonState.Released;
        }

        public Point MousePosition()
        {
            return Mouse.GetState().Position;
        }
    }
}
