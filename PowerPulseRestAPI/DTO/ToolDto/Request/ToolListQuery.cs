namespace PowerPulseRestAPI.DTO.ToolDto.Request
{
    public sealed class ToolListQuery
    {
        public string? Q { get; set; }
        public long? CategoryId { get; set; }
        public bool? IsActive { get; set; }
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 50;
    }
}
