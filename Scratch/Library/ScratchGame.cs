using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scratch
{
    public class ScratchGameHost
    {
        public static ScratchGameHost Create(ScratchGameAction initialize)
        {
            return new ScratchGameHost(initialize);
        }
        public ScratchGameHost(ScratchGameAction initialize)
        {
            value = new ScratchGame(initialize, this);
        }
        public World InitializeWorld()
        {
            return value.InitializeWorld("white");
        }
        public World InitializeWorld(string BackGroundLoadName)
        {
            return value.InitializeWorld(BackGroundLoadName);
        }
        public World InitializeWorld(string BackGroundLoadName, string MouseLoadName)
        {
            return value.InitializeWorld(BackGroundLoadName, MouseLoadName);
        }
        public BonusContent.MultiWorld InitializeMultiWorld()
        {
            return value.InitializeMultiWorld();
        }
        public void Start()
        {
            value.Run();
        }
        ScratchGame value;
    }
    public delegate void ScratchGameAction(ScratchGameHost value);
    class ScratchGame : Game
    {
        public World InitializeWorld(string BackGroundLoadName)
        {
            World result = new World(spriteBatch, this, BackGroundLoadName);
            worlds.Add(result);
            return result;
        }
        public World InitializeWorld(string BackGroundLoadName, string MouseLoadName)
        {
            World result = new World(spriteBatch, this, BackGroundLoadName, MouseLoadName);
            worlds.Add(result);
            return result;
        }
        public BonusContent.MultiWorld InitializeMultiWorld()
        {
            BonusContent.MultiWorld result = new BonusContent.MultiWorld();
            multiworlds.Add(result);
            return result;
        }
        GraphicsDeviceManager graphics;
        List<World> worlds = new List<World>();
        List<BonusContent.MultiWorld> multiworlds = new List<BonusContent.MultiWorld>();
        public ScratchGame(ScratchGameAction initialize, ScratchGameHost host)
        {
            this.callback = initialize;
            this.host = host;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 360;
            Content.RootDirectory = "Content";
        }
        ScratchGameAction callback;
        ScratchGameHost host;
        SpriteBatch spriteBatch;
        protected override void Update(GameTime gameTime)
        {
            foreach (World world in worlds)
            {
                world.Update();
            }
            foreach (BonusContent.MultiWorld multiworld in multiworlds)
            {
                multiworld.Update();
            }
            base.Update(gameTime);
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            callback(host);
            base.LoadContent();
        }
        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void Draw(GameTime gameTime)
        {
            foreach (World world in worlds)
            {
                world.Draw();
            }
            foreach (BonusContent.MultiWorld multiworld in multiworlds)
            {
                multiworld.Draw();
            }
            base.Draw(gameTime);
        }
    }
}
