using FleetManagerWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetManagerWeb.Data
{
    public class FleetDbContext : DbContext
    {
        public FleetDbContext(DbContextOptions<FleetDbContext> options) : base(options) { }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<ServiceRecord> ServiceRecords { get; set; }
        public DbSet<Vacation> Vacations {  get; set; }
    }
}
