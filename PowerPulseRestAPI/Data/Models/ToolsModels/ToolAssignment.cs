using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.EmployeeModels;
using PowerPulseRestAPI.Data.Models.UsersModels;


namespace PowerPulseRestAPI.Data.Models.ToolsModels
{

    public class ToolAssignment
    {
        public long Id { get; set; }
        public long ToolId { get; set; }
        public long EmployeeId { get; set; } 
        public DateTimeOffset AssignedAt { get; set; }
        public DateTimeOffset? ReturnedAt { get; set; }
        public string? Notes { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public Tool Tool { get; set; } = null!;
        public User CreatedByUser { get; set; } = null!;
        public Employee Employee { get; set; } = null!;
    }

}
