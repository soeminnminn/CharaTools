using System;

namespace CharaTools.AIChara
{
    internal static class Mathf
    {
        // Clamps a value between a minimum float and maximum float value.
        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
            return value;
        }

        // Clamps value between min and max and returns value.
        // Set the position of the transform to be that of the time
        // but never less than 1 or more than 3
        //
        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
            return value;
        }

        // Clamps value between 0 and 1 and returns value
        public static float Clamp01(float value)
        {
            if (value < 0F)
                return 0F;
            else if (value > 1F)
                return 1F;
            else
                return value;
        }

        // Interpolates between /a/ and /b/ by /t/. /t/ is clamped between 0 and 1.
        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Clamp01(t);
        }

        public static float InverseLerp(float a, float b, float value)
        {
            if (a != b)
                return Clamp01((value - a) / (b - a));
            else
                return 0.0f;
        }
    }
}
