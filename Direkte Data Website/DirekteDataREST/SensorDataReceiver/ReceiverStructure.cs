using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorDataReceiver
{
    public abstract class ReceiverStructure
    {
        public abstract int Port { get; set; }

        public abstract void StartReceiver();
    }
}
