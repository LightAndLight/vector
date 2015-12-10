using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vector
{
    class Player : Sprite, Vector.IMove, Vector.IJump
    {
        public Vector2 Velocity { get; set; }

        private int MoveSpeed;
        private int JumpSpeed;
        private bool GravityOn;
        private float Gravity;

        public Player(
            string textureName,
            Point position,
            int moveSpeed,
            int jumpSpeed) : base(textureName, position)
        {
            Gravity = 0.7f;
            GravityOn = true;
            Velocity = Vector2.Zero;
            MoveSpeed = moveSpeed;
            JumpSpeed = jumpSpeed;
        }

        public void MoveLeft()
        {
            Velocity = new Vector2(-MoveSpeed, Velocity.Y);
        }

        public void MoveRight()
        {
            Velocity = new Vector2(MoveSpeed, Velocity.Y);
        }

        public void Stop()
        {
            Velocity = new Vector2(GravityOn ? Velocity.X : 0, Velocity.Y);
        }

        public void Jump()
        {
            if (!GravityOn) {
                Velocity = new Vector2(Velocity.X, -JumpSpeed);
                GravityOn = true;
            }
        }

        public void Update()
        {
            Velocity = new Vector2(Velocity.X, Velocity.Y + (GravityOn ? Gravity : 0));
            base.Bounds = new Rectangle(
                Bounds.X + (int)Velocity.X,
                Bounds.Y + (int)Velocity.Y,
                Bounds.Width,
                Bounds.Height);
        }

        public void CollideWithScreen(RenderTarget2D renderTarget)
        {
            if (Bounds.Left + Velocity.X < renderTarget.Bounds.Left)
            {
                Velocity = new Vector2(0, Velocity.Y);
                base.Bounds = new Rectangle(
                    renderTarget.Bounds.Left,
                    Bounds.Y,
                    Bounds.Width,
                    Bounds.Height);
            }
            else if (Bounds.Right + Velocity.X > renderTarget.Bounds.Right)
            {
                Velocity = new Vector2(0, Velocity.Y);
                base.Bounds = new Rectangle(
                    renderTarget.Bounds.Right - Bounds.Width,
                    Bounds.Y,
                    Bounds.Width,
                    Bounds.Height);
            }

            if (Bounds.Top + Velocity.Y < renderTarget.Bounds.Top)
            {
                Velocity = new Vector2(Velocity.X, 0);
                base.Bounds= new Rectangle(
                    Bounds.X,
                    renderTarget.Bounds.Top,
                    Bounds.Width,
                    Bounds.Height);
            }
            else if (Bounds.Bottom + Velocity.Y > renderTarget.Bounds.Bottom)
            {
                Velocity = new Vector2(Velocity.X, 0);
                base.Bounds = new Rectangle(
                    Bounds.X,
                    renderTarget.Bounds.Bottom - Bounds.Height,
                    Bounds.Width,
                    Bounds.Height);
                GravityOn = false;
            }
        }
    }
}
