using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vector
{
    interface ICollide
    {
        void Collide<T>(T component) where T : Sprite, Vector.ICollide;
    }
}
