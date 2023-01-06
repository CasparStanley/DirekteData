﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLib
{
    public class DataStructure
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; private set; }

        [ForeignKey("FK_DataSet_Recordings")]
        public int DataSetId { get; set; }

        // TODO: Make time a float
        public int Time { get; set; }
        public string Rotation { get; set; }

        public DataStructure() { Rotation = ""; }

        public DataStructure(int time, string rotation, int dataSetId = 0)
        {
            Time = time;
            Rotation = rotation;

            DataSetId = dataSetId;
        }
    }
}