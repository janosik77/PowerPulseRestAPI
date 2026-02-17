namespace PowerPulseRestAPI.Data.Models.InvoiceModels
{
    public class Position
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

    }
}
