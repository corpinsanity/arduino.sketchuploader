using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArduinoUploader;
using System.Threading;
using System.Runtime.InteropServices;

namespace Arduino_Uploader
{
    class Program
    {
        private static ArduinoSketchUploader Uploader { get; set; }

        static void Main(string[] args)
        {
            Console.Title = "Arduino Sketch Uploader";
            Console.WindowHeight = 50;
            Console.WindowWidth = 75;
            Console.SetBufferSize(75, 50);
            SetWindowPos(MyConsole, 0, 950, 300, 0, 0, SWP_NOSIZE);

            var options = new ArduinoSketchUploaderOptions();
            string comPort = "COM7";
            if (!Directory.Exists(@"C:\\Arduino\Presets\"))
                Directory.CreateDirectory(@"C:\\Arduino\Presets\");

            Console.Write("Use default COM7? (Y/N) ");
            if (Console.ReadKey().Key.ToString().ToLower() != "y")
            {
                Console.Clear();
                Console.WriteLine("Please select the COM port from the list below");
                DrawLines(46, 10);
                DisplayPorts();
                Console.WriteLine();
                Console.Write("-> ");
                var menOpt = Convert.ToInt32(Console.ReadKey().KeyChar.ToString());
                comPort = SerialPort.GetPortNames()[menOpt];
            }
            Console.Clear();

            Console.WriteLine("Please select the file you want to upload from the list below");
            DrawLines(61, 2);

            var paths = Directory.GetFiles(@"C:\\Arduino\Presets\");
            var dict = new Dictionary<string, string>();

            foreach (var path in paths)
                dict.Add(Path.GetFileName(path), path);

            DisplayFiles(dict);
            Console.WriteLine();
            Console.Write("-> ");
            var fileSelect = Convert.ToInt32(Console.ReadKey().KeyChar.ToString());
            Console.Clear();

            Console.SetCursorPosition((Console.WindowWidth - 11) / 2, Console.CursorTop);
            Console.WriteLine("Final check");
            DrawLines(75, 15);

            string anyKeyText = "Press any key to start uploading";

            Console.SetCursorPosition(23, Console.CursorTop);
            Console.WriteLine("COM Port: " + comPort);
            Console.SetCursorPosition(18, Console.CursorTop);
            Console.WriteLine("Arduino Model: NANO R3");
            Console.SetCursorPosition(25, Console.CursorTop);
            Console.WriteLine("Sketch: " + dict.Keys.ElementAt(fileSelect).Remove(dict.Keys.ElementAt(fileSelect).Length - 4));
            Console.WriteLine();
            Console.WriteLine();
            Console.SetCursorPosition((Console.WindowWidth - anyKeyText.Length) / 2, Console.CursorTop);
            Console.WriteLine(anyKeyText);
            Console.SetCursorPosition(37, Console.CursorTop);
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("Uploading sketch");
            Console.Title = "Progress: 0%";
            DrawLines(75, 30);
            options.FileName = dict.Values.ElementAt(fileSelect);
            options.PortName = comPort;
            options.ArduinoModel = ArduinoUploader.Hardware.ArduinoModel.NanoR3;

            var logger = new ArduinoUploadLog();
            var progressReporter = new ArduinoUploadProgress();
            Uploader = new ArduinoSketchUploader(options, logger, progressReporter);

            Uploader.UploadSketch();

            DrawLines(24, 30);
            Console.WriteLine("Done");
            Console.ReadKey();
        }

        public static void DisplayFiles(Dictionary<string, string> combo)
        {
            int index = 0;
            foreach (var file in combo)
            {
                Console.WriteLine(String.Format("{0}) {1}", index, file.Key.Remove(file.Key.Length - 4)));
                index++;
            }
        }

        public static void DisplayPorts()
        {
            var ports = SerialPort.GetPortNames();
            int index = 0;
            foreach (string port in ports)
            {
                Console.WriteLine(String.Format("{0}) {1}", index, port));
                index++;
            }
        }

        public static void DrawLines(int amount, int sleepTime)
        {
            for (int i = 0; i < amount; i++)
            {
                Thread.Sleep(sleepTime);
                Console.Write("-");
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        #region SetDefaultWinPos
        const int SWP_NOSIZE = 0x0001;

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        private static IntPtr MyConsole = GetConsoleWindow();

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        #endregion
    }
}
