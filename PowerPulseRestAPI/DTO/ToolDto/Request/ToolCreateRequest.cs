using PowerPulseRestAPI.Data.Enums;

namespace PowerPulseRestAPI.DTO.ToolDto.Request
{
    public sealed class ToolCreateRequest
    {
        public string? Sku { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public long CategoryId { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? Barcode { get; set; }
        public string? SerialNumber { get; set; }
        public string Url { get; set; } = null!;
        public ToolCondition Condition { get; set; }
        public ToolStatus Status { get; set; }
        public DateOnly? PurchaseDate { get; set; }

        // jeśli nowy tool ma od razu trafić na magazyn
        public long? StorageLocationId { get; set; }
    }
}
