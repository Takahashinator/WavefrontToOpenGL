using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obj2OpenGlFileHeader
{
    public class Face
    {
        public int V;
        public int Vt;
        public int Vn;

        public Face(int v, int vt, int vn)
        {
            V = v;
            Vt = vt;
            Vn = vn;
        }
    }
}
