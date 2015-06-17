using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vector
{
    class Player : Sprite, Vector.IMove, Vector.IJump, Vector.ICollide
    {
        Vector2 _Velocity;
        public Vector2 Velocity {
            get { return _Velocity; }
            set
            {
                _Velocity = value;
                Arrow.Direction = value;
            }
        }

        public Arrow Arrow { get; set; }
        public bool GravityOn { get; set; }

        int MoveSpeed;
        int JumpSpeed;
        int Gravity;

        public Player(ref GraphicsDeviceManager graphicsDeviceManager)
            : base(ref graphicsDeviceManager)
        {
            Arrow = new Arrow(ref graphicsDeviceManager);
            Gravity = 1;
            GravityOn = true;
        }

        public void Initialize(Point position, int moveSpeed, int jumpSpeed)
        {
            base.Initialize(position);
            Arrow.Initialize(position, 2, 8);

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
            Velocity = Vector2.Zero;
        }

        public void Jump()
        {
            Velocity = new Vector2(Velocity.X, -JumpSpeed);
            GravityOn = true;
        }

        public void Update()
        {
            Velocity = new Vector2(Velocity.X, Velocity.Y + (GravityOn ? Gravity : 0));
            Position = new Point((int)(Position.X + Velocity.X), (int)(Position.Y + Velocity.Y));
        }

        public void Collide<T>(T component) where T : Sprite, Vector.ICollide
        {
            if (Bounds.Intersects(component.Bounds))
            {
                if (Bounds.Bottom > component.Bounds.Top)
                {
                    Velocity = new Vector2(Velocity.X, 0);
                    Position = new Point(Position.X, component.Bounds.Top - Bounds.Height);
                    GravityOn = false;
                }
            }
        }
    }
}
