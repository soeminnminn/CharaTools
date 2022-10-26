using System;
using MessagePack;

namespace CharaTools.AIChara
{
    [MessagePackObject]
    public class Vector2
    {
        // X component of the vector.
        [Key(0)]
        public float x;
        // Y component of the vector.
        [Key(1)]
        public float y;

        // Access the /x/ or /y/ component using [0] or [1] respectively.
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2 index!");
                }
            }

            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector2 index!");
                }
            }
        }

        // Constructs a new vector with given x, y components.
        public Vector2(float x, float y) { this.x = x; this.y = y; }

        public static readonly Vector2 zero = new Vector2(0F, 0F);
        public static readonly Vector2 one = new Vector2(1F, 1F);
    }
}
