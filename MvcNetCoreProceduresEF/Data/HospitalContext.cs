using Microsoft.EntityFrameworkCore;
using MvcNetCoreProceduresEF.Models;

namespace MvcNetCoreProceduresEF.Data
{
    public class HospitalContext: DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options)
            : base(options) { }
        public DbSet<Trabajador> Trabajadores { get; set; }
        public DbSet<VistaEmpleado> VistasEmpleados { get; set; }
        public DbSet<Enfermo> Enfermos { get; set; }
    }
}
