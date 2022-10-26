using System;
using MessagePack;

namespace CharaTools.AIChara
{
    [MessagePackObject]
    public class Vector3
    {
        [Key(0)]
        public float x;
        [Key(1)]
        public float y;
        [Key(2)]
        public float z;

        public Vector3()
        { }

        public Vector3(float x, float y, float z)
        {
            this.x = x; this.y = y; this.z = z;
        }

        public Vector3(float x, float y)
        {
            this.x = x; this.y = y; z = 0F;
        }

        public void Set(float newX, float newY, float newZ)
        {
            x = newX; y = newY; z = newZ;
        }

        public static readonly Vector3 zero = new Vector3(0F, 0F, 0F);
        public static readonly Vector3 one = new Vector3(1F, 1F, 1F);
    }
}
