using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mudo
{
    class CollisionEngine : List<VObject>
    {

        private bool insidecollision = true;
        public CollisionEngine(VObject main, bool insidecollision = true) : base()
        {
            this.Add(main);
            this.insidecollision = insidecollision;
        }

        public void Update(GameTime gameTime)
        {
            if (this.Count > 1)
            {
                VObject main = this.First();
                bool movingx = main.velocity.X != 0;
                bool movingy = main.velocity.Y != 0;
                foreach (VObject curr in this.Skip(1))
                {
                    bool movx = movingx || curr.velocity.X != 0;
                    bool movy = movingy || curr.velocity.Y != 0;

                    if (movx)
                    {
                        //Collision specifically designed for balls
                        if (insidecollision)
                        {
                            if ((main.RightX >= curr.LeftX && main.LeftX <= curr.RightX) && (curr.position.Y >= main.TopY && curr.position.Y <= main.BottomY))
                            {
                                curr.Collision(main, false, true);
                            }
                        } else {
                            if((main.LeftX >= curr.LeftX || main.RightX <= curr.RightX))
                            {
                                curr.Collision(main, false, true);
                            }
                        }
                    }
                    if (movy)
                    {
                        if (insidecollision)
                        {
                            if ((main.BottomY >= curr.TopY && main.TopY <= curr.BottomY) && (curr.position.X >= main.LeftX && curr.position.X <= main.RightX))
                            {
                                curr.Collision(main, true, true);
                            }
                        } else
                        {
                            if ((main.TopY >= curr.TopY || main.BottomY <= curr.BottomY))
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
