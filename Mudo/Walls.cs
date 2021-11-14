using Microsoft.Xna.Framework;

namespace Mudo
{
    class Walls : VObject
    {
        public Walls(Game game, int height, int width) : base(game, null, height, width, true)
        {
            mass = 9000;
        }
    }
}
