using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mudo
{
    class Player_An: Animator
    {
        private Keys left, right;
        public Player_An(Keys left, Keys right)
        {
            this.left = left;
            this.right = right;
        }

        public void UpdateAccell(ref Vector2 curr, Vector2 position, GameTime time)
        {
            KeyboardState st = Keyboard.GetState();
            float x = 0;
            if (st.IsKeyDown(left)) x = -0.95f;
            else if (st.IsKeyDown(right)) x = 0.95f;

            curr = new Vector2(x, curr.Y);
        }
    }
}
