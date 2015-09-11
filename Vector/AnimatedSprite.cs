using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vector
{
    class AnimatedSprite : Sprite, Vector.IUpdate
    {
        Dictionary<String, int[]> Library;
        int[] CurrentAnimation;
        int CurrentFrame;
        Rectangle _Region;
        Rectangle Region
        {
            get
            {
                return new Rectangle(Bounds.Width * CurrentAnimation[CurrentFrame], _Region.Y, Bounds.Width, Bounds.Height);
            }
            set
            {
                _Region = value;
            }
        }
        bool Playing;

        public AnimatedSprite()
        {
            Library = new Dictionary<string, int[]>();
            CurrentAnimation = new int[1];
            CurrentFrame = 0;
            Playing = false;
        }

        public void LoadTexture(ContentManager content, string name, Rectangle region)
        {
            Texture = content.Load<Texture2D>(name);
            Bounds = new Rectangle(Bounds.X, Bounds.Y, region.Width, region.Height);
            Region = region;
        }

        public void AddToLibrary(string name, int[] animation)
        {
            if (animation.Length <= Texture.Width / Bounds.Width)
            {
                Library.Add(name, animation);
            }
        }

        public void PlayAnimation(string name)
        {
            if (!Playing)
            {
                Playing = true;
                CurrentAnimation = Library[name];
                CurrentFrame = 0;
            }
        }

        public void StopAnimation()
        {
            Playing = false;
        }

        public virtual void Update()
        {
            if (Playing)
            {
                CurrentFrame++;
            }

            if (CurrentFrame == CurrentAnimation.Length - 1)
            {
                Playing = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Bounds, Region, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
