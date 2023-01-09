using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib
{
    public class LiveDataHolder
    {
        public static int SelectedDataSetId { get; set; } = 1;
        public static DataStructure Recording { get; set; } = new DataStructure();

        public static void UpdateCurrentRecording(DataStructure recording)
        {
            Recording = recording;
        }
    }
}
