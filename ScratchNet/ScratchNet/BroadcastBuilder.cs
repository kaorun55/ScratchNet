using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class BroadcastBuilder
    {
        public string Message
        {
            get;
            private set;
        }

        public BroadcastBuilder()
        {
            Clear();
        }

        public void Clear()
        {
            Message = "broadcast";
        }

        public void AddValue( string value )
        {
            Message += " \"" + value + "\"";
        }
    }
}
