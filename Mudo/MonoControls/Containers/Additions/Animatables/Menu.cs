using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoControls.Containers.Base;
using Microsoft.Xna.Framework;
using MonoControls.Inputs.Mouse;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace MonoControls.Containers.Helpers.Animatables
{
    class Menu : DuplexStateAnimatable
    {
        public bool spread = true;
        int selected_member = 0;
        public int Selected_member
        {
            get { return selected_member; }
            set {
                DuplexStateAnimatable a = (DuplexStateAnimatable)(this.ElementAt(selected_member));
                if (a.SecondState) a.SwapStates(true);
                selected_member = value;
                a = (DuplexStateAnimatable)(this.ElementAt(selected_member));
                if (!a.SecondState) a.SwapStates(true);
            }
        }
        private bool selected = false;
        public bool Selected
        {
            get { return selected; }
            set { if (value) Selected_member = selected_member; else selected_member = -1; selected = value; }
        }

        public enum Type
        {
            Horizontal,
            Vertical,
            Table
        }

        public Type type;

        Point size_a = new Point();

        public Queue<Mouse_Event> Mouse_events { get; private set; } = new Queue<Mouse_Event>();
        public Mouse_Event menu_event = null;

        private short OnHover(Animatable sender, bool hovered)
        {
            ((DuplexStateAnimatable)sender).SwapStates(sender == this ? false : true);
            return 0;
        }

        public void CretateMouseControls(Mouse_Handler mouse, bool createmenuevent = true)
        {
            foreach (Mouse_Event mevent in Mouse_events)
            {
                mouse.Add(mevent, true);
            }
            if(createmenuevent)
            mouse.Add(menu_event, false);
        }

        public float startoffset { get; private set; } = 0; public float endoffset { get; private set; } = 0;

        public Menu(Type type, float x, float y, int width, int height, Color color, LinkedList<Pair<DuplexStateAnimatable, Func<Animatable, MouseKeys, short>>> parents, bool spread = true, float rotation = 0, float startoffset = 0, float endoffset = 0, bool generate = true)
        {
            this.startoffset = startoffset; this.endoffset = endoffset;
            id = new Random().Next();
            this.type = type; location = new Vector2(x, y); size = new Point(width, height); this.color = color; this.alpha = color.A / 255; color.A = 255; Rotation = rotation;
            menu_event = this.AddMouseEvents(null, OnHover);
            foreach (Pair<DuplexStateAnimatable, Func<Animatable, MouseKeys, short>> a in parents)
                { this.AddLast(a.a.SetParent(this)); size_a += a.a.GetContainerSize(); Mouse_events.Enqueue(a.a.AddMouseEvents(a.b, OnHover)); }
            this.spread = spread;
            if(generate)
            GenerateLayout();
        }

        public Menu(Type type, Vector2 location, Point size, Color color, LinkedList<Pair<DuplexStateAnimatable, Func<Animatable, MouseKeys, short>>> parents, bool spread = true, float rotation = 0, float startoffset = 0, float endoffset = 0, bool generate = true)
        {
            this.startoffset = startoffset; this.endoffset = endoffset;
            id = new Random().Next();
            this.type = type; this.location = location; this.size = size; this.color = color; this.alpha = color.A / 255; color.A = 255; Rotation = rotation;
            menu_event = this.AddMouseEvents(null, OnHover);
            foreach (Pair<DuplexStateAnimatable, Func<Animatable, MouseKeys, short>> a in parents)
                { this.AddLast(a.a.SetParent(this)); size_a += a.a.GetContainerSize(); Mouse_events.Enqueue(a.a.AddMouseEvents(a.b, OnHover)); }
            this.spread = spread;
            if(generate)
            GenerateLayout();
        }

        protected Menu(Type type, Vector2 location, Point size, Color color, LinkedList<Animatable> parents, Queue<Mouse_Event> parents_m, bool spread = true, float rotation = 0f, float startoffset = 0, float endoffset = 0, bool generate = true)
        {
            this.startoffset = startoffset; this.endoffset = endoffset;
            id = new Random().Next();
            this.type = type; this.location = location; this.size = size; this.color = color; Rotation = rotation;
            Mouse_events = parents_m;
            menu_event = this.AddMouseEvents(null, OnHover);
            foreach (DuplexStateAnimatable a in parents)
            {
                this.AddLast(a.SetParent(this)); size_a += a.GetContainerSize();
            }
            this.spread = spread;
            if(generate)
            GenerateLayout();
        }

        public void GenerateLayout()
        {
            Vector2 previous = new Vector2(0, 0);
            float offset = 0;
            switch(type)
            {
                case Type.Horizontal:
                    previous.X = startoffset;
                    offset = spread?(this.Count>1?(this.GetContainerSize().X-endoffset-size_a.X) / (this.Count-1):0):0;
                    break;
                case Type.Vertical:
                    previous.Y = startoffset;
                    offset = spread?(this.Count>1?(this.GetContainerSize().Y - endoffset - size_a.Y) / (this.Count-1):0):0;
                    break;
            }
            foreach (Animatable a in this)
            {
                switch (type) {
                    case Type.Horizontal:
                        a.X = previous.X;
                        a.Y = (this.GetContainerSize().Y - a.size.Y) / 2;
                        previous.X += (spread ? offset : 0) + a.size.X;
                        break;
                    case Type.Vertical:
                        a.Y = previous.Y;
                        a.X = (this.GetContainerSize().X - a.size.X) / 2;
                        previous.Y += (spread?offset:0) + a.size.Y;
                        break;
                }
            }
        }

        public override void SwapStates(bool recursive)
        {
            base.SwapStates(recursive);
        }

    }
}
