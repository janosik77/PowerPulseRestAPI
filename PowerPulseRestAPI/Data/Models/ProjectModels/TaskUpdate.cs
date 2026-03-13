using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.ProjectModels
{


    public class TaskUpdate
    {
        public long Id { get; set; }
        public long TaskId { get; set; }
        public TaskUpdateType UpdateType { get; set; }
        public string Content { get; set; } = null!;
        public long CreatedByUserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public ProjectTask Task { get; set; } = null!;
        public User CreatedByUser { get; set; } = null!;
    }

}
