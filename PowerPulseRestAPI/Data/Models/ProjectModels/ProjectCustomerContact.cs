using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.CustomerModels;

namespace PowerPulseRestAPI.Data.Models.ProjectModels
{


    public class ProjectCustomerContact
    {
        public long ProjectId { get; set; }
        public long CustomerContactId { get; set; }
        public ProjectCustomerContactRole ContactRole { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public Project? Project { get; set; }
        public CustomerContact? CustomerContact { get; set; }
    }

}
