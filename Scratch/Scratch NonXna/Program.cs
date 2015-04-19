using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scratch;

namespace Scratch_NonXna
{
    class GameTest : ScratchGameType
    {
        Sprite cat;
        World world;
        public override void Initialize (ScratchGameHost host)
        {
            world = host.InitializeWorld("white"/*, "cat"*/);
            cat = world.CreateSprite("cat");
            cat.BonusContent.SETTINGS = Scratch.BonusContent.SpriteBonusContent.Settings.Draggable;
            world.GreenFlag += world_GreenFlag;
        }

        void world_GreenFlag()
        {
            cat.Say("Hello World!", TimeSpan.FromSeconds(2.0));
        }
    }
}
