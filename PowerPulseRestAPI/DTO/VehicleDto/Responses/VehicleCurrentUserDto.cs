namespace PowerPulseRestAPI.DTO.VehicleDto.Responses
{
    public sealed class VehicleCurrentUserDto
    {
        public long EmployeeId { get; init; }
        public string EmployeeName { get; init; } = null!; // np. "Jan Kowalski"
        public DateTimeOffset AssignedAt { get; init; }
        public string? Note { get; init; }
    }
}
