using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mudo
{
    class Ball : VObject 
    {
        public Ball(Texture2D texture, int width, int height) : base(texture, width, height)
        {
            mass = 10; min_speed = 0;
            rotation_coef = 9f;
            this.min_y = 4f;
            this.air_rot_k = -0.0015f;
            air_k = new Vector2(-0.0008f, -0.0008f);
        }
    }
}
