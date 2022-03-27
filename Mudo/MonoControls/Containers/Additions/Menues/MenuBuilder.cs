/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;*/
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using MonoControls.Containers.Base;
using MonoControls.Containers.Helpers.Animatables;
using Microsoft.Xna.Framework;

namespace MonoControls.Containers.Helpers.Menues
{
    static class MenuBuilder 
    {
        public static MenuScreen Build_Interlopated_Back(Game context, Menu main, Screen Background, Interlopator a, Queue<Menu> menus = null)
        {
            Menu[] menu_c = new Menu[1+(menus==null?0:menus.Count)];
            menu_c[0] = main;
            if (menus != null) {
                menus.CopyTo(menu_c, 1);
            }
            MenuScreen menu = new MenuScreen(context, menu_c, a, 0f);
            menu.nested = Background;
            return menu;
        }

        public static MenuScreen Build_Shaped(Game context, Menu main, bool mainshaped, Screen background, Interlopator a = null, float alpha = 1, Queue<Pair<Menu,bool>> menus = null)
        {
            Menu[] menu_c = new Menu[1 + (menus == null ? 0 : menus.Count)];
            menu_c[0] = mainshaped?new MenuShaped(main):main;
            if (menus != null)
            {
                Queue<Pair<Menu, bool>> tempme = new Queue<Pair<Menu, bool>>(menus);
                for (int x = 1; menus.Count > 0; x++)
                {
                    Pair<Menu, bool> temp = tempme.Dequeue();
                    menu_c[x] = temp.b ? new MenuShaped(temp.a) : temp.a;
                }
            }
            MenuScreen menu = new MenuScreen(context, menu_c, a, alpha);
            menu.nested = background;
            return menu;
        }

    }
}

