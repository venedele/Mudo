using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoControls.Containers.Base;

namespace MonoControls.Containers.Additions.Animatables
{
    internal class PulsingText : Animatable
    {
        protected String next = null;
        protected Interlopator anim = null;
        protected float alphascale = 0;
        protected float starting_alpha = 0;
        protected float starting_scale = 0;
        protected float scalechange = 0;
        protected float pulsetime = 0; 
        public String str
        {
            get { return base.str; }
            set {
                next = value;
                anim.Reset(); 
            }
        }


        public PulsingText(SpriteFont font, Vector2 location, float scale, float alpha, float final_alpha, Color color, float scalechange = 0.1f, float pulsetime = 20,  String initial = "") : base(font, initial, location, color)
        {
            Vector2 a = font.MeasureString(initial);
            this.size = a.ToPoint();
            this.Scale = new Vector2(scale, scale);
            this.alpha = alpha;
            this.starting_alpha = alpha;
            this.starting_scale = scale;
            this.scalechange = scalechange;
            this.pulsetime = pulsetime;
            alphascale = (final_alpha - alpha);
            anim = new Interlopator(delegate (float t) {
                float value = -t*(t-pulsetime)/(pulsetime*pulsetime/4);
                if (value > 0) return value; else return 0;
            });
        }

        public void Update(GameTime time)
        {
            if (next != null || anim.started)
            {
                if(next != null)
                    base.str = next;
                    this.size = spriteFont.MeasureString(base.str).ToPoint();
                next = null;
                float value = anim.Update(time);
                this.Scale = new Vector2(starting_scale, starting_scale) + new Vector2(starting_scale * scalechange * value, starting_scale * scalechange * value);
                this.alpha = starting_alpha + alphascale * value;
            }
        }

    }
}
