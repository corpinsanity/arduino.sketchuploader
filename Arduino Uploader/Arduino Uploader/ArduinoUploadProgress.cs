using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arduino_Uploader
{
    class ArduinoUploadProgress : IProgress<double>
    {
        public void Report(double value)
        {
            var convertedValue = Math.Ceiling(value * 100) + "%";
            Console.Title = "Progress: " + convertedValue;
        }
    }
}
