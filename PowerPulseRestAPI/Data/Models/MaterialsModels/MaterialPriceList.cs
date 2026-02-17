namespace PowerPulseRestAPI.Data.Models
{
    
    
        public class MaterialPriceList
        {
            public long Id { get; set; }
            public long MaterialId { get; set; }
            public decimal Price { get; set; }
            public string Currency { get; set; }
            public DateOnly ValidFrom { get; set; }
            public DateOnly? ValidTo { get; set; }
            public DateTimeOffset CreatedAt { get; set; }
            public DateTimeOffset UpdatedAt { get; set; }

            public Material? Material { get; set; }
        }
    
}
