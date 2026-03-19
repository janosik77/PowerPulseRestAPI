using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.ProjectModels
{


    public class ProjectAttachment
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public string Url { get; set; } = null!;
        public string? Caption { get; set; }
        public AttachmentType AttachmentType { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public Project Project { get; set; } = null!;
        public User CreatedByUser { get; set; } = null!;
    }

}
