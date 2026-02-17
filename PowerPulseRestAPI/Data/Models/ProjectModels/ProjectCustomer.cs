using PowerPulseRestAPI.Data.Enums;
using PowerPulseRestAPI.Data.Models.CustomerModels;

namespace PowerPulseRestAPI.Data.Models.ProjectModels
{


    public class ProjectCustomer
    {
        public long ProjectId { get; set; }
        public long CustomerId { get; set; }
        public ProjectCustomerRelationshipType RelationshipType { get; set; }
        public bool? IsPrimaryOwner { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public Project? Project { get; set; }
        public Customer? Customer { get; set; }
    }

}
