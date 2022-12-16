using System.Numerics;

namespace ModelLib
{
    public class DataStructure
    {
        // TODO: Make time a float
        public int Time { get; set; }
        public int Speed { get; set; }
        public string Rotation { get; set; }

        public DataStructure()
        {

        }

        public DataStructure(int time, int speed, string rotation)
        {
            Time = time;
            Speed = speed;
            Rotation = rotation;
        }
    }
}