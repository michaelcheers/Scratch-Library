using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scratch
{
    internal static class StaticClass
    {
        internal static Point ToPoint(this Vector2 input)
        {
            return new Point((int)input.X, (int)input.Y);
        }
    }
}
