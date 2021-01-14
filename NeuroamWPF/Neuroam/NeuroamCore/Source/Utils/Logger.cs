using System;

namespace NeuroamCore
{
    public class Logger
    {
        public static Logger Instance = new Logger();

        private Logger()
        {
        }

        void Write(string msg)
        {
            Console.WriteLine(msg);
        }

        public void Log(String msg)
        {
            Write($"[Log] {msg}");
        }

        public void LogException(String msg, Exception e)
        {
            Write($"[Exception] {msg}. Message: {e.Message}");
        }

        public void LogError(string msg)
        {
            Write($"[Error] {msg}");
        }

    }
}
