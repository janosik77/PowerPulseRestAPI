namespace PowerPulseRestAPI.DTO.MaterialDto.Response
{
    public class MaterialTransferContextDto
    {
        public MaterialTransferEndpointType Type { get; set; }
        public long? ProjectId { get; set; }
        public string DisplayName { get; set; } = null!;
        public List<MaterialBalanceDto> Materials { get; set; } = new();
    }
}
