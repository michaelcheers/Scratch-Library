using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scratch
{
    namespace BonusContent
    {
        public class WorldBonusContent
        {
            World value;
            public int ScreenWidth { get { return value.game.GraphicsDevice.Viewport.Width; } }
            public int ScreenHeight { get { return value.game.GraphicsDevice.Viewport.Height; } }
            public WorldBonusContent(World value)
            {
                this.value = value;
            }
            public void Update()
            {

            }
            public void Draw()
            {

            }
        }
    }
}
