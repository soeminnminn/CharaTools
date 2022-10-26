using System;
using MessagePack;

namespace CharaTools.AIChara
{
    [MessagePackObject]
    public class Color
    {
        [Key(0)]
        public float r;

        [Key(1)]
        public float g;

        [Key(2)]
        public float b;

        [Key(3)]
        public float a;

        public Color()
        { }

        public Color(float r, float g, float b, float a)
        {
            this.r = r; this.g = g; this.b = b; this.a = a;
        }

        public Color(float r, float g, float b)
        {
            this.r = r; this.g = g; this.b = b; this.a = 1.0F;
        }

        // Interpolates between colors /a/ and /b/ by /t/.
        public static Color Lerp(Color a, Color b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Color(
                a.r + (b.r - a.r) * t,
                a.g + (b.g - a.g) * t,
                a.b + (b.b - a.b) * t,
                a.a + (b.a - a.a) * t
            );
        }

        // Interpolates between colors /a/ and /b/ by /t/ without clamping the interpolant
        public static Color LerpUnclamped(Color a, Color b, float t)
        {
            return new Color(
                a.r + (b.r - a.r) * t,
                a.g + (b.g - a.g) * t,
                a.b + (b.b - a.b) * t,
                a.a + (b.a - a.a) * t
            );
        }

        public static Color white { get { return new Color(1F, 1F, 1F, 1F); } }
        public static Color black { get { return new Color(0F, 0F, 0F, 1F); } }

        public static Color red { get { return new Color(1F, 0F, 0F, 1F); } }
        public static Color green { get { return new Color(0F, 1F, 0F, 1F); } }
        public static Color blue { get { return new Color(0F, 0F, 1F, 1F); } }
    }
}
