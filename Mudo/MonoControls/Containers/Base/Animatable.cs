using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoControls.Inputs.Mouse;

namespace MonoControls.Containers.Base
{
    //TODO: Inherit DrawableGameComponent if possible
    class Animatable : LinkedList<Animatable>
    {

        public int id;

        private Vector2 scale = new Vector2(1, 1);

        public Vector2 Scale
        {
            get { return scale; }
            set { if (texture == null) scale = value; else throw new NotImplementedException("Currently scale changing is not supported for Textured Animatables."); }
        }

        public Mutex drawing = new Mutex();

        public void UpdateScale()
        {
            if (texture != null)
            {
                Point temp = (sub_size.X == 0 ? size : sub_size);
                scale = new Vector2(temp.X / (float)(texture.Width), temp.Y / (float)(texture.Height));
            }
            else scale = new Vector2(1f, 1f);
        }

        public SpriteEffects effects = SpriteEffects.None;
        public float alpha = 1;
        public Animatable parent = null;
        public bool size_locked = false;
        private Texture2D texture_c = null;
        public Texture2D texture
        {
            get { return texture_c;  }
            set { texture_c = value; UpdateScale(); }
        }
        protected SpriteFont spriteFont = null;
        public String str = null;
        public Color color;
        private Vector2 location_c;
        public Vector2 location {
            get { return location_c; }
            set { location_c = value; if (event_handler != null) UpdateMouseevent(); foreach (Animatable chi in this) if (chi.event_handler != null) chi.UpdateMouseevent(); }
        }
        private Point size_c;
        public Point size
        {
            get { return (size_c == Point.Zero && texture_c!=null)?texture_c.Bounds.Size:size_c; }
            set { size_c = value; if (event_handler != null) UpdateMouseevent(); foreach (Animatable chi in this) if (chi.event_handler != null) chi.UpdateMouseevent(); UpdateScale(); }
        }


        public int width
        {
            get { return size.X; }
            protected set { size_c.X = value; }
        }

        public int height
        {
            get { return size.Y; }
            protected set { size_c.Y = value; }
        }


        public float Xposition_left
        {
            get { return location.X - (this.centerCoords?(width / 2.0f):0); }
        }

        public float Xposition_right
        {
            get { return location.X + (this.centerCoords ? (width / 2.0f) : 0); }
        }

        public float Yposition_top
        {
            get { return location.Y - (this.centerCoords ? height / 2.0f:0); }
        }

        public float Yposition_bottom
        {
            get { return location.Y + (this.centerCoords ? height / 2.0f:0); }
        }


        Vector2 sub_location = new Vector2(0, 0);
        Point sub_size = new Point(0, 0);
        private float rotation = 0.0f;

        private Mouse_Event event_handler = null;

        public float X
        {
            get { return location.X;  }
            set { location_c.X = value; if (event_handler != null) UpdateMouseevent(); foreach (Animatable chi in this) if (chi.event_handler != null) chi.UpdateMouseevent(); }
        }

        public float Y
        {
            get { return location.Y; }
            set { location_c.Y = value; if (event_handler != null) UpdateMouseevent(); foreach (Animatable chi in this) if (chi.event_handler != null) chi.UpdateMouseevent(); }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value;}
        }

        public Animatable SetParent(Animatable parent)
        {
            this.parent = parent;
            return this;
        }

        public Vector2 GetGlobalLocation()
        {
            return location + ((parent == null) ? new Vector2(0, 0) : parent.GetGlobalLocation());
        }

        public void UpdateMouseevent()
        {
            Vector2 temp = GetGlobalLocation();
            event_handler.region = new Rectangle(new Point((int)temp.X, (int)temp.Y), GetSize());
        }

        public Animatable SetLocalCoords(Vector2 sub_location, Point sub_size)
        {
            this.sub_location = sub_location;
            this.sub_size = sub_size;
            UpdateScale();
            return this;
        }


        public Animatable (Texture2D texture, float x, float y, int width, int height, Color color, float rotation = 0f, LinkedList<Animatable> parents = null)
        {
            id = new Random().Next();
            this.texture = texture; location = new Vector2(x, y); size = new Point(width, height); Rotation = rotation;
            if(parents!=null)foreach (Animatable a in parents)
                this.AddLast(a.SetParent(this));
            alpha = color.A/(float)255;
            color.A = 255; this.color = color;
        }

        public Animatable (Texture2D texture, Vector2 location, Point size, Color color, float rotation = 0, LinkedList<Animatable> parents = null)
        {
            id = new Random().Next();
            this.texture = texture; this.location = location; this.size = size ; Rotation = rotation;
            if (parents != null) foreach (Animatable a in parents)
                { this.AddLast(a.SetParent(this)); }
            alpha = color.A/(float)255;
            color.A = 255; this.color = color;
        }

        public Animatable Add(Animatable child)
        {
            drawing.WaitOne();
            this.AddLast(child.SetParent(this));
            drawing.ReleaseMutex();
            return this;
        }

        public Animatable SetEffect(SpriteEffects eff)
        {
            effects = eff;
            return this;
        }

        private bool centerCoords = false;
        public bool isCenterCoord
        {
            get { return centerCoords; }
        }
        public void setCentralCoords(bool center_coords)
        {
            centerCoords = center_coords;
        }

