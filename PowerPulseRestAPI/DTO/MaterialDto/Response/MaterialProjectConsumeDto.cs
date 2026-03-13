namespace PowerPulseRestAPI.DTO.MaterialDto.Response
{
    public class MaterialProjectConsumeDto
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public long MaterialId { get; set; }
        public string MaterialName { get; set; } = null!;
        public decimal PreviousQuantity { get; set; }
        public decimal ActualQuantity { get; set; }
        public decimal ConsumedQuantity { get; set; }
        public string Unit { get; set; } = null!;
        public long? InvoiceId { get; set; }
        public DateTimeOffset? InvoicedAt { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
