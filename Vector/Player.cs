using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Vector
{
    class Player
    {
        Rectangle bounds;
        public Rectangle Bounds { get { return bounds; } set { bounds = value; } }
        public int MovementSpeed { get; private set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Position { get { return new Vector2(bounds.Left, bounds.Top); } }

        int Gravity;
        int JumpVelocity;
        Texture2D Texture;

        public void Initialize(int x, int y)
        {
            bounds = new Rectangle(x, y, 0, 0);
            Velocity = new Vector2(0);
            Gravity = 1;
            MovementSpeed = 5;
            JumpVelocity = -15;
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("man");
            bounds = new Rectangle(Bounds.X, Bounds.Y, Texture.Width, Texture.Height);
        }

        public void Update()
        {
            Velocity = new Vector2(Velocity.X,Velocity.Y+Gravity);
            bounds.Offset(Velocity);
        }

        public void MoveLeft() {
            Velocity = new Vector2(-MovementSpeed, Velocity.Y);
        }

        public void MoveRight() {
            Velocity = new Vector2(MovementSpeed, Velocity.Y);
        }

        public void Stop() {
            Velocity = new Vector2(0, Velocity.Y);
        }

        public void Jump() {
            Velocity = new Vector2(Velocity.X, JumpVelocity);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, bounds, Color.White);
        }
    }
}
