using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class SensorUpdateBuilder
    {
        public string Message
        {
            get;
            private set;
        }

        public SensorUpdateBuilder()
        {
            Clear();
        }

        public void Clear()
        {
            Message = "sensor-update";
        }

        public void AddMessage( string key, string value )
        {
            Message += " \"" + key + "\" " + value;
        }
    }
}
