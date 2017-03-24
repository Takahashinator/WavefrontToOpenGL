using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obj2OpenGlFileHeader
{
    public class ThreesomeNode
    {
        public int Postion;
        public int Normal;
        public int Color;
        public int Index;

        public ThreesomeNode(int pos, int norm, int col, int ind)
        {
            Postion = pos;
            Normal = norm;
            Color = col;
            Index = ind;
        }

    }
}
