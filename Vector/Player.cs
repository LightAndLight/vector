using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vector
{
    class Player : AnimatedSprite, Vector.IMove, Vector.IJump, Vector.ICollide
    {
        public Vector2 Velocity { get; set; }
        public Arrow Arrow { get; set; }
        public PowerBar PowerBar { get; set; }
        public bool GravityOn { get; set; }

        int MoveSpeed;
        int JumpSpeed;
        int Gravity;

        public Player(ref GraphicsDeviceManager graphicsDeviceManager)
            : base(ref graphicsDeviceManager)
        {
            Arrow = new Arrow(ref graphicsDeviceManager);
            PowerBar = new PowerBar(ref graphicsDeviceManager);
            Gravity = 1;
            GravityOn = true;
        }

        public void Initialize(Point position, int moveSpeed, int jumpSpeed)
        {
            base.Initialize(position);
            Arrow.Initialize(position, 1, 8);
            PowerBar.Initialize(new Point(380,10));

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

        public override void Update()
        {
            base.Update();

            Velocity = new Vector2(Velocity.X, Velocity.Y + (GravityOn ? Gravity : 0));
            Position = new Point((int)(Position.X + Velocity.X), (int)(Position.Y + Velocity.Y));

            Arrow.Direction = Velocity;
            Arrow.Position = new Point(Position.X+Bounds.Width/2,Position.Y+Bounds.Height/2);
        }

        public void Collide<T>(T component) where T : Sprite, Vector.ICollide
        {
            Rectangle velocityBB = new Rectangle(Position, Velocity.ToPoint());
            if (Bounds.Intersects(component.Bounds) || velocityBB.Intersects(component.Bounds))
            {
                if (Bounds.Bottom > component.Bounds.Top || velocityBB.Bottom > component.Bounds.Top)
                {
                    Velocity = new Vector2(Velocity.X, 0);
                    Position = new Point(Position.X, component.Bounds.Top - Bounds.Height);
                    GravityOn = false;
                }
            }
        }

        public void Collide(RenderTarget2D renderTarget)
        {
            if (Bounds.Left + Velocity.X < renderTarget.Bounds.Left)
            {
                Velocity = new Vector2(0, Velocity.Y);
                Position = new Point(renderTarget.Bounds.Left, Position.Y);
            }
            else if (Bounds.Right + Velocity.X > renderTarget.Bounds.Right)
            {
                Velocity = new Vector2(0, Velocity.Y);
                Position = new Point(renderTarget.Bounds.Right - Bounds.Width, Position.Y);
            }

            if (Bounds.Top + Velocity.Y < renderTarget.Bounds.Top)
            {
                Velocity = new Vector2(Velocity.X, 0);
                Position = new Point(Position.X, renderTarget.Bounds.Top);
            }
        }
    }
}
