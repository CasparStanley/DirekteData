using DirekteDataREST.Controllers;
using DirekteDataREST.Managers;

namespace DirekteDataREST.SensorReceiver
{
    public abstract class ReceiverStructure
    {
        public abstract int Port { get; set; }

        public abstract void RunReceiver();
    }
}
