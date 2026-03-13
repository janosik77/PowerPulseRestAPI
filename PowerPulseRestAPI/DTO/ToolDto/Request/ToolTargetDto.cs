using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.ToolDto.Request
{
    public sealed class ToolTargetDto
    {
        public ToolTransferTargetType TargetType { get; set; }
        public long TargetId { get; set; }
    }
}
