namespace PowerPulseRestAPI.DTO.MaterialDto.Request
{
    public class SubmitProjectInventoryItemRequest
    {
        public long MaterialId { get; set; }
        public decimal ActualQuantity { get; set; }
    }
}
