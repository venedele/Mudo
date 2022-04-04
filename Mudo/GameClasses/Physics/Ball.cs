using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mudo
{
    class Ball : PhysObject
    {
        public int defeat1 { get; private set; } = 0;
        public int defeat2 { get; private set; } = 0;

        public event EventHandler Player1DefeatChanged;
        public event EventHandler Player2DefeatChanged;

        private Walls wall_ref = null;
        private bool score_or = false;

        public Ball(Game context, int width = 40, int height = 40) : base(context, width, height)
        {
            mass = 10; min_speed = 0;
            rotation_coef = 9f;
            this.min_y = 4f;
            this.air_rot_k = -0.0015f;
            air_k = new Vector2(-0.0008f, -0.0008f);
        }

        public void setScoring(Walls wall, bool score_orientation = false)
        {
            wall_ref = wall;
            score_or = score_orientation;
        }

        //TODO: LoadContent Temporary executed by main class, until a more leen solution is found
        protected override void LoadContent(ContentManager g)
        {
            texture = g.Load<Texture2D>("acc");
        }

        protected double scoreticks = 0;

        public override void Update(GameTime time)
        {
            scoreticks++;
            base.Update(time);
        }

        public override void Collision(PhysObject collisioned, bool orientation, bool anti_clipping = false)
        {
            if (Object.ReferenceEquals(collisioned, wall_ref) && orientation == score_or)
            {
                if (scoreticks > 10)
                {
                    if (orientation)
                        if (velocity.Y > 0) { defeat1++; Player1DefeatChanged?.Invoke(this, null); } else { defeat2++; Player2DefeatChanged?.Invoke(this, null); }
                    else
                        if (velocity.X > 0) { defeat2++; Player2DefeatChanged?.Invoke(this, null); } else { defeat1++; Player1DefeatChanged?.Invoke(this, null); }

                }
                scoreticks = 0;
            }
            base.Collision(collisioned, orientation, anti_clipping);    
        }
    }
}
