using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vector
{
    class Wall
    {
        public Rectangle Bounds { get; private set; }
        Texture2D Texture;

        public void Initialize(int x, int y, int width, int height)
        {
            Bounds = new Rectangle(x, y, width, height);
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("dirt");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, 
                new Vector2(Bounds.X,Bounds.Y), 
                Bounds, 
                Color.White, 0, 
                new Vector2(0), 1, 
                SpriteEffects.None, 0);
        }
    }
}
