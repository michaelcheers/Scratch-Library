using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Scratch
{
    public class BrodcastEvent
    {
        List<System.Threading.ThreadStart> broadcast = new List<System.Threading.ThreadStart>();
        /// <summary>
        /// Creates an event.
        /// </summary>
        public BrodcastEvent()
        {
        }
        /// <summary>
        /// Broadcasts the event.
        /// </summary>
        public void BroadCast()
        {
            foreach (System.Threading.ThreadStart b in broadcast)
            {
                new System.Threading.Thread(b).Start();
            }
        }
        /// <summary>
        /// Broadcast the event and waits.
        /// </summary>
        public void BroadCastAndWait()
        {
            foreach (System.Threading.ThreadStart b in broadcast)
            {
                b();
            }
        }
        /// <summary>
        /// Adds a function to the things to broadcast.
        /// </summary>
        /// <param name="c1">The thing to add the function to.</param>
        /// <param name="c2">The function to add to the event.</param>
        /// <returns>The function added to the event.</returns>
        public static BrodcastEvent operator +(BrodcastEvent c1, System.Threading.ThreadStart c2)
        {
            BrodcastEvent result = new BrodcastEvent();
            result.broadcast.AddRange(c1.broadcast);
            result.broadcast.Add(c2);
            return result;
        }
    }
}