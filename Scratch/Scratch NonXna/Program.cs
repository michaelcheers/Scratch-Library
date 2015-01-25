using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scratch;

namespace Scratch_NonXna
{
    static class Program
    {
        static Sprite cat;
        static World world;
        static void Initialize (ScratchGameHost host)
        {
            world = host.InitializeWorld("white"/*, "cat"*/);
            cat = world.CreateSprite("cat");
            cat.BonusContent.SETTINGS = Scratch.BonusContent.SpriteBonusContent.Settings.Draggable;
            world.GreenFlag += world_GreenFlag;
        }

        static void world_GreenFlag()
        {
            cat.GoTo(0, 0);
            cat.Say("Hello World!", TimeSpan.FromSeconds(2.0));
        }

        static void Main(string[] args)
        {
            ScratchGameHost.Create(Initialize).Start();
        }
    }
}
