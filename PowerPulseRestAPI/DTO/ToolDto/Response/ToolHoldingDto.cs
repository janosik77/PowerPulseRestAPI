using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.ToolDto.Response
{
    public sealed class ToolHoldingDto
    {
        public long ToolId { get; set; }
        public string? Sku { get; set; }
        public string Name { get; set; } = null!;
        public string? SerialNumber { get; set; }
        public string? Barcode { get; set; }
        public string Url { get; set; } = null!;
        public ToolCondition Condition { get; set; }
        public ToolStatus Status { get; set; }

        public ToolTransferTargetType CurrentTargetType { get; set; }
        public long CurrentTargetId { get; set; }
    }
}
