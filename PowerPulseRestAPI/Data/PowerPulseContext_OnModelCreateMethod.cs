using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.UsersModels;


namespace PowerPulseRestAPI.Data
{
    public partial class PowerPulseContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PowerPulseContext).Assembly);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().HasData(
        new Role
        {
            Id = 1,
            Name = "MANAGER",
            Description = "Manager",
        },
        new Role
        {
            Id = 2,
            Name = "USER",
            Description = "USER",
        }
    );
        }
    }
}
