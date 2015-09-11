using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vector
{
    class PowerBar : Sprite
    {
        public int TotalCapacity { get; private set; }
        int _CurrentCapacity;
        public int CurrentCapacity
        {
            get
            {
                return _CurrentCapacity;
            }
            set
            {
                if (value > TotalCapacity)
                {
                    _CurrentCapacity = TotalCapacity;
                }
                else
                {
                    _CurrentCapacity = value;
                }
            }
        }

        public PowerBar()
        {
            TotalCapacity = 100;
            CurrentCapacity = TotalCapacity;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentCapacity >= 0)
            {
                int bottomDrawAmount = CurrentCapacity; 

                if (CurrentCapacity > 10)
                {
                    bottomDrawAmount = 10;
                    int middleDrawAmount = CurrentCapacity - 10;

                    if (CurrentCapacity > 90)
                    {
                        middleDrawAmount = 80;
                        int topDrawAmount = CurrentCapacity - 90;
                        spriteBatch.Draw(Texture, new Rectangle(Position.X,Position.Y+80,Bounds.Width,topDrawAmount), 
                            new Rectangle(0,20,Bounds.Width,topDrawAmount), Color.White, 
                            0, Vector2.Zero, SpriteEffects.None, 0);
                    }

                    spriteBatch.Draw(Texture, new Rectangle(Position.X,Position.Y+10,Bounds.Width,middleDrawAmount), 
                        new Rectangle(0,10,Bounds.Width,10), Color.White, 
                        0, Vector2.Zero, SpriteEffects.None, 0);
                }

                spriteBatch.Draw(Texture, new Rectangle(Position.X,Position.Y,Bounds.Width,bottomDrawAmount), 
                    new Rectangle(0,0,Bounds.Width,bottomDrawAmount), Color.White, 
                    0, Vector2.Zero, SpriteEffects.None, 0);
            }
        }
    }
}
