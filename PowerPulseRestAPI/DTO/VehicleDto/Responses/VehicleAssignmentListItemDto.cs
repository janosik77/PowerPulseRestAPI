namespace PowerPulseRestAPI.DTO.VehicleDto.Responses
{
    public class VehicleAssignmentListItemDto
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeFullName { get; set; } = null!;
        public DateTimeOffset AssignedAt { get; set; }
        public DateTimeOffset? ReturnedAt { get; set; }
        public bool IsActive { get; set; }
        public string? Note { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
