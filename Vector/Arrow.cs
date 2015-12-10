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
        private float ScaleFactor;
        public Point EndPoint
        {
            get
            {
                return Vector2.Add(base.Bounds.Location.ToVector2(), Vector2.Multiply(Velocity, ScaleFactor)).ToPoint();
            }

            set
            {
                Velocity = Vector2.Divide(Vector2.Subtract(value.ToVector2(), Bounds.Location.ToVector2()), ScaleFactor);
            }
        }

        private bool dragging;
        private Vector2 startVelocity;
        public bool Dragging
        {
            get
            {
                return dragging;
            }
        }

        public int Change
        {
            get
            {
                int currentMag = (int) Velocity.Length();
                int startMag = (int) startVelocity.Length();
                if (dragging && currentMag > startMag) {
                    return currentMag - startMag;
                } else {
                    return 0;
                }
            }
        }

        public Arrow(string textureName, Point position, int scaleFactor, Vector2 velocity) : base(textureName, position)
        {
            base.Visible = false;
            Velocity = velocity;
            ScaleFactor = scaleFactor;
            dragging = false;
        }

        protected override void DrawFunc(SpriteBatch spriteBatch)
        {
            float theta = (float) ((Velocity.Y < 0 ? -1 : 1) * Math.Acos(Vector2.Dot(Vector2.UnitX, Velocity) / Velocity.Length()));
            Rectangle bounds = new Rectangle(Bounds.Location, new Point((int) (Velocity.Length() * 10), Bounds.Height));
            spriteBatch.Draw(base.Texture, bounds, null, Color.White, theta, new Vector2(0, Bounds.Height / 2), SpriteEffects.None, 0);
        }

        public void StartDragging(Vector2 velocity)
        {
            dragging = true;
            startVelocity = velocity;
        }

        public void StopDragging()
        {
            dragging = false;
        }
    }
}
