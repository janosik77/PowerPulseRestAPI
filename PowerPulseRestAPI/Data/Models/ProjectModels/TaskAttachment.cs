namespace PowerPulseRestAPI.Data.Models.ProjectModels
{


    public class TaskAttachment
    {
        public long Id { get; set; }
        public long TaskId { get; set; }
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public ProjectTask? Task { get; set; }
    }

}
