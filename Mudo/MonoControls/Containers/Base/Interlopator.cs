using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoControls.Containers.Base
{
    class Interlopator
    {
        double start = -1;
        private Func<float, float> function;
        double wait_milis; int total_ticks=0; float scale; float multiplier;
        public float changeable { get; private set; }
        private float changeable_start;
        public bool started
        {
            get { return start != -1; }
        }
        public Interlopator(Func<float, float> function, float changeable = 0, double wait_milis = 0, float multiplier = 1, float scale = 1)
        {
            this.function = function;
            this.wait_milis = wait_milis;
            this.scale = scale;
            this.changeable = changeable;
            this.changeable_start = changeable;
            this.multiplier = multiplier;
        }

        public float Update(GameTime current)
        {
            
            if (start == -1)
                start = current.TotalGameTime.TotalMilliseconds;
            double total = (current.TotalGameTime.TotalMilliseconds - start);
            if (total > wait_milis)
            {
                return changeable = changeable_start + multiplier * function.Invoke((++total_ticks) / scale);
            }
            return changeable;
            //TODO: Changes speed with targetfps. Workaround: Use desiredfps/targetfps coefficient in order to stabalise. 
        }

        public void Reset(float changeable = float.NaN)
        {
            total_ticks = 0;
            if (!(changeable != changeable))
            {
                this.changeable = changeable;
                this.changeable_start = changeable;
            }
            start = -1;
        }

        public void SetStartingInterval(double wait_milis)
        {
            this.wait_milis = wait_milis;
        }

        public static Interlopator GetPredefined(Predefined interlop, float changeable = 0, double wait_millis = 0, float multiplier = 1, float scale = 1)
        {
            Func<float, float> result = null;
            switch (interlop) {
                case Predefined.Accelerate:
                    result = delegate (float x)
                    {
                        return x * x/5;
                    };
                break;
                case Predefined.Decelerate:
                    result = delegate (float x)
                    {
                        return -x * x / 5;
                    };
                break;
                case Predefined.LinearUp:
                    result = delegate (float x)
                    {
                        return 1*x;
                    };
                    break;
                case Predefined.LinearDown:
                    result = delegate (float x)
                    {
                        return -1*x;
                    };
                    break;
                case Predefined.Constant:
                    result = delegate (float x)
                    {
                        return 1;
                    };
                    break;
                default:
                    throw new FormatException();
            }
            return new Interlopator(result, changeable, wait_millis, multiplier, scale);
        }

        public enum Predefined
        {
            Accelerate,
            Decelerate,
            LinearUp,
            LinearDown,
            Constant,
        }
    }
}
