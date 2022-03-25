using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoControls.Containers.Base;

namespace MonoControls.Containers.Helpers.Animatables
{
    class DuplexStateAnimatable : Animatable
    {
        public Texture2D texture_alt { get; private set; }
        public Color color_alt { get; private set; }//new Color(new Vector3(256, -1, -1));
        public float alpha_alt { get; private set; }
        public bool changable_color { get; private set; } = false;

        public bool SecondState { get; private set; } = false;
        public bool Created { get; private set; } = false;
        private bool[] swapped = { false, false, false };

        public DuplexStateAnimatable(Texture2D texture, float x, float y, int width, int height, Color color, float rotation = 0f, LinkedList<Animatable> parents = null):base(texture, x, y,width, height, color, rotation, parents) {}

        public DuplexStateAnimatable(Texture2D texture, Vector2 location, Point size, Color color, float rotation = 0, LinkedList<Animatable> parents = null) : base(texture, location, size, color, rotation, parents) {}

        public DuplexStateAnimatable(SpriteFont spriteFont, String str, Vector2 location, Color color, int containerwidth = 0, int containerheight = 0, float rotation = 0, LinkedList<Animatable> parents = null): base(spriteFont, str, location, color, containerwidth, containerheight, rotation, parents) {}

        public DuplexStateAnimatable(SpriteFont spriteFont, String str, float x, float y, Color color, int containerwidth = 0, int containerheight = 0, float rotation = 0, LinkedList<Animatable> parents = null) :base(spriteFont, str, x, y, color, containerwidth, containerheight, rotation, parents){}

        protected DuplexStateAnimatable(){}
        public DuplexStateAnimatable CreateState(Texture2D texture_alt, Vector4 color_alt, float alpha_alt)
        {
            this.texture_alt = texture_alt;
            if (color_alt.X > -1)
            {
                this.color_alt = new Color(color_alt);
                changable_color = true;
            }
            this.alpha_alt = alpha_alt;
            Created = true;
            return this;
        }

        public virtual void SwapStates(bool recursive)
        {
            if (recursive)
                foreach (DuplexStateAnimatable child in this)
                {
                    try
                    { child.SwapStates(true); } catch { }
                }
            if(Created)
            {
                if(SecondState)
                {
                    if(swapped[0])
                    {
                        Texture2D temp = texture;
                        texture = texture_alt;
                        texture_alt = temp;
                    }   
                    if(swapped[1])
                    {
                        Color temp = color;
                        color = color_alt;
                        color_alt = temp;
                    }

                    if(swapped[2])
                    {
                        alpha += alpha_alt;
                        alpha_alt = alpha - alpha_alt;
                        alpha = alpha - alpha_alt;
                    }
                    for (int a = 0; a < swapped.Length; a++)
                        swapped[a] = false;
                } else
                {
                    if(texture_alt != null)
                    {
                        Texture2D temp = texture;
                        texture = texture_alt;
                        texture_alt = temp;
                        swapped[0] = true;
                    }
                    if (changable_color)
                    {
                        Color temp = color;
                        color = color_alt;
                        color_alt = temp;
                        swapped[1] = true;
                    }
                    if(alpha_alt >-1)
                    {
                        alpha += alpha_alt;
                        alpha_alt = alpha - alpha_alt;
                        alpha = alpha - alpha_alt;
                        swapped[2] = true;
                    }
                }
                SecondState = !SecondState;
            }

        }

    }
}
