using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mudo
{
    class Ai_An : Animator
    {
        Ball follow = null;
        public Ai_An(Ball follow) {
            this.follow = follow;
        }

        public void UpdateAccell(ref Vector2 curr, Vector2 position, GameTime time)
        {
            curr = new Vector2(0.006f*(follow.location.X-position.X), 0);
        }
    }
}
