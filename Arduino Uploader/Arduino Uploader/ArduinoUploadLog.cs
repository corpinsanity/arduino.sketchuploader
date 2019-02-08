using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArduinoUploader;

namespace Arduino_Uploader
{
    public class ArduinoUploadLog : IArduinoUploaderLogger
    {
        public void Error(string message, Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR: " + message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("WARNING: " + message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Debug(string message)
        {
            
        }

        public void Trace(string message)
        {
            
        }
    }
}
