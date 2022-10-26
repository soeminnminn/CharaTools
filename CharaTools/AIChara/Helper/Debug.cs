using System;
using System.IO;

namespace CharaTools.AIChara
{
    public static class Debug
    {
        private static long prevPos = 0;
#if DEBUG
        public static bool DebugLogging = true;
#else
        public static bool DebugLogging = false;
#endif

        public static void Log(string message)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(message);
#endif
        }

        public static void LogWarning(string warning)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(warning);
#endif
        }

        public static void LogError(string message)
        {
#if DEBUG
            System.Diagnostics.Debug.Fail(message);
#endif
        }

        public static void LogError(Exception exception)
        {
#if DEBUG
            System.Diagnostics.Debug.Fail(exception.Message, exception.StackTrace);
#endif
        }

        public static void Assert(bool condition)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(condition);
#endif
        }

        public static void Assert(bool condition, string message)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(condition, message);
#endif
        }

        public static void LogSize(Stream stream)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"pos : {stream.Position}, size : {stream.Position - prevPos}");
            prevPos = stream.Position;
#endif
        }
    }
}
