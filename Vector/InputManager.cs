using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Vector
{
    public class InputManager 
    {
        private bool PauseReady;

        enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        public bool MoveLeft
        {
            get
            {
                return MovementStack.Count > 0 && MovementStack.First.Value == Direction.Left;
            }
        }

        public bool MoveRight
        {
            get
            {
                return MovementStack.Count > 0 && MovementStack.First.Value == Direction.Right;
            }
        }

        public bool Jump
        {
            get 
            {
                return Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up);
            }
        }

        public bool MouseDown
        {
            get {
                return Mouse.GetState().LeftButton == ButtonState.Pressed;
            }
        }

        public bool MouseUp
        {
            get
            {
                return Mouse.GetState().LeftButton == ButtonState.Released;
            }
        }

        public Point MousePosition
        {
            get
            {
                return Mouse.GetState().Position;
            }
        }

        public bool Exit
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.Escape);
            }
        }

        private bool PausePressed
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.Space);
            }
        }

        private bool PauseReleased
        {
            get
            {
                return Keyboard.GetState().IsKeyUp(Keys.Space);
            }
        }

        private bool LeftPressed
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left);
            }
        }

        private bool LeftReleased
        {
            get
            {
                return Keyboard.GetState().IsKeyUp(Keys.A) && Keyboard.GetState().IsKeyUp(Keys.Left);
            }
        }

        private bool RightPressed
        {
            get
            {
                return Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right);
            }
        }

        private bool RightReleased
        {
            get
            {
                return Keyboard.GetState().IsKeyUp(Keys.D) && Keyboard.GetState().IsKeyUp(Keys.Right);
            }
        }

        LinkedList<Direction> MovementStack;

        public InputManager()
        {
            MovementStack = new LinkedList<Direction>();
            PauseReady = true;
        }

        public void Update()
        {
            if (LeftPressed && !MovementStack.Contains(Direction.Left))
            {
                MovementStack.AddFirst(Direction.Left);
            }
            else if (LeftReleased && MovementStack.Contains(Direction.Left))
            {
                MovementStack.Remove(Direction.Left);
            }

            if (RightPressed && !MovementStack.Contains(Direction.Right))
            {
                MovementStack.AddFirst(Direction.Right);
            }
            else if (RightReleased && MovementStack.Contains(Direction.Right))
            {
                MovementStack.Remove(Direction.Right);
            }

        }

        public bool TogglePause()
        {
            if (PausePressed && PauseReady)
            {
                PauseReady = false;
                return true;
            }
            else if (PauseReleased)
            {
                PauseReady = true;
            }

            return false;
        }

    }
}
