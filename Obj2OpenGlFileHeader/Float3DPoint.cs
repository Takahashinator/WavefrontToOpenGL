using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obj2OpenGlFileHeader
{
    public class Float3DPoint
    {
        public float X;
        public float Y;
        public float Z;

        public Float3DPoint(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return X + ", " + Y + ", " + Z + ", ";
        }
    }
}
