namespace PowerPulseRestAPI.Data.Models
{
    
    
        public class MaterialImage
        {
            public long Id { get; set; }
            public long MaterialId { get; set; }
            public string Url { get; set; } = null!;
            public bool IsPrimary { get; set; }
            public string? AltText { get; set; }
            public int SortOrder { get; set; }
            public DateTimeOffset CreatedAt { get; set; }

            public Material? Material { get; set; }
        }
    
}
