using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scratch
{
    public class Sprite
    {
        internal class WaitHandler : System.Threading.EventWaitHandle
        {
            public WaitHandler() : base(false, System.Threading.EventResetMode.ManualReset) { }
            public void Wait()
            {
                WaitOne();
            }
        }
        public class Vector2Object
        {
            internal int x;
            internal int y;
            public Vector2Object()
            {
                x = 0;
                y = 0;
            }
            public Vector2Object(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            public Vector2Object(Vector2 value)
            {
                x = (int)value.X;
                y = (int)value.Y;
            }
            public Vector2Object(Point value)
            {
                x = value.X;
                y = value.Y;
            }
            public Rectangle ConvertToRectangle(Texture2D picture)
            {
                return new Rectangle(x, y, picture.Width, picture.Height);
            }
        }
        public static class Direction
        {
            public const float left = -90;
            public const float up = 0;
            public const float right = 90;
            public const float down = 180;
        }
        public struct GlideData
        {
            /// <summary>
            /// The number of frames until the glide is finished.
            /// </summary>
            public double framesleft { get; private set; }
            /// <summary>
            /// The distance to go per second.
            /// </summary>
            public Vector2 perframe { get; private set; }
            WaitHandler waitHandle;
            public GlideData(double framesleft, Vector2 perframe)
                : this()
            {
                this.perframe = perframe;
                this.framesleft = framesleft;
                waitHandle = new WaitHandler();
            }
            public void WaitTillGlideFinished()
            {
                waitHandle.Wait();
            }

            public void UpdateValues()
            {
                framesleft--;
                if (framesleft <= 0.0)
                {
                    if (waitHandle != null)
                    {
                        waitHandle.Set();
                        waitHandle = null;
                    }
                }
            }
        }
        public struct SayData
        {
            TimeSpan? timeleft;
            string text;
            Sprite value;
            WaitHandler waitHandle;
            public SayData(string text, TimeSpan timeleft, Sprite value)
                : this()
            {
                this.text = text;
                this.timeleft = timeleft;
                waitHandle = new WaitHandler();
                this.value = value;
            }

            public void WaitTillSayFinished()
            {
                waitHandle.Wait();
            }

            public bool IsValid()
            {
                return timeleft != null;
            }
            public string GetText()
            {
                return text;
            }
            public void Update()
            {
                if (timeleft != null)
                {
                    if (timeleft != TimeSpan.MinValue)
                        timeleft -= TimeSpan.FromSeconds(1.0 / 60.0);
                    if (timeleft <= TimeSpan.Zero)
                    {
                        timeleft = null;
                        waitHandle.Set();
                    }
                }
            }

            public void DrawStretchedRect(SpriteBatch spriteBatch, Rectangle rect, Texture2D texture, Color color)
            {
                rect = ConvertToRectangle(value.ConvertFromScratchPosToXnaPos(ConvertToVector2(rect)), rect);
                int fragmentW = texture.Width / 4;
                int fragmentH = texture.Height / 4;
                int rightEdgeX = rect.X + rect.Width - fragmentW;
                int bottomEdgeY = rect.Y + rect.Height - fragmentH;
                // TL, top, TR
                spriteBatch.Draw(texture, new Rectangle(rect.X, rect.Y, fragmentW, fragmentH),
                    new Rectangle(0, 0, fragmentW, fragmentH), color);
                spriteBatch.Draw(texture, new Rectangle(rect.X + fragmentW, rect.Y, rect.Width - fragmentW * 2, fragmentH),
                    new Rectangle(fragmentW, 0, fragmentW * 2, fragmentH), color);
                spriteBatch.Draw(texture, new Rectangle(rightEdgeX, rect.Y, fragmentW, fragmentH),
                    new Rectangle(fragmentW * 3, 0, fragmentW, fragmentH), color);

                // left, center, right
                spriteBatch.Draw(texture, new Rectangle(rect.X, rect.Y + fragmentH, fragmentW, rect.Height - fragmentH * 2),
                    new Rectangle(0, fragmentH, fragmentW, fragmentH * 2), color);
                spriteBatch.Draw(texture, new Rectangle(rect.X + fragmentW, rect.Y + fragmentH, rect.Width - fragmentW * 2, rect.Height - fragmentH * 2),
                    new Rectangle(fragmentW, fragmentH, fragmentW * 2, fragmentH * 2), color);
                spriteBatch.Draw(texture, new Rectangle(rightEdgeX, rect.Y + fragmentH, fragmentW, rect.Height - fragmentH * 2),
                    new Rectangle(fragmentW * 3, fragmentH, fragmentW, fragmentH * 2), color);

                // BL, bottom, BR
                spriteBatch.Draw(texture, new Rectangle(rect.X, bottomEdgeY, fragmentW, fragmentH),
                    new Rectangle(0, fragmentH * 3, fragmentW, fragmentH), color);
                spriteBatch.Draw(texture, new Rectangle(rect.X + fragmentW, bottomEdgeY, rect.Width - fragmentW * 2, fragmentH),
                    new Rectangle(fragmentW, fragmentH * 3, fragmentW * 2, fragmentH), color);
                spriteBatch.Draw(texture, new Rectangle(rightEdgeX, bottomEdgeY, fragmentW, fragmentH),
                    new Rectangle(fragmentW * 3, fragmentH * 3, fragmentW, fragmentH), color);
            }

            public void Draw(World world, Rectangle spriteRect)
            {
                Vector2 contentSize = world.font.MeasureString(text);
                const int padding = 5;
                Rectangle bubbleRect = new Rectangle((int)(spriteRect.Center.X - contentSize.X / 2) - padding, (int)(spriteRect.Top - contentSize.Y), (int)contentSize.X + padding * 2, (int)contentSize.Y);
                DrawStretchedRect(world.batch, bubbleRect, world.bubbleframe, Color.White);
                world.batch.Draw(world.bubblearrow, value.ConvertFromScratchPosToXnaPos(new Vector2(spriteRect.Center.X - world.bubblearrow.Width / 2, spriteRect.Top - world.bubblearrow.Height / 2)), Color.White);
                world.batch.DrawString(world.font, text, value.ConvertFromScratchPosToXnaPos(new Vector2(bubbleRect.Left + padding, bubbleRect.Top)), Color.Black);
            }
        }
        internal Rectangle rect;
        void RectChanged(Rectangle OriginalValue)
        {
            Rectangle ValueNow = rect;
        }
        internal int costume;
        public int Costume
        {
            get
            {
                return costume + 1;
            }
        }
        bool IsPenDown;
        public void PenDown()
        {
            IsPenDown = true;
        }
        public void PenUp()
        {
            IsPenDown = false;
        }
        internal Converter<Vector2, Vector2> ConvertFromScratchPosToXnaPos;
        public BonusContent.SpriteBonusContent BonusContent;
        List<Texture2D> pictures = new List<Texture2D>(10);
        Texture2D picture { get { return pictures[costume]; } }
        Game game;
        Color color;
        float rotation;
        World world;
        GlideData glidedata;
        SayData saydata;
        #region Constructor
        public Sprite(string load_name, World world) : this(world.FromLoadName(load_name), world) { }
        public Sprite(Texture2D picture, World world) : this(new Vector2(), picture, world) { }
        public Sprite(Vector2 pos, Texture2D picture, World world) : this(pos, new Texture2D[]{picture}, world) { }
        public Sprite(Vector2 pos, List<string> load_names, World world) : this(pos, load_names.ConvertAll(world.FromLoadName), world) { }
        public Sprite(Vector2Object pos, List<string> load_names, World world) : this(pos, load_names.ConvertAll(world.FromLoadName), world) { }
        public Sprite(Vector2 pos, IEnumerable<Texture2D> pictures, World world) : this(new Vector2Object(pos), pictures, world) { }
        public Sprite(Vector2 pos, string load_name, World world) : this(pos, world.FromLoadName(load_name), world) { }
        public Sprite(Vector2Object pos, Texture2D picture, World world) : this(pos, new Texture2D[] { picture }, world) { }
        public Sprite(Vector2Object pos, string load_name, World world) : this(pos, world.FromLoadName(load_name), world) { }
        public Sprite(Vector2Object pos, IEnumerable<Texture2D> pictures, World world) : this(pos, pictures, world, world.ConvertFromScratchPosToXnaPos) { }
        public Sprite(Vector2Object pos, IEnumerable<Texture2D> pictures, World world, Converter<Vector2, Vector2> posConverter)
        {
            ConvertFromScratchPosToXnaPos = posConverter;
            this.game = world.game;
            this.world = world;
            foreach (Texture2D t in pictures)
                this.pictures.Add(t);
            this.rect = pos.ConvertToRectangle(picture);
            this.world = world; 
            BonusContent = new Scratch.BonusContent.SpriteBonusContent(this);
            glidedata = new GlideData(0, new Vector2());
            color = Color.White;
            rotation = (float)(Math.PI / 2.0);
            world.sprites.Add(this);
        }
        #endregion
        /// <summary>
        /// Points to the specified sprite.
        /// </summary>
        /// <param name="point">The sprite to point to.</param>
        public void PointTowards(Sprite point)
        {
            PointTowards(new Vector2(point.rect.Center.X, point.rect.Center.Y));
        }
        /// <summary>
        /// Points to the specified location.
        /// </summary>
        /// <param name="point">The location to point to.</param>
        public void PointTowards(Vector2 point)
        {
            Vector2 back = ((ConvertToVector2(rect) - point));
            back.X = -back.X;
            back.Normalize();
            if (back.X > 0)
                rotation = (float)Math.Acos(back.Y);
            else
                rotation = (float)(Math.PI * 2 - Math.Acos(back.Y));
            //            Vector2 reverse = new Vector2((float)Math.Asin(back.X), (float)-Math.Acos(back.Y));
        }
        /// <summary>
        /// Goes to the specified location.
        /// </summary>
        /// <param name="x">X to go to.</param>
        /// <param name="y">Y to go to.</param>
        public void GoTo(int x, int y)
        {
            Rectangle OriginalValue = rect;
            this.rect.X = x;
            this.rect.Y = y;
            RectChanged(OriginalValue);
        }
        /// <summary>
        /// Goes to the specified position.
        /// </summary>
        /// <param name="pos">The position to go to.</param>
        public void GoTo(Vector2 pos)
        {
            Rectangle OriginalValue = rect;
            this.rect = ConvertToRectangle(pos, this.rect);
            RectChanged(OriginalValue);
        }
        /// <summary>
        /// Switches the costume to the specified costume.
        /// </summary>
        /// <param name="costume">The specified costume.</param>
        public void SwitchToCostume(int costume)
        {
            this.costume = costume + 1;
        }
        /// <summary>
        /// Goes to the specified position.
        /// </summary>
        /// <param name="pos">The position to go to.</param>
        public void GoTo(Vector2Object pos)
        {
            Rectangle OriginalValue = rect;
            this.rect = ConvertToRectangle(pos, this.rect);
            RectChanged(OriginalValue);
        }
        public void GoTo(Sprite GoTo)
        {
            Rectangle OriginalValue = rect;
            rect = ConvertToRectangle(ConvertToVector2(GoTo.rect), rect);
            RectChanged(OriginalValue);
        }
        /// <summary>
        /// Glides to the specified locaton on the screen in th especified time.
        /// </summary>
        /// <param name="x">The x of the place to glide to.</param>
        /// <param name="y">The y of the place to glide to.</param>
        /// <param name="time">How long the glide will take.</param>
        public void GlideTo(int x, int y, TimeSpan time)
        {
            glidedata = new GlideData(
              time.TotalSeconds * 60,
              new Vector2(
                  (float)((x - rect.X) / (time.TotalSeconds * 60)),
                  (float)((y - rect.Y) / (time.TotalSeconds * 60))
              )
            );
            glidedata.WaitTillGlideFinished();
        }
        /// <summary>
        /// Glides to the specified locaton on the screen in th especified time.
        /// </summary>
        /// <param name="pos">The place to glide to.</param>
        /// <param name="time">How long the glide will take.</param>
        public void GlideTo(Vector2 pos, TimeSpan time)
        {
            glidedata = new GlideData(
              time.TotalSeconds * 60,
              new Vector2(
                  (float)((pos.X - this.rect.X) / (time.TotalSeconds * 60)),
                  (float)((pos.Y - this.rect.Y) / (time.TotalSeconds * 60))
              )
            );
            glidedata.WaitTillGlideFinished();
        }
        public void Say(string text, double secondsInTime)
        {
            saydata = new SayData(text, TimeSpan.FromSeconds(secondsInTime), this);
            saydata.WaitTillSayFinished();
        }
        public void Say(string text, TimeSpan time)
        {
            saydata = new SayData(text, time, this);
            saydata.WaitTillSayFinished();
        }
        public void Say(string text)
        {
            saydata = new SayData(text, System.TimeSpan.MinValue, this);
            saydata.WaitTillSayFinished();
        }
        /// <summary>
        /// Set the x or left and right of the sprite.
        /// The bigger the number the farther right.
        /// The smaller the number the farther left.
        /// </summary>
        /// <param name="setto">The x or left and right of the sprite to set the sprites x to.</param>
        public void SetXTo(int setto)
        {
            Rectangle OriginalValue = rect;
            rect.X = setto;
            RectChanged(OriginalValue);
        }
        /// <summary>
        /// Set the x or left and right of the sprite.
        /// The bigger the number the farther right.
        /// The smaller the number the farther left.
        /// </summary>
        /// <param name="setto">The x or left and right of the sprite to set the sprites x to.</param>
        public void SetXTo(double setto)
        {
            Rectangle OriginalValue = rect;
            rect.X = (int)setto;
            RectChanged(OriginalValue);
        }
        /// <summary>
        /// Set the y or up and down of the sprite.
        /// The bigger the number the farther down.
        /// The smaller the number the farther up.
        /// </summary>
        /// <param name="setto">The y or left and right of the sprite to set the sprites y to.</param>
        public void SetYTo(int setto)
        {
            Rectangle OriginalValue = rect;
            rect.Y = setto;
            RectChanged(OriginalValue);
        }
        /// <summary>
        /// Set the y or up and down of the sprite.
        /// The bigger the number the farther down.
        /// The smaller the number the farther up.
        /// </summary>
        /// <param name="setto">The y or left and right of the sprite to set the sprites y to.</param>
        public void SetYTo(double setto)
        {
            Rectangle OriginalValue = rect;
            rect.Y = (int)setto;
            RectChanged(OriginalValue);
        }
        /// <summary>
        /// Increase the x or left and right of the sprite.
        /// Positive numbers are right.
        /// Negative numbers are left.
        /// </summary>
        /// <param name="changeby">The x or left and right of the sprite to change it by.</param>
        public void ChangeXBy(int changeby)
        {
            Rectangle OriginalValue = rect;
            rect.X += changeby;
            RectChanged(OriginalValue);
        }
        /// <summary>
        /// Increase the x or left and right of the sprite.
        /// Positive numbers are right.
        /// Negative numbers are left.
        /// </summary>
        /// <param name="changeby">The x or left and right of the sprite to change it by.</param>
        public void ChangeXBy(double changeby)
        {
            Rectangle OriginalValue = rect;
            rect.X += (int)changeby;
            RectChanged(OriginalValue);
        }
        /// <summary>
        /// Increase the y or up and down of the sprite.
        /// Positive numbers are down.
        /// Negative numbers are up.
        /// </summary>
        /// <param name="changeby">The y or up and down of the sprite to change it by.</param>
        public void ChangeYBy(int changeby)
        {
            Rectangle OriginalValue = rect;
            rect.Y += changeby;
            RectChanged(OriginalValue);
        }
        /// <summary>
        /// Increase the y or up and down of the sprite.
        /// Positive numbers are down.
        /// Negative numbers are up.
        /// </summary>
        /// <param name="changeby">The y or up and down of the sprite to change it by.</param>
        public void ChangeYBy(double changeby)
        {
            Rectangle OriginalValue = rect;
            rect.Y += (int)changeby;
            RectChanged(OriginalValue);
        }
        /// <summary>
        /// If on the edge of the screen bounces off.
        /// </summary>
        public void IfOnEdgeBounce()
        {
            if (!game.GraphicsDevice.Viewport.Bounds.Contains(rect))
            {
                Rectangle OriginalValue = rect;
                rect = ConvertToRectangle(-ConvertToVector2(rect), rect);
                RectChanged(OriginalValue);
            }
        }
        /// <summary>
        /// Activates when this sprite is clicked.
        /// </summary>
        public event Action Clicked;
        /// <summary>
        /// The x or left and right of the current sprite.
        /// The smaller the number the farther left.
        /// The bigger the number the farther right.
        /// </summary>
        public int x { get { return rect.X; } private set { rect.X = value; } }
        /// <summary>
        /// The y or up and down of the current sprite.
        /// The smaller the number the farther up.
        /// The bigger the number the farther down.
        /// </summary>
        public int y { get { return rect.Y; } private set { rect.Y = value; } }
        /// <summary>
        /// The angle of the sprite or direction.
        /// </summary>
        public float direction { get { return rotation * (180 / ((float)Math.PI)); } private set { rotation = value * ((float)Math.PI / 180f); } }
        internal static Vector2 ConvertToVector2(Rectangle rect)
        {
            return new Vector2(rect.X, rect.Y);
        }
        internal static Vector2 GetSize(Rectangle rect)
        {
            return new Vector2(rect.Width, rect.Height);
        }
        internal static Rectangle ConvertToRectangle(Vector2Object vec2, Rectangle from_size)
        {
            return new Rectangle(vec2.x, vec2.y, from_size.Width, from_size.Height);
        }
        internal static Rectangle ConvertToRectangle(Vector2 vec2, Rectangle from_size)
        {
            return ConvertToRectangle(new Vector2Object(vec2), from_size);
        }
        internal static Vector2 MinusPos(Rectangle pos1, Rectangle pos2)
        {
            return ConvertToVector2(pos1) - ConvertToVector2(pos2);
        }
        internal static Vector2 PlusPos(Rectangle pos1, Rectangle pos2)
        {
            return ConvertToVector2(pos1) + ConvertToVector2(pos2);
        }
        internal static Vector2 MinusSize(Rectangle pos1, Rectangle pos2)
        {
            return GetSize(pos1) - GetSize(pos2);
        }
        internal static Vector2 PlusSize(Rectangle pos1, Rectangle pos2)
        {
            return GetSize(pos1) + GetSize(pos2);
        }
        /// <summary>
        /// Turns right a certain amount of degrees.
        /// </summary>
        /// <param name="degrees">The amount of degrees to turn right.</param>
        public void TurnRight(float degrees)
        {
            PointInDirection(degrees + ((rotation / (float)Math.PI) * 180));
        }
        /// <summary>
        /// Turns left a certain amount of degrees.
        /// </summary>
        /// <param name="degrees">The amount of degrees to turn left.</param>
        public void TurnLeft(float degrees)
        {
            TurnRight(-degrees);
        }
        /// <summary>
        /// Moves a cerain amount of steps.
        /// </summary>
        /// <param name="steps">The amount of steps to move forward.</param>
        public void Move_Steps(float steps)
        {
            double x = Math.Sin(Convert.ToDouble(rotation));
            double y = -Math.Cos(Convert.ToDouble(rotation));
            Rectangle OriginalValue = rect;
            rect.X += (int)(x * steps);
            rect.Y += (int)(y * steps);
            RectChanged(OriginalValue);
        }
        /// <summary>
        /// Points a certain degrees.
        /// </summary>
        /// <param name="degrees">The degrees to point to.</param>
        public void PointInDirection(float degrees)
        {
            rotation = (float)((Math.PI / 180.0) * ((double)(degrees)));
        }
        static Vector2 RotateAboutOrigin(Vector2 point, Vector2 origin, float rotation)
        {
            return Vector2.Transform(point - origin, Matrix.CreateRotationZ(rotation)) + origin;
        }

        /// <summary>
        /// Draws the sprite.
        /// </summary>
        /// <param name="world">The world to draw the sprite from.</param>
        internal void Draw()
        {
            float xnaRotation = rotation - (float)(Math.PI / 2);
            Vector2 halfTextureSize = new Vector2(picture.Width / 2, picture.Height / 2);
            Vector2 rotatedHalfTextureSize = RotateAboutOrigin(halfTextureSize, Vector2.Zero, xnaRotation);
            Rectangle finalRect = ConvertToRectangle(ConvertFromScratchPosToXnaPos(ConvertToVector2(rect)), rect);
            finalRect.X += (int)(halfTextureSize.X - rotatedHalfTextureSize.X);
            finalRect.Y += (int)(halfTextureSize.Y - rotatedHalfTextureSize.Y);
            finalRect = ConvertToRectangle(new Vector2Object(finalRect.Center), finalRect);
            world.batch.Draw(picture, finalRect, null, color, xnaRotation, new Vector2(), SpriteEffects.None, 0f);

            if (saydata.IsValid())
            {
                saydata.Draw(world, rect);
            }
            BonusContent.Draw();
        }
        /// <summary>
        /// Updates the sprite.
        /// </summary>
        /// <param name="world">The world to updates the sprite from.</param>
        internal void Update()
        {
            glidedata.UpdateValues();
            if (Clicked != null)
            {
                MouseState state = Mouse.GetState();
                if (state.LeftButton == ButtonState.Pressed)
                    if (rect.Contains(state.X, state.Y))
                        Clicked();
            }
            if (saydata.IsValid())
            {
                saydata.Update();
            }
            if (glidedata.framesleft > 0)
            {
                Rectangle OriginalValue = rect;
                rect = ConvertToRectangle(glidedata.perframe + ConvertToVector2(rect), rect);
                RectChanged(OriginalValue);
            }
            BonusContent.Update();
        }
    }
}