        public Animatable Add(LinkedList<Animatable> parents)
        {
            drawing.WaitOne();
            if (parents != null) foreach (Animatable a in parents)
                { this.AddLast(a.SetParent(this)); }
            drawing.ReleaseMutex();
            return this;
        }
        public Animatable SetId(int id = 0)
        {
            this.id = id; 
            return this;
        }

        public Animatable (SpriteFont spriteFont, String str, Vector2 location, Color color, int containerwidth = 0, int containerheight = 0, float rotation = 0, LinkedList<Animatable> parents = null)
        {
            this.spriteFont = spriteFont; this.str = str; this.location = location;
            size = new Point(containerwidth, containerheight);
            if (parents != null) foreach (Animatable a in parents)
                { this.AddLast(a.SetParent(this)); }
            alpha = color.A / (float)255; color.A = 255; Rotation = rotation; this.color = color;
        }

        public Animatable(SpriteFont spriteFont, String str, float x, float y, Color color, int containerwidth = 0, int containerheight = 0, float rotation = 0, LinkedList<Animatable> parents = null)
        {
            this.spriteFont = spriteFont; this.str = str; this.location = new Vector2(x,y);
            size = new Point(containerwidth, containerheight);
            if (parents != null) foreach (Animatable a in parents)
                { this.AddLast(a.SetParent(this)); }
            alpha = color.A / (float)255; color.A = 255; Rotation = rotation; this.color = color;
        }

        protected Animatable()
        {

        }

        public Mouse_Event AddMouseEvents(Func<Animatable, MouseKeys, short> OnKeyChange, Func<Animatable, bool, short> OnHoverChange)
        {
            Vector2 temp = GetGlobalLocation();
            event_handler = new Mouse_Event(new Rectangle(new Point((int)temp.X, (int)temp.Y), GetSize()), delegate(MouseKeys mouse) { if (OnKeyChange != null) OnKeyChange.Invoke(this, mouse); return 0; }, delegate (bool hover) { if (OnHoverChange != null) OnHoverChange.Invoke(this, hover);  return 0; });
            return event_handler;
        }

        public Animatable AddMouseEvents(Func<Animatable, MouseKeys, short> OnKeyChange, Func<Animatable, bool, short> OnHoverChange, Mouse_Handler attachto, bool priority = false)
        {
            Vector2 temp = GetGlobalLocation();
            event_handler = new Mouse_Event(new Rectangle(new Point((int)temp.X, (int)temp.Y), GetSize()), delegate (MouseKeys mouse) { if(OnKeyChange!=null)OnKeyChange.Invoke(this, mouse); return 0; }, delegate (bool hover) { if (OnHoverChange != null) OnHoverChange.Invoke(this, hover); return 0; });
            attachto.Add(event_handler, priority);
            return this;
        }

        ~Animatable()
        {
            Dispose();
        }

        public virtual void Dispose()
        {

        }

        public Point GetSize()
        {
            return (sub_size.X == 0 ? size : sub_size);
        }

        public Point GetContainerSize()
        {
            return size;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 cord_root)
        {
            drawing.WaitOne();
            foreach (Animatable a in this)
            {
                a.Draw(spriteBatch, new Vector2(cord_root.X+location.X, cord_root.Y+location.Y), alpha); 
            }
            if(texture != null)
            {
                if (alpha > 0)
                {
                    Point temp = (sub_size.X == 0 ? size : sub_size);
                    Vector2 s = (cord_root + location + sub_location) + (this.centerCoords?Vector2.Zero:new Vector2(temp.X / 2, temp.Y / 2));
                    spriteBatch.Draw(texture, s, null, color * alpha, rotation, new Vector2(texture.Width / 2, texture.Height / 2),scale, effects, 1f);
                }
            }
            else if (spriteFont != null)
            {
                Point temp = (sub_size.X == 0 ? size : sub_size);
                if (alpha > 0)
                    spriteBatch.DrawString(spriteFont, str, cord_root + location + sub_location + (this.centerCoords ? Vector2.Zero : new Vector2(temp.X / 2f, temp.Y / 2f)), color * alpha, rotation, new Vector2(size.X / 2, size.Y / 2), scale, SpriteEffects.None, 0f);
            }
            drawing.ReleaseMutex();
        }
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 cord_root, float alphal = 1)
        {
            drawing.WaitOne();
            foreach (Animatable a in this)
            {
                a.Draw(spriteBatch, new Vector2(cord_root.X + location.X, cord_root.Y + location.Y), alphal*alpha);
            }
            if (texture != null)
            {
                if (alpha > 0)
                {
                    Point temp = (sub_size.X == 0 ? size : sub_size);
                    Vector2 s = (cord_root + location + sub_location) + (this.centerCoords ? Vector2.Zero : new Vector2(temp.X / 2, temp.Y / 2));
                    
                        spriteBatch.Draw(texture, s, null, color * (alphal * alpha), rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, effects, 1f);
                }
            }
            else if (spriteFont != null)
            {
                Point temp = (sub_size.X == 0 ? size : sub_size);
                if (alpha > 0)
                    spriteBatch.DrawString(spriteFont, str, cord_root + location + sub_location + (this.centerCoords ? Vector2.Zero : new Vector2(temp.X / 2f, temp.Y / 2f)), color * (alphal * alpha), rotation, new Vector2(size.X / 2, size.Y / 2), scale, SpriteEffects.None, 0f);
            }
            drawing.ReleaseMutex();
        }
    }
}
