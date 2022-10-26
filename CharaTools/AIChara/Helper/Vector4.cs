using System;
using MessagePack;

namespace CharaTools.AIChara
{
    [MessagePackObject]
    public class Vector4
    {
        [Key(0)]
        public float x;
        [Key(1)]
        public float y;
        [Key(2)]
        public float z;
        [Key(3)]
        public float w;

        public Vector4()
        { }

        public Vector4(float x, float y, float z, float w)
        {
            this.x = x; this.y = y; this.z = z; this.w = w;
        }

        public Vector4(float x, float y, float z)
        {
            this.x = x; this.y = y; this.z = z; this.w = 0F;
        }

        public Vector4(float x, float y)
        {
            this.x = x; this.y = y; this.z = 0F; this.w = 0F;
        }
    }
}
