
using MyHalp.MyMath;

namespace UnityVoxelPlanet
{
    public struct Int3
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Int3(int x, int y, int z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public MyVector3 ToVector3()
        {
            return new MyVector3(X, Y, Z);
        }

        public static Int3 FromVector3(MyVector3 value)
        {
            return new Int3((int)value.X, (int)value.Y, (int)value.Z);
        }
    }
}
