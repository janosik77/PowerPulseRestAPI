namespace PowerPulseRestAPI.DTO.ToolDto.Request
{
    public class ToolAssignRequest
    {
        public long ToolId { get; set; }
        public ToolTargetDto Destination { get; set; } = null!;
        public string? Notes { get; set; }
    }
}
