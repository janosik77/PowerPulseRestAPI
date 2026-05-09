using Microsoft.EntityFrameworkCore;
using PowerPulseRestAPI.Data.Models.CustomerModels;
using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.Data.Models.KnowledgeModels;
using PowerPulseRestAPI.Data.Models.MaterialsModels;
using PowerPulseRestAPI.Data.Models.PersonModels;
using PowerPulseRestAPI.Data.Models.ProjectModels;
using PowerPulseRestAPI.Data.Models.ToolsModels;
using PowerPulseRestAPI.Data.Models.UsersModels;
using PowerPulseRestAPI.Data.Models.VehicleModels;


namespace PowerPulseRestAPI.Data
{
    public partial class PowerPulseContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PowerPulseContext).Assembly);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Project>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Person>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Employee>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Tool>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Vehicle>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Material>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<MaterialCategory>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<ToolCategory>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<KnowledgeCategory>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "ADMIN",
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
