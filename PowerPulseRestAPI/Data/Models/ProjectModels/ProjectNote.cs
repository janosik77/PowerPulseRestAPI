using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.ProjectModels
{


    public class ProjectNote
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public long? WorkSessionId { get; set; }
        public string Content { get; set; } = null!;
        public NoteType NoteType { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Project? Project { get; set; }
        public WorkSession? WorkSession { get; set; }
        public User? CreatedByUser { get; set; }
    }

}
