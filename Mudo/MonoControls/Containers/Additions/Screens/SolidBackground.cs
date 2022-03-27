using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoControls.Containers.Base;

namespace MonoControls.Containers.Helpers.Screens
{
    class SolidBackground : Screen
    {
        Animatable background = null;
        String loadneeded = null;

        public SolidBackground(Game context, string textureresource, float alpha, GraphicsDevice device) : base(context)
        {
            loadneeded = textureresource;
            background = new Animatable(null, new Vector2(0, 0), new Point(), new Color(Color.White,alpha*255));
            //graphicsDevice = device;
        }

        public SolidBackground(Game context, Animatable background, GraphicsDevice device) : base(context)
        {
            this.background = background;
            //graphicsDevice = device;
        }

        public SolidBackground(Game context, Texture2D back, GraphicsDevice device,  float alpha) : base(context)
        {
            background = new Animatable(back, new Vector2(0, 0), new Point(), new Color(Color.White, alpha*255));
            //graphicsDevice = device;
        }

        public SolidBackground(Game context, Color gradientstart, Color step, GraphicsDevice gr, int gradientresoulution, float alpha = 1, bool vertical = true): base(context)
        {
            //graphicsDevice = gr;
            gradientresoulution *= 10;
            Texture2D back = new Texture2D(gr, vertical ? 10 : gradientresoulution, vertical ? gradientresoulution : 10);
            Color[] data = new Color[back.Width * back.Height];
            if(vertical)
            {
                for (int y = 0; y < back.Height; y++) {
                    for (int x = 0; x < back.Width; x++)
                    {
                        data[y * back.Width + x] = gradientstart;
                    }
                    gradientstart = new Color(gradientstart.ToVector3()+step.ToVector3());
                }
            } else
            {

            }
            back.SetData(data);
            background = new Animatable(back, new Vector2(0, 0), new Point(), new Color(Color.White, alpha * 255));
        }

        public SolidBackground(Game context, Color gradientstart, Color endcolor, int gradientresolution, GraphicsDevice gr, float alpha = 1, bool vertical = true) : base(context)
        {
            //graphicsDevice = gr;
            gradientresolution *= 10;
            Vector3 gradientstartv = gradientstart.ToVector3();
            Vector3 step = ((endcolor.ToVector3() - gradientstart.ToVector3()) / gradientresolution);
            Texture2D back = new Texture2D(gr, vertical ? 10 : gradientresolution, vertical ? gradientresolution : 10);
            Vector3[] data = new Vector3[back.Width * back.Height];
            if (vertical)
            {
                for (int y = 0; y < back.Height; y++)
                {
                    for (int x = 0; x < back.Width; x++)
                    {
                        data[y * back.Width + x] = gradientstartv;
                    }
                    gradientstartv += step;
                }
            }
            else
            {

            }
            Color[] data2 = new Color[back.Width * back.Height];
            for (int x = 0; x < data2.Length; x++)
                data2[x] = new Color(data[x]);
            back.SetData(data2);
            background = new Animatable(back, new Vector2(0, 0), new Point(), new Color(Color.White, alpha * 255));
        }

        public SolidBackground(Game context, Color solidColor, float alpha) : base(context)
        {
            Texture2D back = new Texture2D(graphicsDevice, 10, 10);
            Color[] solid = new Color[10*10];
            for (int x = 0; x < 10 * 10; x++)
                solid[x] = solidColor;
            back.SetData(solid);
            background = new Animatable(back, new Vector2(0, 0), new Point(), new Color(Color.White, alpha*255));
            //graphicsDevice = device;
        }

        protected override void Resource_Load(Microsoft.Xna.Framework.Content.ContentManager content_l = null)
        {
            if (background.texture == null)
                background.texture = content_l.Load<Texture2D>(loadneeded);
            background.size = (background.texture.Height<background.texture.Width)?new Point((int)((graphicsDevice.Viewport.Height / (float)background.texture.Height) * background.texture.Width), graphicsDevice.Viewport.Height):graphicsDevice.Viewport.Bounds.Size;
            background.location = new Vector2((graphicsDevice.Viewport.Width - background.size.X) / 2, 0);
        }
        protected override void Current_Update(GameTime gameTime)
        {
            
        }
        protected override void Current_Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch, new Vector2(0, 0));
        }
        protected override void Dispose()
        {

        }
    }
}
