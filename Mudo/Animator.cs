using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mudo
{
    interface Animator
    {
        void UpdateAccell(ref Vector2 curr, Vector2 position, GameTime time);
    }
}
