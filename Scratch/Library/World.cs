using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scratch
{
    public class World
    {
        public Texture2D bubblearrow;
        public Texture2D bubbleframe;
        public SpriteFont font;
        public BonusContent.WorldBonusContent BonusContent;
        WaitData waitdata;
        public struct WaitData
        {
            public void Wait()
            {
                waitHandle.Wait();
            }
            TimeSpan? timeleft;
            Sprite.WaitHandler waitHandle;
            public WaitData(TimeSpan timeleft)
            {
                this.timeleft = timeleft;
                waitHandle = new Sprite.WaitHandler();
            }
            public void Update()
            {
                if (timeleft != null)
                {
                    timeleft -= TimeSpan.FromSeconds(1.0 / 60.0);
                    if (timeleft <= TimeSpan.Zero)
                    {
                        waitHandle.Set();
                        timeleft = null;
                    }
                }
            }
        }
        public void StopAll()
        {
            Environment.Exit(Environment.ExitCode);
        }
        /// <summary>
        /// Waits the specified amount of time.
        /// </summary>
        /// <param name="time">The amount of time to wait.</param>
        public void Wait(TimeSpan time)
        {
            waitdata = new WaitData(time);
            waitdata.Wait();
        }
        public class Button
        {
            Texture2D image;
            Rectangle bounds;
            World creator;
            public event EventHandler Click;
            public Button(Texture2D image, Rectangle bounds, World creator)
            {
                this.image = image;
                this.bounds = bounds;
                this.creator = creator;
            }
            public Button(Texture2D image, Vector2 bounds, World creator)
            {
                this.image = image;
                this.bounds = new Rectangle((int)bounds.X, (int)bounds.Y, image.Width, image.Height);
                this.creator = creator;
            }
            bool clicked_last_frame = false;
            /// <summary>
            /// Draws the button.
            /// </summary>
            public void Draw()
            {
                creator.batch.Draw(image, bounds, Color.White);
            }
            /// <summary>
            /// Updates the button.
            /// </summary>
            public void Update()
            {
                MouseState state = Mouse.GetState();
                if ((state.X >= bounds.X) && (state.X <= (bounds.X + bounds.Width)) && (state.Y >= bounds.Y) && (state.Y <= (bounds.Y + bounds.Height)) && state.LeftButton == ButtonState.Pressed && !clicked_last_frame)
                {
                    clicked_last_frame = true;
                    Click(this, EventArgs.Empty);
                }
                else
                {
                    if (state.LeftButton == ButtonState.Released)
                        clicked_last_frame = false;
                }
            }
        }
        internal List<Sprite> sprites = new List<Sprite>();
        //bool game_running;
        internal SpriteBatch batch;
        internal Game game;
        /// <summary>
        /// The thread that the green flag is called on.
        /// </summary>
        internal System.Threading.Thread ThreadGreenFlag;
        /// <summary>
        /// The green flag.
        /// </summary>
        internal Button green_flag;
        internal Texture2D mouse = null;
        internal Button red_flag;
        /// <summary>
        /// Draws the world. Call in draw function.
        /// </summary>
        public void Draw()
        {
            batch.Begin();
            MouseState state = Mouse.GetState();
            batch.Draw(background, game.GraphicsDevice.Viewport.Bounds, Color.White);
            /*if (game_running)
            {*/
            foreach (Sprite sprite in sprites)
            {
                sprite.Draw(this);
            }
            //}
            green_flag.Draw();
            if (mouse != null)
                batch.Draw(mouse, new Rectangle(state.X, state.Y, 50, 50), Color.White);
            BonusContent.Draw();
            batch.End();
        }
        /// <summary>
        /// Waits until wait is true.
        /// </summary>
        /// <param name="wait">The thing you that you are waiting for to become true.</param>
        internal void WaitUntil(bool wait)
        {
            while (!wait) { }
        }
        KeyboardState last_frame_pressed;
        /// <summary>
        /// Args for CallKeyPress() function.
        /// </summary>
        Keys key_press_call_args;
        /// <summary>
        /// The function to call when a key press ocurrs.
        /// </summary>
        public void CallKeyPress()
        {
            switch (key_press_call_args)
            {
                case Keys.Left:
                    {
                        if (Left_Pressed != null)
                            Left_Pressed();
                        break;
                    }
                case Keys.Up:
                    {
                        if (Up_Pressed != null)
                            Up_Pressed();
                        break;
                    }
                case Keys.Right:
                    {
                        if (Right_Pressed != null)
                            Right_Pressed();
                        break;
                    }
                case Keys.Down:
                    {
                        if (Down_Pressed != null)
                            Down_Pressed();
                        break;
                    }
            }
            if (KeyPress != null)
                KeyPress(key_press_call_args);
        }
        /// <summary>
        /// Updates the world. Call in update function.
        /// </summary>
        public void Update()
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.Update();
            }
            green_flag.Update();
            KeyboardState state = Keyboard.GetState();
            foreach (var key in Enum.GetValues(typeof(Keys)))
            {
                if (state.IsKeyDown((Keys)key) && (!last_frame_pressed.IsKeyDown((Keys)key)))
                {
                    System.Threading.Thread thread = new System.Threading.Thread(CallKeyPress);
                    key_press_call_args = (Keys)key;
                    thread.Start();
                }
            }
            waitdata.Update();
            last_frame_pressed = state;
            BonusContent.Update();
        }
        /// <summary>
        /// Creates a sprite.
        /// </summary>
        /// <param name="load_name">The name of the picture to get.</param>
        /// <returns>The sprite created.</returns>
        public Sprite CreateSprite(string load_name)
        {
            return new Sprite(load_name, this);
        }
        /// <summary>
        /// Creates a sprite with a position.
        /// </summary>
        /// <param name="load_name">The name of the picture to load.</param>
        /// <param name="pos">The position of the sprite.</param>
        /// <returns>The sprite created.</returns>
        public Sprite CreateSprite(string load_name, Vector2 pos)
        {
            return new Sprite(pos, load_name, this);
        }
        /// <summary>
        /// Creates a sprite with a position.
        /// </summary>
        /// <param name="load_name">The name of the picture to load.</param>
        /// <param name="pos">The position of the sprite.</param>
        /// <returns>The sprite created.</returns>
        public Sprite CreateSprite(string load_name, Sprite.Vector2Object pos)
        {
            return new Sprite(pos, load_name, this);
        }
        /// <summary>
        /// Creates a sprite with an x any a y;
        /// </summary>
        /// <param name="load_name">The name of the picture to load.</param>
        /// <param name="x">The x of the sprite.</param>
        /// <param name="y">The y of the sprite.</param>
        /// <returns>The sprite created.</returns>
        public Sprite CreateSprite(string load_name, float x, float y)
        {
            return new Sprite(new Vector2(x, y), load_name, this);
        }
        /// <summary>
        /// Creates a sprite with an x any a y;
        /// </summary>
        /// <param name="load_name">The name of the picture to load.</param>
        /// <param name="x">The x of the sprite.</param>
        /// <param name="y">The y of the sprite.</param>
        /// <returns>The sprite created.</returns>
        public Sprite CreateSprite(string load_name, int x, int y)
        {
            return new Sprite(new Sprite.Vector2Object(x, y), load_name, this);
        }
        /// <summary>
        /// Creates a world.
        /// </summary>
        /// <param name="batch">The spritebatch used to draw the screen.</param>
        /// <param name="game">The game needed to load the content.</param>
        public World(SpriteBatch batch, Game game, string BackGroundLoadName)
        {
            System.Reflection.Assembly current = System.Reflection.Assembly.GetExecutingAssembly();
            this.batch = batch;
            this.game = game;
            green_flag = new Button(Texture2D.FromStream(game.GraphicsDevice, current.GetManifestResourceStream("Library.green-flag.png")), new Rectangle(50, 50, 25, 25), this);
            green_flag.Click += green_flag_Click;
            bubblearrow = Texture2D.FromStream(game.GraphicsDevice, current.GetManifestResourceStream("Library.bubblearrow.png"));
            bubbleframe = Texture2D.FromStream(game.GraphicsDevice, current.GetManifestResourceStream("Library.bubbleframe.png"));
            game.IsMouseVisible = true;
            game.Exiting += game_Exiting;
            font = game.Content.Load<SpriteFont>("SayFont");
            BonusContent = new BonusContent.WorldBonusContent(this);
            background = FromLoadName(BackGroundLoadName);
        }

        public World(SpriteBatch batch, Game game, string BackGroundLoadName, string MouseLoadName)
        {
            System.Reflection.Assembly current = System.Reflection.Assembly.GetExecutingAssembly();
            this.batch = batch;
            this.game = game;
            green_flag = new Button(Texture2D.FromStream(game.GraphicsDevice, current.GetManifestResourceStream("Library.green-flag.png")), new Rectangle(50, 50, 25, 25), this);
            green_flag.Click += green_flag_Click;
            bubblearrow = Texture2D.FromStream(game.GraphicsDevice, current.GetManifestResourceStream("Library.bubblearrow.png"));
            bubbleframe = Texture2D.FromStream(game.GraphicsDevice, current.GetManifestResourceStream("Library.bubbleframe.png"));
            game.Exiting += game_Exiting;
            mouse = FromLoadName(MouseLoadName);
            font = game.Content.Load<SpriteFont>("SayFont");
            BonusContent = new BonusContent.WorldBonusContent(this);
            background = FromLoadName(BackGroundLoadName);
        }

        internal Texture2D FromLoadName(string value)
        {
            if (!System.IO.File.Exists(value))
                return Texture2D.FromStream(game.GraphicsDevice, System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Library." + value + ".png"));
            else
                return Texture2D.FromStream(game.GraphicsDevice, System.IO.File.Open(value, System.IO.FileMode.Open));
        }

        Texture2D background;

        internal void LoadIfCalledByEmptyConstructor(SpriteBatch batch, Game game)
        {
            System.Reflection.Assembly current = System.Reflection.Assembly.GetExecutingAssembly();
            this.batch = batch;
            this.game = game;
            green_flag = new Button(Texture2D.FromStream(game.GraphicsDevice, current.GetManifestResourceStream("Library.green-flag.png")), new Rectangle(50, 50, 25, 25), this);
            green_flag.Click += green_flag_Click;
            bubblearrow = Texture2D.FromStream(game.GraphicsDevice, current.GetManifestResourceStream("Library.bubblearrow.png"));
            bubbleframe = Texture2D.FromStream(game.GraphicsDevice, current.GetManifestResourceStream("Library.bubbleframe.png"));
            game.IsMouseVisible = true;
            game.Exiting += game_Exiting;
        }

        void game_Exiting(object sender, EventArgs e)
        {
            if (ThreadGreenFlag != null)
            {
                ThreadGreenFlag.Abort();
            }
        }
        /// <summary>
        /// Checks whether to wrap up the thread.
        /// </summary>
        void ExitIfExit()
        {
            if (System.Threading.Thread.CurrentThread.ThreadState == System.Threading.ThreadState.SuspendRequested)
                System.Threading.Thread.CurrentThread.Abort();
        }
        /// <summary>
        /// Called when a key is pressed.
        /// </summary>
        public event KeyPress KeyPress;
        /// <summary>
        /// Executed when up pressed.
        /// </summary>
        public event Action Up_Pressed;
        /// <summary>
        /// Executed when down pressed.
        /// </summary>
        public event Action Down_Pressed;
        /// <summary>
        /// Called when the left key is pressed.
        /// </summary>
        public event Action Left_Pressed;
        /// <summary>
        /// Called when the right key is pressed.
        /// </summary>
        public event Action Right_Pressed;
        /// <summary>
        /// Called when the green flag is clicked.
        /// </summary>
        public event Action GreenFlag;
        Random random;

        public int CreateRandomNumber(int min, int max)
        {
            if (random == null)
            {
                int seed;
                bool sucsess = int.TryParse(System.IO.File.ReadAllText("/Private/seed.txt"), out seed);
                if (!sucsess)
                    random = new Random();
                else
                    random = new Random(seed);
            }
            System.IO.File.WriteAllText("/Private/seed.txt", Convert.ToString(random.Next()));
            ExitIfExit();
            return random.Next(min, max) + 1;
        }

        void CallGreenFlag()
        {
            if (GreenFlag != null)
                GreenFlag();
        }

        void green_flag_Click(object sender, EventArgs e)
        {
            if (ThreadGreenFlag != null)
                ThreadGreenFlag.Abort();
            ThreadGreenFlag = new System.Threading.Thread(CallGreenFlag);
            ThreadGreenFlag.Start();
        }

    }
    /// <summary>
    /// A delgate that represents a key press.
    /// </summary>
    /// <param name="key">The key that was pressed.</param>
    public delegate void KeyPress(Keys key);
}
