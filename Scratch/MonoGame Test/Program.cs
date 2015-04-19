using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scratch;

namespace MonoGame_Test
{
    class Program
    {
        static World world;
        static Sprite sprite;
        static void Initialize (ScratchGameHost host)
        {
            world = host.InitializeWorld("cat");
            sprite = world.CreateSprite("cat");
            world.GreenFlag += world_GreenFlag;
        }

        static void world_GreenFlag()
        {
            sprite.Move_Steps(100f);
            world.Wait(TimeSpan.FromSeconds(0.5));
            sprite.PointTowards(0, 0);
            world.Wait(TimeSpan.FromSeconds(0.5));
            sprite.Move_Steps(10f);
        }
        static void Main(string[] args)
        {
            ScratchGameHost.Create(Initialize).Start();
        }
    }
}
