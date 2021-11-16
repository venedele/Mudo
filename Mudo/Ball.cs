using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mudo
{
    class Ball : VObject 
    {
        public int defeat1 { get; private set; } = 0;
        public int defeat2 { get; private set; } = 0;
        private Walls wall_ref = null;
        private bool score_or = false;

        public Ball(Game game, Texture2D texture, int width = 40, int height = 40) : base(game, texture, width, height)
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

        protected override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("acc");
            base.LoadContent();
        }

        public override void Collision(VObject collisioned, bool orientation, bool anti_clipping = false)
        {
            if (Object.ReferenceEquals(collisioned, wall_ref) && orientation == score_or)
            {
                if (orientation)
                    if (velocity.Y > 0) defeat1++; else defeat2++;
                else
                    if (velocity.X > 0) defeat2++; else defeat1++;
            }
            base.Collision(collisioned, orientation, anti_clipping);    
        }
    }
}
