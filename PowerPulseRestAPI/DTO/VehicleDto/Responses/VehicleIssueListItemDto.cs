using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.VehicleDto.Responses
{
    public class VehicleIssueListItemDto
    {
        public long Id { get; set; }
        public string ReportedByUserFullName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public GenericStatus Status { get; set; }
        public bool IsResolved { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
