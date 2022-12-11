using System.Numerics;

namespace ModelLib
{
    public class DataStructure
    {
        public int Time { get; set; }
        public int Speed { get; set; }
        public Vector3 Rotation { get; set; }

        public DataStructure()
        {

        }

        public DataStructure(int time, int speed, Vector3 rotation)
        {
            Time = time;
            Speed = speed;
            Rotation = rotation;
        }
    }
}