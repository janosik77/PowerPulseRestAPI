using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.UsersModels;

namespace PowerPulseRestAPI.Data.Models.CustomerModels
{

    public class CustomerNote
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public CustomerNoteType NoteType { get; set; }
        public string Content { get; set; } = null!;
        public long CreatedByUserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public Customer? Customer { get; set; }
        public User? CreatedByUser { get; set; }
    }
}
