using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vector
{
    class Sprite
    {
        public MainGame Game { get; private set;}
        public Rectangle Bounds { get; set; }
        public bool Visible { get; set; }
        private string TextureName;
        protected Texture2D Texture;

        public Sprite(string textureName, Point position)
        {
            Bounds = new Rectangle(position, new Point(0, 0));
            TextureName = textureName;
            Visible = true;
        }

        public void LoadTexture(ContentManager contentManager)
        {
            Texture = contentManager.Load<Texture2D>(TextureName);
            Bounds = new Rectangle(Bounds.X, Bounds.Y, Texture.Width, Texture.Height);
        }

        protected virtual void DrawFunc(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Bounds, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                DrawFunc(spriteBatch);
            }
        }
    }
}
