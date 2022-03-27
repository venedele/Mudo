using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoControls.Containers.Base
{
    class Quadruplets<T, T1, T2, T3>
    {
        public T a;
        public T1 b;
        public T2 c;
        public T3 d;
        public Quadruplets(T a, T1 b, T2 c, T3 d)
        {
            this.a = a; this.b = b; this.c = c; this.d = d;
        }
    }
    class Triplet<T,T1,T2>
    {
        public T a;
        public T1 b;
        public T2 c;
        public Triplet(T a, T1 b, T2 c)
        {
            this.a = a; this.b = b; this.c = c;
        }
    }
    class Pair<T, T1>
    {
        public T a;
        public T1 b;
        public Pair(T a, T1 b)
        {
            this.a = a; this.b = b;
        }
    }
}
