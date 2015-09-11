using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vector
{
    class Arrow : Sprite
    {
        Vector2 _Direction;
        public Vector2 Direction
        {
            get { return _Direction;  }
            set
            {
                _Direction = value;
                Bounds = new Rectangle(Position.X, Position.Y, (int)(Direction.Length() * Scale), Thickness);
            }
        }

        public bool Dragging { get; set; }

        public Vector2 End
        {
            get
            {
                return new Vector2(Position.X + Direction.X * Scale, Position.Y + Direction.Y * Scale);
            }
            set
            {
                Direction = new Vector2((value.X - Position.X) / Scale, (value.Y - Position.Y) / Scale);  
            }
        }

        int Scale;
        int Thickness;

        public Arrow()
        {
            Dragging = false;
        }

        public void Initialize(Point position, int thickness, int scale)
        {
            base.Initialize(position);
            Scale = scale;
            Thickness = thickness;
        }

        public double Angle() {
            double angle = Math.Acos(Vector2.Dot(Direction, Vector2.UnitX) / Direction.Length());
            if (Direction.Y < 0)
            {
                angle *= -1;
            }
            return angle;
        }
    }
}
