using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Vector
{
    class InputManager
    {
        public bool HasLeft()
        {
            return Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left);
        }

        public bool HasRight()
        {
            return Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right);
        }

        public bool HasJump()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Up);
        }

        public bool HasPause()
        {
            return Keyboard.GetState().IsKeyDown(Keys.P);
        }

        public bool HasExit()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Escape);
        }

        public bool HasMouseDown()
        {
            return Mouse.GetState().LeftButton == ButtonState.Pressed;
        }

        public Point GetMousePosition()
        {
            return Mouse.GetState().Position
        }
    }
}
