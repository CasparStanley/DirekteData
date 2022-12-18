using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ModelLib
{
    public class DataStructure
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; private set; }

        public int Data_Set_ID { get; set; }

        // TODO: Make time a float
        public int Time { get; set; }
        public int Speed { get; set; }
        public string Rotation { get; set; }

        public DataStructure()
        {

        }

        public DataStructure(int time, int speed, string rotation, int dataSetID = 0)
        {
            Time = time;
            Speed = speed;
            Rotation = rotation;

            Data_Set_ID = dataSetID;
        }
    }
}