using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vector
{
    class Arrow : Sprite
    {
        public Vector2 Velocity { get; set; }

        public Arrow(string textureName, Point position, Vector2 velocity) : base(textureName, position)
        {
            Velocity = velocity;
        }

        protected override void DrawFunc(SpriteBatch spriteBatch)
        {
            float theta = (float) ((Velocity.Y < 0 ? -1 : 1) * Math.Acos(Vector2.Dot(Vector2.UnitX, Velocity) / Velocity.Length()));
            Rectangle bounds = new Rectangle(Bounds.Location, new Point((int) Velocity.Length() * 10, Bounds.Height));
            Console.Write(bounds);
            spriteBatch.Draw(base.Texture, bounds, null, Color.White, theta, new Vector2(0, Bounds.Height / 2), SpriteEffects.None, 0);
        }
    }
}
