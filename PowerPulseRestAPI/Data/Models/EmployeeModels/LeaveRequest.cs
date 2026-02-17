using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.EmployeeModels
{


    public class LeaveRequest
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public LeaveType LeaveType { get; set; }
        public LeaveStatus Status { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string? Reason { get; set; }
        public long RequestedByUserId { get; set; }
        public long? ApprovedByUserId { get; set; }
        public DateTimeOffset? ApprovedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Employee? Employee { get; set; }
        public User? RequestedByUser { get; set; }
        public User? ApprovedByUser { get; set; }
    }

}
