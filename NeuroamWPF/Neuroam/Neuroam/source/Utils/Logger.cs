using System;

namespace Neuroam
{
    public class Logger
    {
        public static Logger Instance = new Logger();

        private Logger()
        {
        }

        public void Write(String msg)
        {
            Console.WriteLine(msg);
        }
        public void Write(String msg, Exception e)
        {
            Write(msg);
            Write(e.Message);
        }
    }
}
