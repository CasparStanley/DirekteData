using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLib
{
    public class DataStructure
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; private set; }

        [ForeignKey("FK_Recordings_DataSets")]
        public int DataSetId { get; set; }

        // TODO: Make time a float
        public float Time { get; set; }
        public string Rotation { get; set; }

        public DataStructure() { Rotation = ""; }

        public DataStructure(float time, string rotation, int dataSetId = 0)
        {
            Time = time;
            Rotation = rotation;

            DataSetId = dataSetId;
        }

        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id.ToString()}, {nameof(DataSetId)}={DataSetId.ToString()}, {nameof(Time)}={Time.ToString()}, {nameof(Rotation)}={Rotation}}}";
        }
    }
}