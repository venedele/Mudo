using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoControls.Containers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoControls.Containers.Base
{
    //If Loading management is required, use the Screen class
    internal class AdvancedAnimatable : Animatable
    {

        public Game context;

        public AdvancedAnimatable(Game context, Vector2 location, Point size, Color color, float rotation = 0, LinkedList<Animatable> parents = null) : base(null, location, size, color, rotation, parents)
        {
            this.context = context;
        }


        public void Load ()
        {
            LoadContent(context.Content);
            foreach(Animatable child in this)
            {
                if(child.GetType().IsAssignableFrom(typeof(AdvancedAnimatable)))
                {
                    ((AdvancedAnimatable)child).Load();
                }
            }
        }

        protected virtual void LoadContent(ContentManager content)
        {
            
        }

        public virtual void Update(GameTime time)
        {
            foreach (Animatable child in this)
            {
                if (child.GetType().IsAssignableFrom(typeof(AdvancedAnimatable)))
                {
                    ((AdvancedAnimatable)child).Update(time);
                }
            }
        }
    }
}
