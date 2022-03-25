/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;*/
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoControls.Inputs.Mouse;

namespace MonoControls.Containers.Base
{
    abstract class Screen
    {
        public bool active = true;

        public Screen parent { private set; get; } = null;

        private bool custom_root_compliable_v = false;
        public bool custom_root_compliable
        {
            get { return custom_root_compliable_v; }
            set { custom_root_compliable_v = value; OnCustomVectorChange(customroot_v, custom_root_compliable_v); }
        }
        private Vector2 customroot_v = new Vector2(0, 0);
        public Vector2 customroot
        {
            get { return customroot_v; }
            set { if(nested != null?nested.custom_root_compliable_v:false)nested.customroot = value; customroot_v = value; OnCustomVectorChange(customroot_v, custom_root_compliable); }
        }

        protected virtual void OnCustomVectorChange(Vector2 CustomVector, bool custom_vector_compliable)
        { }

        protected Mouse_Handler mouse = null;
        public bool mouseBlocked { get; private set; } = false;
        public void CreateMouseHandler()
        {
            mouse = new Mouse_Handler();
        }

        public virtual Screen MouseState(bool blockMouse)
        {
            mouseBlocked = blockMouse;
            return this;
        }

        public bool automaticallyLoadNext = true;

        protected bool initialised = false;
        public bool Initialised
        {
            set { if (initialised ^ (initialised = value)) { if (initialised) { if (child != null) child.Resource_Load(content); Resource_Load(content); } else { if (child != null) child.Dispose();  Dispose(); } }  }
            get { return initialised; }
        }
        public Pair<Microsoft.Xna.Framework.Content.ContentManager, GraphicsDevice> Load {
            set { if ((value.a != content || GraphicsDevice!=value.b || !initialised) && value!=null)
                { graphicsDevice = value.b; content = value.a; initialised = true; if(child!=null)child.Load = value; Resource_Load(content); }  }
        }
        public bool paused = false;
        protected Microsoft.Xna.Framework.Content.ContentManager content;
        protected GraphicsDevice GraphicsDevice;
        public GraphicsDevice graphicsDevice
        {
            set { GraphicsDevice = value; }
        }
        protected Screen child = null;
        public Screen nested
        {
            get { return child; }
            set { if(child!=null)child.parent = null; child = value; if (child != null) { child.parent = this; if (automaticallyLoadNext && content!=null) child.Load = new Pair<Microsoft.Xna.Framework.Content.ContentManager, GraphicsDevice>(content, GraphicsDevice); } }
        }
        public Screen Detach()
        {
            parent = null;
            return this;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if(child!=null)child.Draw(spriteBatch);
            Current_Draw(spriteBatch);
        }
        public void Update(GameTime gameTime)
        {
            if (!paused)
            {
                if (!(mouseBlocked || mouse == null))
                    mouse.Update();
                if (child != null) child.Update(gameTime, mouseBlocked);
                Current_Update(gameTime);
            }
        }

        public void Update(GameTime gameTime, bool mouseblocked)
        {
            if (!paused)
            {
                if (!(mouseBlocked||mouseblocked || mouse == null))
                    mouse.Update();
                if (child != null) child.Update(gameTime, mouseblocked);
                Current_Update(gameTime);
            }
        }

        protected abstract void Resource_Load(Microsoft.Xna.Framework.Content.ContentManager content_l = null);
        protected abstract void Current_Update(GameTime gameTime);
        protected abstract void Current_Draw(SpriteBatch spriteBatch);
        protected abstract void Dispose();

    }
}
