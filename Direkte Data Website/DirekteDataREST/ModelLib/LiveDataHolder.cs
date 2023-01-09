using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib
{
    public static class LiveDataHolder
    {
        public static int SelectedDataSetId { get; set; }
        public static DataStructure Recording { get; set; } = new DataStructure();

        public static void UpdateCurrentRecording(DataStructure recording)
        {
            Recording = recording;
        }
    }
}
