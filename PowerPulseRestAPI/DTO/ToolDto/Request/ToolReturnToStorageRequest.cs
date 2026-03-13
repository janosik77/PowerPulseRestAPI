namespace PowerPulseRestAPI.DTO.ToolDto.Request
{
    public sealed class ToolReturnToStorageRequest
    {
        public long StorageLocationId { get; set; }
        public string? Notes { get; set; }
    }
}
