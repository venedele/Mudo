using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mudo
{
    class Platform:PhysObject
    {
        public Animator controller;

        int texture_variant = 0;

        public Platform(Animator controller, PhysObject ball_collision, int variant = 0) : base(null, 0, 0)
        {
            this.controller = controller;
            air_k = new Vector2(-0.1f, 0);
            this.mass = 1000;
            this.bounce = 1.1f;
            this.coll.Add(ball_collision);
            this.texture_variant = variant;
        }

        //TODO: Read Ball.cs TODO
        public void LoadContent(Game g)
        {
            this.setTexture(g.GraphicsDevice, texture_variant);
            //base.LoadContent();
        }

        public void setTexture(GraphicsDevice gr, int variant = 0)
        {
            this.texture = new Texture2D(gr, 122, 18);

            Color[] data = new Color[122 * 18];
            if (variant == 1)
                for (int i = 0; i < data.Length; ++i) if ((i % 9 < 4) || i < 244 || i > 1952) data[i] = Color.Black; else data[i] = Color.Orange;
            else if (variant == 2)
                for (int i = 0; i < data.Length; ++i) if ((i % 11 < 5) || i < 244 || i > 1952) data[i] = Color.Black; else data[i] = Color.Orange;
            else
                for (int i = 0; i < data.Length; ++i) if ((i % 10 < 4) || i < 244 || i > 1952) data[i] = Color.Black; else data[i] = Color.Orange;
            this.texture.SetData(data);
        }

        public override void Update(GameTime time)
        {
            controller.UpdateAccell(ref acceleration, location, time);
            base.Update(time);
        }
    }
}
