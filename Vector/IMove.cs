using System;
using Microsoft.Xna.Framework;

namespace Vector
{
    interface IMove
    {
        void MoveLeft();
        void MoveRight();
        void Stop();
    }
}
