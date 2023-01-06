using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib
{
    public class DataSet
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public List<DataStructure> Recordings { get; set; } = new List<DataStructure> { };

        public DataSet() { Name = ""; Description = ""; }

        public DataSet(string name, string description, List<DataStructure> recordings)
        {
            Name = name;
            Description = description;
            Recordings = recordings;
        }
    }
}
