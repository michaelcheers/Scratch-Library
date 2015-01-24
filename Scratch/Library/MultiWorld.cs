using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scratch
{
    namespace BonusContent
    {
        public class MultiWorld
        {
            List<World> worlds = new List<World>();
            int current = 0;
            public World currentworld { get { return worlds[current]; } }
            #region Constructor
            public MultiWorld()
            {
            }
            public MultiWorld(World world1)
            {
                worlds.Add(world1);
            }
            public MultiWorld(World world1, World world2)
            {
                worlds.Add(world1);
                worlds.Add(world2);
            }
            public MultiWorld(World world1, World world2, World world3)
            {
                worlds.Add(world1);
                worlds.Add(world2);
                worlds.Add(world3);
            }
            public MultiWorld(World world1, World world2, World world3, World world4)
            {
                worlds.Add(world1);
                worlds.Add(world2);
                worlds.Add(world3);
                worlds.Add(world4);
            }
            public MultiWorld(World world1, World world2, World world3, World world4, World world5, World world6)
            {
                worlds.Add(world1);
                worlds.Add(world2);
                worlds.Add(world3);
                worlds.Add(world4);
                worlds.Add(world5);
                worlds.Add(world6);
            }
            public MultiWorld(World world1, World world2, World world3, World world4, World world5, World world6, World world7)
            {
                worlds.Add(world1);
                worlds.Add(world2);
                worlds.Add(world3);
                worlds.Add(world4);
                worlds.Add(world5);
                worlds.Add(world6);
                worlds.Add(world7);
            }
            public MultiWorld(World world1, World world2, World world3, World world4, World world5, World world6, World world7, World world8)
            {
                worlds.Add(world1);
                worlds.Add(world2);
                worlds.Add(world3);
                worlds.Add(world4);
                worlds.Add(world5);
                worlds.Add(world6);
                worlds.Add(world7);
                worlds.Add(world8);
            }
            public MultiWorld(World world1, World world2, World world3, World world4, World world5, World world6, World world7, World world8, World world9)
            {
                worlds.Add(world1);
                worlds.Add(world2);
                worlds.Add(world3);
                worlds.Add(world4);
                worlds.Add(world5);
                worlds.Add(world6);
                worlds.Add(world7);
                worlds.Add(world8);
                worlds.Add(world9);
            }
            public MultiWorld(World world1, World world2, World world3, World world4, World world5, World world6, World world7, World world8, World world9, World world10)
            {
                worlds.Add(world1);
                worlds.Add(world2);
                worlds.Add(world3);
                worlds.Add(world4);
                worlds.Add(world5);
                worlds.Add(world6);
                worlds.Add(world7);
                worlds.Add(world8);
                worlds.Add(world9);
                worlds.Add(world10);
            }
            public MultiWorld(params World[] worlds)
            {
                this.worlds.AddRange(worlds);
            }
            #endregion
            /// <summary>
            /// Switches to the next world.
            /// </summary>
            public void NextWorld()
            {
                current++;
                current = current % worlds.Count;
            }
            /// <summary>
            /// Switchs the world to the specified world.
            /// </summary>
            /// <param name="world">The world to switch to.</param>0
            public void SwitchWorldTo(World world)
            {
                bool contains = true;
                int n = worlds.IndexOf(world);
                if (n == -1)
                    contains = false;
                if (contains)
                    current = n;
                else
                {
                    worlds.Add(world);
                    current = worlds.Count - 1;
                }
            }
            public void Add(World world)
            {
                worlds.Add(world);
            }
            public void Draw()
            {
                currentworld.Draw();
            }
            public void Update()
            {
                currentworld.Update();
            }
        }
    }
}
