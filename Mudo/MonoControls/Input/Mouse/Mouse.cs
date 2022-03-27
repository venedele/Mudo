using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoControls.Containers.Base;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoControls.Inputs.Mouse
{
    class Mouse_Handler
    {
        public bool custom_root_compliable = false;
        Point customroot = new Point(0, 0);
        Point mouseposition2;
        Texture2D now;
        Texture2D mousetexture;
        Texture2D mousetextureclick;
        Texture2D mousetexturemiddle;
        Point mousesize = new Point(20, 20);
        Point mouseposition;
        private LinkedList<Mouse_Event> mouseevents = new LinkedList<Mouse_Event>();
        LinkedListNode<Mouse_Event> priority_last = null;
        public MouseKeys pressed = new MouseKeys();

        public void SetCustomRootreference(Vector2 customrootref, bool custom_root_compliable)
        {
            customroot = new Point((int)customrootref.X, (int)customrootref.Y);
            this.custom_root_compliable = custom_root_compliable;
        }

        public static MouseKeys GetPressed()
        {
            MouseState state = Microsoft.Xna.Framework.Input.Mouse.GetState();
            MouseKeys keys = new MouseKeys();
            keys.left = state.LeftButton == ButtonState.Pressed; keys.right = state.RightButton == ButtonState.Pressed; keys.middle = state.MiddleButton == ButtonState.Pressed;
            return keys;
        }

        public Mouse_Handler()
        {
            mouseposition = Microsoft.Xna.Framework.Input.Mouse.GetState().Position;
        }

        public Mouse_Handler(Pair<Mouse_Event, bool>[] evental_regions = null)
        {
            mouseposition = Microsoft.Xna.Framework.Input.Mouse.GetState().Position;
            mouseposition2 = mouseposition - (custom_root_compliable ? customroot : new Point(0, 0));
            if (evental_regions != null)
                foreach(Pair<Mouse_Event, bool> evental_region in evental_regions)
                {
                    if(evental_region.b == true)
                    {
                        mouseevents.AddFirst(evental_region.a);
                        if (priority_last == null) priority_last = mouseevents.Find(evental_region.a);
                    }
                    if (priority_last == null) mouseevents.AddFirst(evental_region.a);
                    else mouseevents.AddAfter(priority_last, evental_region.a);
                }
        }

        public Mouse_Handler(Texture2D mouse_texture, Texture2D mouse_texture_click, Texture2D mouse_texture_middle, Point mousesize, Pair<Mouse_Event, bool>[] evental_regions = null)
        {
            mousetexture = mouse_texture; if (mousesize != null) this.mousesize = mousesize;
            mousetextureclick = mouse_texture_click;
            mousetexturemiddle = mouse_texture_middle;
            mouseposition = Microsoft.Xna.Framework.Input.Mouse.GetState().Position;
            mouseposition2 = mouseposition - (custom_root_compliable ? customroot : new Point(0, 0));

            if (evental_regions != null)
                foreach (Pair<Mouse_Event, bool> evental_region in evental_regions)
                {
                    if (evental_region.b == true)
                    {
                        mouseevents.AddFirst(evental_region.a);
                        if (priority_last == null) priority_last = mouseevents.Find(evental_region.a);
                    }
                    if (priority_last == null) mouseevents.AddFirst(evental_region.a);
                    else mouseevents.AddAfter(priority_last, evental_region.a);
                }
        }

        public void Add(Mouse_Event mevent, bool priority)
        {
            if (priority == true)
            {      
                mouseevents.AddFirst(mevent);
                if (priority_last == null) priority_last = mouseevents.Find(mevent);
            }
            if (priority_last == null) mouseevents.AddFirst(mevent);
            else mouseevents.AddAfter(priority_last, mevent);
        }

        /*public void Add(Pair<Mouse_Event, bool> evental_region)
        {
            if (evental_region.b == true)
            {
                mouseevents.AddFirst(evental_region.a);
                if (priority_last == null) priority_last = mouseevents.Find(evental_region.a);
            }
            if (priority_last == null) mouseevents.AddFirst(evental_region.a);
            else mouseevents.AddAfter(priority_last, evental_region.a);
        }*/

        public void Update()
        {
            MouseState now = Microsoft.Xna.Framework.Input.Mouse.GetState();
            MouseKeys old = pressed;
            pressed = GetPressed();

            if (pressed.middle)
                this.now = mousetexturemiddle ?? mousetexture;
            else if (pressed.left || pressed.right)
                this.now = mousetextureclick ?? mousetexture;
            else this.now = mousetexture;

            Point mouseposition_l = now.Position - (custom_root_compliable ? customroot : new Point(0, 0));
            if (!mouseposition2.Equals(mouseposition_l))
            {
                mouseposition = now.Position;
                mouseposition2 = mouseposition_l;
                bool mouse_on = false, mouse_off = false;
                foreach(Mouse_Event mouseevent in mouseevents)
                {
                    Rectangle region = mouseevent.region;
                    bool inside = (mouseposition_l.X > region.X && mouseposition_l.X < region.X + region.Width) ? (mouseposition_l.Y > region.Y && mouseposition_l.Y < region.Y + region.Height) : false;
                    if((mouseevent.Hover && !inside))
                    {
                        mouseevent.OnHoverSet(false);
                        mouse_off = true;
                        if (mouse_on) break;
                    } else if(!mouseevent.Hover&&inside)
                    {
                        if(!mouse_on)
                        mouseevent.OnHoverSet(true);
                        mouse_on = true;
                        if (mouse_off) break;
                    }
                }
            }

            if (!old.Equals(pressed))
            {
                foreach (Mouse_Event mouseevent in mouseevents)
                    if(mouseevent.Hover)
                    {
                        mouseevent.OnClickUpdate();
                        break;
                    }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(now != null)
            spriteBatch.Draw(now, new Rectangle(mouseposition, mousesize), Color.White);
        }

        private void Event_Generator()
        {

        }

    }
}
