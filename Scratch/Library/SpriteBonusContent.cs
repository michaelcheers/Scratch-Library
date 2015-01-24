using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scratch
{
    namespace BonusContent
    {
        public partial class SpriteBonusContent
        {
            public enum Settings
            {
                Default = 0,
                Draggable = 1
            }
            public Settings SETTINGS
            {
                get
                {
                    return settings;
                }
                set
                {
                    settings = value;
                }
            }
            Settings settings;
            Sprite value;
            public SpriteBonusContent(Sprite value)
            {
                this.value = value;
            }
            public void Draw()
            {

            }
            public void Update()
            {
                if ((settings & Settings.Draggable) == Settings.Draggable)
                {
                    MouseState state = Mouse.GetState();
                    if (state.LeftButton == ButtonState.Pressed)
                    {
                        if (value.rect.Contains(state.X, state.Y))
                        {
                            if (isDragging)
                            {
                                value.rect = Sprite.ConvertToRectangle(new Vector2(state.X, state.Y) - new Vector2(lastDraggingX, lastDraggingY), value.rect);
                            }
                            else
                                isDragging = true;
                            lastDraggingX = state.X;
                            lastDraggingY = state.Y;
                        }
                    }
                    else if (isDragging)
                    {
                        isDragging = false;
                    }
                }
            }
        }
    }
}
