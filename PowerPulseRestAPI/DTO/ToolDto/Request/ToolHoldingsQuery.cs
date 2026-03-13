using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.ToolDto.Request
{
    public sealed class ToolHoldingsQuery
    {
        public ToolTransferTargetType TargetType { get; set; } 
        public long TargetId { get; set; }
        public string? Q { get; set; }
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 100;
    }
}
