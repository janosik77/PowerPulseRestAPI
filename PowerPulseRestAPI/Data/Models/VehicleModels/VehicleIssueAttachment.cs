namespace PowerPulseRestAPI.Data.Models.VehicleModels
{


    public class VehicleIssueAttachment
    {
        public long Id { get; set; }
        public long VehicleIssueId { get; set; }
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public VehicleIssue? VehicleIssue { get; set; }
    }

}
