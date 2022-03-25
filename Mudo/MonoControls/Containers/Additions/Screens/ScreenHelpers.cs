using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoControls.Containers.Base;

namespace MonoControls.Containers.Helpers.Screens
{
    static class ScreenHelpers
    {
        public static void AddLast(Screen s, Screen queue)
        {
            while (queue.nested != null)
            { queue = queue.nested;  }
            queue.nested = s;
        }

        public static void AddAt(Screen s, Screen queue, int index)
        {
            if (index == 0) { AddFirst(s, queue); return; }
            Screen start = queue;
            for (; index>1; index--)
                queue = queue.nested;
            Screen temp = queue.nested;
            AddLast(temp, s);
            queue.nested = s;
        }

        public static void AddFirst(Screen s, Screen queue)
        {
            if(queue.parent != null)
                queue.parent.nested = s;
            AddLast(queue, s);
        }
        public static int Count(Screen queue)
        {
            for (int x = 0; ; queue = queue.nested)
                if(queue.nested==null)return x;
        }

        public static Screen PurgeInactive(Screen s, bool fixindexing)
        {
            if (fixindexing) s = FixIndexing(s);
            s = PurgeFirstInactive(s, false);
            Screen p = s;
            while(p!=null?p.nested!= null:false)
            {
                if (!p.nested.active) { p.nested = PurgeFirstInactive(p.nested, false); }
                p = p.nested;
            }
            return s;
        }

        public static void ForceLoad(Screen queue, Microsoft.Xna.Framework.Content.ContentManager content, Microsoft.Xna.Framework.Graphics.GraphicsDevice gr)
        {
            while(queue != null)
            {
                if (!queue.Initialised) queue.Load = new Pair<Microsoft.Xna.Framework.Content.ContentManager, Microsoft.Xna.Framework.Graphics.GraphicsDevice>(content, gr);
                queue = queue.nested;
            }
        }

        public static Screen FixIndexing(Screen queue)
        {
            for (; queue.parent != null; queue = queue.parent);
            return queue;
        }

        public static Screen PurgeFirstInactive(Screen s, bool fixindexing)
        {
            if (fixindexing) s = FixIndexing(s);
            while (s == null ? false : !s.active)
            {
                s = s.nested;
            }
            if(s.parent!=null)
            s.parent.nested = null;
            return s;
        }
    }
}
