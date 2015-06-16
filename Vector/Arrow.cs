using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vector
{
    class Arrow
    {
        Texture2D Texture;
        public Vector2 Direction { get; set; }
        public Vector2 End { 
            get { return Vector2.Add(Vector2.Multiply(Direction, Stretch),Position); }
            set { Direction = Vector2.Divide(Vector2.Subtract(value, Position),Stretch); }
        }
        public Vector2 Position { get; set; }
        public bool Dragging { get; set; }

        int Width;
        int Stretch;
        public void Initialize(Texture2D texture, Vector2 direction, Vector2 position, int width, int stretch) {
            Texture = texture;
            Direction = direction;
            Position = position;
            Width = width;
            Stretch = stretch;
            Dragging = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            double angle = Math.Acos(Vector2.Dot(Direction, Vector2.UnitX) / Direction.Length());
            if (Direction.Y < 0)
            {
                angle *= -1;
            }

            spriteBatch.Draw(
                Texture,
                new Rectangle((int)Position.X, (int)Position.Y, (int)Direction.Length()*Stretch, Width),
                null,
                Color.Red,
                (float) angle,
                new Vector2(0),
                SpriteEffects.None,
                1f
                );
        }
    }
}
