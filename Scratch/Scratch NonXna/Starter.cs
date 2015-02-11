using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scratch_NonXna
{
    abstract class Startable
    {
        public abstract void Initialize();
        public virtual void Main ()
        {
            Initialize();
        }
        public void Run ()
        {
            Main();
        }
    }
    abstract class ScratchGameType : Startable
    {
        public abstract void Initialize(Scratch.ScratchGameHost gameHost);
        public override void Initialize()
        {
            Scratch.ScratchGameHost.Create(Initialize).Start();
        }
    }
    static class Starter
    {
        static void Main ()
        {
            new GameTest().Run();
        }
    }
}
