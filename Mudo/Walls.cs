using Microsoft.Xna.Framework;

namespace Mudo
{
    class Walls : PhysObject
    {
        public Walls(int height, int width) : base(null, height, width, true)
        {
            mass = 9000;
        }
    }
}
