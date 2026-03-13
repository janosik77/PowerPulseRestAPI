namespace PowerPulseRestAPI.DTO.VehicleDto.Responses
{
    public sealed class VehicleUsageHistoryItemDto
    {
        public long AssignmentId { get; init; }
        public long EmployeeId { get; init; }
        public string EmployeeName { get; init; } = null!;
        public DateTimeOffset AssignedAt { get; init; }
        public DateTimeOffset? ReturnedAt { get; init; }
        public string? Note { get; init; }
    }

}
