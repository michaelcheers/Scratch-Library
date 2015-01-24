# Scratch Library
This is a library for implementing projects using Scratch graphics libraries. It is implemented in C# calling XNA.

I am 9 and this is my introduction to this library.
I learn't Scratch when I was 5 because my dad said if you would like to make a video game make it yourself.
I decided to make this product so that people learning c# coming from scratch either don't get bored with Console Applications or not know how to use more complex graphics library like **DirectX** or **OpenGl** or **Xna**, etc.

## Precompiled Version

To use this project make a Console Application and add the [dll](https://www.dropbox.com/s/gwby19s6x8mv9ob/Library.dll?dl=0) to the project.
You must install xna redistrabutable from this [link](http://www.microsoft.com/en-ca/download/details.aspx?id=20914) to use my library.

  ````c#
using System;
using Scratch;

namespace Scratch_NonXna
{
    static class Program
    {
        static Sprite cat;
        static World world;
        static void Initialize (ScratchGameHost host)
        {
            world = host.InitializeWorld("white", "cat");
            cat = world.CreateSprite("cat");
            world.GreenFlag += world_GreenFlag;
        }

        static void world_GreenFlag()
        {
            cat.GoTo(world.BonusContent.ScreenWidth / 2, world.BonusContent.ScreenHeight / 2);
            cat.Say("Hello World!", TimeSpan.FromSeconds(2.0));
        }

        static void Main(string[] args)
        {
            ScratchGameHost.Create(Initialize).Start();
        }
    }
}

  ````
  ##How to build
  
  To build this project you must install [xna](https://msxna.codeplex.com/downloads/get/777889).
  This project is for vs 2013 for how to port a project [click here](http://stackoverflow.com/questions/20486230/how-to-convert-visual-studios-2013-project-to-visual-studios-2010).