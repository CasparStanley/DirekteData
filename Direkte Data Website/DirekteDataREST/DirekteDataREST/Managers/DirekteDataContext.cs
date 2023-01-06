using Microsoft.EntityFrameworkCore;
using ModelLib;

namespace DirekteDataREST.Managers
{
    public class DirekteDataContext : DbContext
    {
        public DirekteDataContext(DbContextOptions<DirekteDataContext> options) : base(options) 
        { 

        }

        public DbSet<DataStructure> Recordings { get; set; }
        public DbSet<DataSet> DataSets { get; set; }
    }
}
