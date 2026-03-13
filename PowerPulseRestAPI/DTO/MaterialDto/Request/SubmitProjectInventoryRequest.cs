namespace PowerPulseRestAPI.DTO.MaterialDto.Request
{
    public class SubmitProjectInventoryRequest
    {
        public long ProjectId { get; set; }
        public List<SubmitProjectInventoryItemRequest> Items { get; set; } = new();
    }
}
