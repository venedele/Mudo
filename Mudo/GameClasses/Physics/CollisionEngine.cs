using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mudo
{
    class CollisionEngine : List<PhysObject>
    {

        private bool insidecollision = true;
        public CollisionEngine(PhysObject main, bool insidecollision = true) : base()
        {
            this.Add(main);
            this.insidecollision = insidecollision;
        }

        public void Update(GameTime gameTime)
        {
            if (this.Count > 1)
            {
                PhysObject main = this.First();
                bool movingx = main.velocity.X != 0;
                bool movingy = main.velocity.Y != 0;
                foreach (PhysObject curr in this.Skip(1))
                {
                    bool movx = movingx || curr.velocity.X != 0;
                    bool movy = movingy || curr.velocity.Y != 0;

                    if (movx)
                    {
                        //Collision specifically designed for balls
                        if (insidecollision)
                        {
                            if ((main.Xposition_right >= curr.Xposition_left && main.Xposition_left <= curr.Xposition_right) && (curr.location.Y >= main.Yposition_top && curr.location.Y <= main.Yposition_bottom))
                            {
                                curr.Collision(main, false, true);
                            }
                        } else {
                            if((main.Xposition_left >= curr.Xposition_left || main.Xposition_right <= curr.Xposition_right))
                            {
                                curr.Collision(main, false, true);
                            }
                        }
                    }
                    if (movy)
                    {
                        if (insidecollision)
                        {
                            if ((main.Yposition_bottom >= curr.Yposition_top && main.Yposition_top <= curr.Yposition_bottom) && (curr.location.X >= main.Xposition_left && curr.location.X <= main.Xposition_right))
                            {
                                curr.Collision(main, true, true);
                            }
                        } else
                        {
                            if ((main.Yposition_top >= curr.Yposition_top || main.Yposition_bottom <= curr.Yposition_bottom))
                            {
                                curr.Collision(main, true, true);
                            }
                        }
                    }
                }
            }
        }

    }
}
