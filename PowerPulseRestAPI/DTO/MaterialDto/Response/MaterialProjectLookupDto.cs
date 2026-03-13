namespace PowerPulseRestAPI.DTO.MaterialDto.Response
{
    public class MaterialProjectLookupDto
    {
        public long ProjectId { get; set; }
        public string ProjectCode { get; set; } = null!;
        public string ProjectName { get; set; } = null!;
    }
}
