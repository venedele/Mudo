using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoControls.Containers.Base;
using MonoControls.Containers.Helpers.Animatables;
using MonoControls.Inputs.Mouse;

namespace MonoControls.Containers.Helpers.Menues
{
    class MenuScreen : Screen
    {
        float alpha = 0f;
        LinkedList<Menu> container = new LinkedList<Menu>();
        Interlopator r = null;

        public MenuScreen(Game context, Menu[] contains, Interlopator a = null, float alpha = 0f, bool custom_root_compliable = true): base(context)
        {
            this.alpha = alpha;
            r = a;
            foreach(Menu m in contains)
            {
                container.AddLast(m);
            }
            this.custom_root_compliable = custom_root_compliable;
            CreateMouseHandler();
            mouse.SetCustomRootreference(customroot, custom_root_compliable);
        }

        protected override void Resource_Load(Microsoft.Xna.Framework.Content.ContentManager content_l = null)
        {
            foreach (Menu menu in container)
                menu.CretateMouseControls(mouse);
        }

        bool looped = true;

        protected override void OnCustomVectorChange(Vector2 ch, bool compliable)
        {
            if(mouse != null)
            mouse.SetCustomRootreference(ch, compliable);
        }

        protected override void Current_Update(GameTime gameTime)
        {
            if (looped)
            {
                if(r!=null)
                alpha = r.Update(gameTime);
                if (alpha >= 1f) { alpha = 1f; looped = false;  }
            }
        }

        protected override void Current_Draw(SpriteBatch spriteBatch)
        {
            foreach(Menu b in container)
            {
                b.Draw(spriteBatch, custom_root_compliable?customroot:new Vector2(0, 0), alpha);
            }
        }

        protected override void Dispose()
        {

        }
    }
}
