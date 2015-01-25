using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scratch
{
    public class Picture
    {
        Texture2D Internal;
        World world;
        public Picture(System.IO.Stream stream, World world)
        {
            Internal = Texture2D.FromStream(world.game.GraphicsDevice, stream);
            this.world = world;
        }
        public Picture(string path, World world)
        {
            Internal = Texture2D.FromStream(world.game.GraphicsDevice, System.IO.File.Open(path, System.IO.FileMode.Open));
            this.world = world;
        }
        public Sprite ConvertToSprite()
        {
            return new Sprite(Internal, world);
        }
    }
}
