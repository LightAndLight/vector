using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vector
{
    class Sprite
    {
        public Rectangle Bounds { get; set; }
        public Texture2D Texture { get; set; }

        public Point Position
        {
            get
            {
                return Bounds.Location;
            }

            set
            {
                Bounds = new Rectangle(value.X, value.Y, Bounds.Width, Bounds.Height);
            }
        }

        public Sprite()
        {
            Bounds = new Rectangle();
        }

        public void Initialize(Point position)
        {
            Position = position;
        }

        public void LoadTexture(GraphicsDevice graphicsDevice, Color color)
        {
            Texture = new Texture2D(graphicsDevice, 1, 1);
            Texture.SetData<Color>(new Color[] { color });
        }

        public virtual void LoadTexture(ContentManager content, string name)
        {
            Texture = content.Load<Texture2D>(name);
            Bounds = new Rectangle(Bounds.X, Bounds.Y, Texture.Width, Texture.Height);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Bounds, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
        }

        public void DrawRotated(SpriteBatch spriteBatch, float angle, Vector2 origin)
        {
            spriteBatch.Draw(Texture, Bounds, null, Color.White, angle, origin, SpriteEffects.None, 0);
        }

        public void DrawTiled(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position.ToVector2(), Bounds, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }
    }
}
