using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vector
{
    class Wall : Sprite, Vector.ICollide
    {
        public void Initialize(Rectangle bounds)
        {
            Bounds = bounds;
        }

        public override void LoadTexture(ContentManager content, string name)
        {
            Texture = content.Load<Texture2D>(name);
            Bounds = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, Texture.Height);
        }

        public void Collide<T>(T component) where T : Sprite, Vector.ICollide { }
    }
}
