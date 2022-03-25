using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MonoControls.Inputs.Mouse
{
    public struct MouseKeys
    {
        public bool left;
        public bool right;
        public bool middle;
    }
    class Mouse_Event
    {
        protected static int given_id = 0;
        
        public Rectangle region;
        public int id { get; }
        private bool hover;
        public bool Hover { get { return hover; } }
        private MouseKeys need_reset = new MouseKeys();
        private MouseKeys pressed = new MouseKeys();
        private Func<MouseKeys, short> OnKeyChange;
        private Func<bool, short> OnHoverChange;
        public Mouse_Event(Rectangle region, Func<MouseKeys, short> OnKeyChange, Func<bool, short> OnHoverChange)
        {
            this.region = region;
            id = given_id++;
            this.OnKeyChange = OnKeyChange;
            this.OnHoverChange = OnHoverChange;
            MouseState state = Microsoft.Xna.Framework.Input.Mouse.GetState();
            need_reset.left = state.LeftButton == ButtonState.Pressed;
            need_reset.right = state.RightButton == ButtonState.Pressed;
            need_reset.middle = state.MiddleButton == ButtonState.Pressed;
            pressed.left = false;
            pressed.right = false;
            pressed.middle = false;
        }
        public void OnClickUpdate()
        {
            MouseKeys b = pressed;
            MouseState state = Microsoft.Xna.Framework.Input.Mouse.GetState();
            pressed.left = state.LeftButton == ButtonState.Pressed;
            pressed.right = state.RightButton == ButtonState.Pressed;
            pressed.middle = state.MiddleButton == ButtonState.Pressed;
            need_reset.left &= pressed.left;
            need_reset.right &= pressed.right;
            need_reset.middle &= pressed.middle;
            pressed.left &= !need_reset.left;
            pressed.right &= !need_reset.right;
            pressed.middle &= !need_reset.middle;
            if(!pressed.Equals(b))
            OnKeyChange?.Invoke(pressed);
        }
        public void OnHoverSet(bool set)
        {
            if (hover ^ set)
            {
                if (set)
                {
                    MouseState state = Microsoft.Xna.Framework.Input.Mouse.GetState();
                    pressed.left = state.LeftButton == ButtonState.Pressed;
                    pressed.right = state.RightButton == ButtonState.Pressed;
                    pressed.middle = state.MiddleButton == ButtonState.Pressed;
                    need_reset.left = state.LeftButton == ButtonState.Pressed;
                    need_reset.right = state.RightButton == ButtonState.Pressed;
                    need_reset.middle = state.MiddleButton == ButtonState.Pressed;
                }
                else
                {
                    MouseKeys backup = pressed;
                    pressed.left = false;
                    pressed.right = false;
                    pressed.middle = false;
                    if (!backup.Equals(pressed))
                        OnKeyChange(pressed);
                }
                OnHoverChange?.Invoke(set);
            }
            hover = set;
        }
    }
}
