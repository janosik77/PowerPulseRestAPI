using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.VehicleDto.Responses
{
    public sealed class VehicleLoadToolDto
    {
        public long ToolAssetId { get; init; }
        public long ToolId { get; init; }
        public string ToolName { get; init; } = null!;
        public string? AssetTag { get; init; }
        public string? SerialNumber { get; init; }
        public ToolCondition Condition { get; init; }
        public ToolAssetStatus Status { get; init; }
        public DateTimeOffset AssignedAt { get; init; }
    }

}
